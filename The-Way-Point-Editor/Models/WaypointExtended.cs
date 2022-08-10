using System.Collections.Generic;
using Newtonsoft.Json;

namespace The_Way_Point_Editor.Models
{
    public class WaypointsExtended:Waypoints
    {
        [JsonProperty("waypoints")]
        public List<WaypointExtended> waypoints { get; set; }
        public Waypoints DeExtend()
        {
            Waypoints tempWaypoints = new Waypoints();
            tempWaypoints.source = source;
            tempWaypoints.commit = commit;
            tempWaypoints.comments = comments;
            tempWaypoints.vehicles = vehicles;
            tempWaypoints.version = version;
            tempWaypoints.type = type;
            tempWaypoints.waypoints = new List<Waypoint>();
            foreach(WaypointExtended wp in waypoints)
            {
                tempWaypoints.waypoints.Add(wp.DeExtend());
            }

            return tempWaypoints;
        }

    }
    public class WaypointExtended : Waypoint
    {
        [JsonProperty("WaypointTypeIdentificationDescription")]
        public string WaypointTypeIdentificationDescription { get; set; }
        public Waypoint DeExtend()
        {
            Waypoint tempWaypoint = new Waypoint();
            tempWaypoint.Latitude = Latitude;
            tempWaypoint.Longitude = Longitude;
            tempWaypoint.Elevation = Elevation;
            tempWaypoint.LatitudeHemisphere = LatitudeHemisphere;  
            tempWaypoint.LongitudeHemisphere = LongitudeHemisphere;
            tempWaypoint.WaypointType = WaypointType;
            tempWaypoint.WaypointTypeIdentification = WaypointTypeIdentification;
            tempWaypoint.WaypointTypeFreeText = WaypointTypeFreeText;   
            return tempWaypoint;
        }
    }
}
