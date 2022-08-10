using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

using Microsoft.WindowsAPICodePack.Dialogs;

using Newtonsoft.Json;
using The_Way_Point_Editor;
using The_Way_Point_Editor.Models;

namespace The_Way_Point_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window
    {
        private static List<WaypointClass> _abbreviationClasses = null;
        private static WaypointsExtended _waypoints = null;
        private static bool _isWaypointDefinitionUsed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadWaypoints()
        {
            string initialDirectory = "";
            if (initialDirectory == "")
            {
                //KnownFolder savedGamesknownFolder = new KnownFolder(KnownFolderType.SavedGames);
                initialDirectory = System.IO.Path.Combine(Environment.GetEnvironmentVariable("userprofile"), "Documents", "DCS The Way");
            }
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = initialDirectory,
                DefaultFileName = "waypoints.json",
                DefaultExtension = "json",
                IsFolderPicker = false,
                EnsureFileExists = true,
                Title = "Select Waypoints File Exported from \"The Way\""
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                JsonRead(dialog.FileName);
            }
        }
        private void SaveWaypoints()
        {
            string initialDirectory = "";
            if (initialDirectory == "")
            {
                //KnownFolder savedGamesknownFolder = new KnownFolder(KnownFolderType.SavedGames);
                initialDirectory = System.IO.Path.Combine(Environment.GetEnvironmentVariable("userprofile"), "Documents", "DCS The Way");
            }
            CommonSaveFileDialog dialog = new CommonSaveFileDialog
            {
                InitialDirectory = initialDirectory,
                DefaultFileName = "waypoints_bak.json",
                DefaultExtension = "json",
                EnsureFileExists = false,
                Title = "Select Waypoints File to Save Waypoints for \"The Way\""
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                JsonWrite(dialog.FileName);
            }
        }

        public void JsonRead(string waypointFilename) 
        {
            try
            {
                string json = System.IO.File.ReadAllText(waypointFilename);
                var wpFile = JsonConvert.DeserializeObject<WaypointsExtended>(json);
                _waypoints = wpFile as WaypointsExtended;
                /// we want to populate the Ident Description for each waypoint before
                /// giving the ItemSource but we need to get the abbreviation dicionary first.

                _abbreviationClasses = GetAbbreviationClasses(wpFile.module);

                if (_abbreviationClasses.Count > 0)
                {
                        cb_WaypointType.ItemsSource = _abbreviationClasses;
                        cb_WaypointType.SelectedItem = cb_WaypointType.Items[0];
                        WaypointClass wpc = (WaypointClass) cb_WaypointType.SelectedItem;
                        List<Abbreviation> t = GetAbbreviationClass(wpc.classname);
                        if (t.Count > 0)
                        {
                            cb_WaypointIdent.ItemsSource = t;
                            cb_WaypointIdent.SelectedItem = cb_WaypointIdent.Items[0];
                        }
                    WaypointCombos.Visibility = Visibility.Visible;
                }
                if (wpFile != null)
                {
                    foreach (WaypointExtended wp in _waypoints.waypoints)
                    {
                        wp.WaypointTypeIdentificationDescription = GetAbbreviationDescription(wp.WaypointType, wp.WaypointTypeIdentification);
                    }
                    listboxWaypoints.ItemsSource = wpFile.waypoints;
                    TB_listboxLabel.Text = "Waypoints For Module:  " + wpFile.module;
                    lbGrid.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void JsonWrite(string waypointFilename)
        {
            try
            {
                Waypoints wps = _waypoints.DeExtend();
                string wpFile = JsonConvert.SerializeObject(_waypoints.DeExtend());
                System.IO.File.WriteAllText(waypointFilename, wpFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            switch (menuItem.Name)
            {
                case "Open_Menu":
                    LoadWaypoints();
                    break;
                case "Save_Menu":
                    SaveWaypoints();
                    break;
                case "SaveAs_Menu":
                    break;
                case "Close_Menu":
                    _abbreviationClasses = null;
                    _waypoints = null;
                    WaypointCombos.Visibility = Visibility.Hidden;
                    listboxWaypoints.ItemsSource = null;
                    listboxWaypoints.Items.Clear();
                    listboxWaypoints.Items.Refresh();
                    TB_listboxLabel.Text = "Waypoints For Module:";
                    lbGrid.Visibility = Visibility.Hidden;
                    break;
                case "Exit_Menu":
                    this.Close();
                    return;
                    break;
                case "Add_Waypoint":
                    if (listboxWaypoints.SelectedItem != null)
                    {
                        List<WaypointExtended> wps = listboxWaypoints.ItemsSource as List<WaypointExtended>;
                        wps.Insert(listboxWaypoints.SelectedIndex+1, new WaypointExtended());
                        listboxWaypoints.SelectedIndex++;
                        listboxWaypoints.Items.Refresh();
                    }
                    break;
                case "Clone_Waypoint":
                    if (listboxWaypoints.SelectedItem != null)
                    {
                        List<WaypointExtended> wps = listboxWaypoints.ItemsSource as List<WaypointExtended>;
                        WaypointExtended origWp = listboxWaypoints.SelectedItem as WaypointExtended;
                        wps.Insert(listboxWaypoints.SelectedIndex + 1, origWp.CreateDeepCopy());
                        listboxWaypoints.SelectedIndex++;
                        listboxWaypoints.Items.Refresh();
                    }
                    break;
                case "Delete_Waypoint":
                    if (listboxWaypoints.SelectedItem != null)
                    {
                        List<WaypointExtended> wps = listboxWaypoints.ItemsSource as List<WaypointExtended>;
                        wps.Remove(listboxWaypoints.SelectedItem as WaypointExtended);
                        listboxWaypoints.Items.Refresh();
                    }
                    break;
                case "Help":
                    break;
                case "About":
                    break;


            }
        }
        private List<WaypointClass> GetAbbreviationClasses(String abbreviationsModule)
        {
            String json = System.IO.File.ReadAllText(@".\resources\abbreviations.json");
            var abbreviationAll = JsonConvert.DeserializeObject<Modules>(json);
            if (abbreviationAll != null && abbreviationAll.modules.Exists(r => r.module.Equals(abbreviationsModule)))
            {
                return abbreviationAll.modules.Find(r => r.module.Equals(abbreviationsModule)).abbreviationdata[0].waypointclasses;
            }
            else
            {
                return new List<WaypointClass>();
            }
        }
        private List<Abbreviation> GetAbbreviationClass(String abbreviationsClass)
        {
            if (_abbreviationClasses != null && _abbreviationClasses.Exists(r => r.classname.Equals(abbreviationsClass)))
            {
                return _abbreviationClasses.Find(r => r.classname.Equals(abbreviationsClass)).abbreviations;
            } else
            {
                return new List<Abbreviation>();
            }
        }

        private string GetAbbreviationDescription(String type, String code)
        {
            string description = "";
            if(_abbreviationClasses != null && _abbreviationClasses.Exists(r => r.classname.Equals(type)))
            {
                List<Abbreviation> abbreviationsForWaypointClass = _abbreviationClasses.Find(r => r.classname.Equals(type)).abbreviations;
                if(abbreviationsForWaypointClass != null && abbreviationsForWaypointClass.Exists(r => r.code.Equals(code)))
                {
                    description = abbreviationsForWaypointClass.Find(r => r.code.Equals(code)).description;
                }
            } 
            return description;
        }

        private void CBSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if(cb != null && cb.Name == "cb_WaypointType" && cb.SelectedItem != null)
            {
                WaypointClass wpc = (WaypointClass)cb.SelectedItem;
                List<Abbreviation> t = GetAbbreviationClass(wpc.classname);
                if (t.Count > 0)
                {
                    cb_WaypointIdent.ItemsSource = t;
                    cb_WaypointIdent.SelectedItem = cb_WaypointIdent.Items[0];
                }
            }
        }

        private void LISelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if(lb.SelectedItem != null)
            {
                WaypointExtended wp = lb.SelectedItem as WaypointExtended;  
                if (wp != null)
                {
                    WaypointClass wpc = cb_WaypointType.SelectedItem as WaypointClass;
                    if(wpc != null)
                    {
                        if ((bool) _isWaypointDefinitionUsed)
                        {
                            wp.WaypointType = wpc.classname;
                            Abbreviation abr = cb_WaypointIdent.SelectedItem as Abbreviation;
                            wp.WaypointTypeIdentification = abr.code;
                            wp.WaypointTypeIdentificationDescription = GetAbbreviationDescription(wp.WaypointType, wp.WaypointTypeIdentification);
                            lb.Items.Refresh();
                        } else
                        {
                            if (_abbreviationClasses != null && _abbreviationClasses.Exists(r => r.classname.Equals(wp.WaypointType)))
                            {
                                cb_WaypointType.SelectedItem = _abbreviationClasses.Find(r => r.classname.Equals(wp.WaypointType));
                                cb_WaypointIdent.ItemsSource = _abbreviationClasses.Find(r => r.classname.Equals(wp.WaypointType)).abbreviations;
                                cb_WaypointIdent.Items.Refresh();
                                cb_WaypointIdent.SelectedItem = _abbreviationClasses.Find(r => r.classname.Equals(wp.WaypointType)).abbreviations.Find(r => r.code.Equals(wp.WaypointTypeIdentification));
                            }
                        }
                    }
                }
            }
        }

        private void ButtonArrowClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                switch (button.Name)
                {
                    case "ButtonUpArrow":
                        if (listboxWaypoints.SelectedItem != null)
                        {
                            List<WaypointExtended> wps = listboxWaypoints.ItemsSource as List<WaypointExtended>;
                            WaypointExtended origWp = listboxWaypoints.SelectedItem as WaypointExtended;
                            int currentPosition = listboxWaypoints.SelectedIndex;
                            if (currentPosition > 0)
                            {
                                wps.Remove(origWp);
                                wps.Insert(currentPosition - 1, origWp);
                                listboxWaypoints.SelectedIndex--;
                                listboxWaypoints.Items.Refresh();
                            }
                        }
                        break;
                    case "ButtonDownArrow":
                        if (listboxWaypoints.SelectedItem != null)
                        {
                            List<WaypointExtended> wps = listboxWaypoints.ItemsSource as List<WaypointExtended>;
                            WaypointExtended origWp = listboxWaypoints.SelectedItem as WaypointExtended;
                            int currentPosition = listboxWaypoints.SelectedIndex;
                            if (currentPosition <  wps.Count-1)
                            {
                                wps.Remove(origWp);
                                wps.Insert(currentPosition + 1, origWp);
                                listboxWaypoints.SelectedIndex++;
                                listboxWaypoints.Items.Refresh();
                            }
                        }
                        break;
                }
            }
        }
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox; 
            if (checkBox != null)
            {
                _isWaypointDefinitionUsed = (bool) checkBox.IsChecked;
                cb_WaypointType.IsEnabled = _isWaypointDefinitionUsed;
                cb_WaypointIdent.IsEnabled = _isWaypointDefinitionUsed;              
            }
        }

        protected void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            item.IsSelected = true;
        }
        protected void DeselectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            item.IsSelected = false;
        }
    }
}
