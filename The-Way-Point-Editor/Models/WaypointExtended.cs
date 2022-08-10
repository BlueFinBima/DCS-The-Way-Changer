using System.Collections.Generic;
using Newtonsoft.Json;

public interface IPrototype<T>
{
    T CreateDeepCopy();
}

namespace The_Way_Point_Editor.Models
{

    public class WaypointsExtended : Waypoints
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
            foreach (WaypointExtended wp in waypoints)
            {
                tempWaypoints.waypoints.Add(wp.DeExtend());
            }

            return tempWaypoints;
        }

    }

    public class WaypointExtended : Waypoint, IPrototype<WaypointExtended>
    {
        [JsonProperty("WaypointTypeIdentificationDescription")]
        public string WaypointTypeIdentificationDescription { get; set; }

        public WaypointExtended()
        {
            Latitude = "00.00000000";
            Longitude = "00.00000000";
            Elevation = "0.00";
            LatitudeHemisphere = "NORTH";
            LongitudeHemisphere = "EAST";
            WaypointType = "WP";
            WaypointTypeIdentification = "";
            WaypointTypeFreeText = "";
        }

        public WaypointExtended CreateDeepCopy()
        {
            var wp = (WaypointExtended)MemberwiseClone();
            return wp;
        }
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


