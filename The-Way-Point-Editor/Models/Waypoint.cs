using System.Collections.Generic;
using Newtonsoft.Json;

namespace The_Way_Point_Editor.Models
{
    public class Waypoints
    {
        [JsonProperty("source")]
        public string source { get; set; }
        [JsonProperty("comments")]
        public string comments { get; set; }
        [JsonProperty("module")]
        public string module { get; set; }
        [JsonProperty("commit")]
        public string commit { get; set; }
        [JsonProperty("type")]
        public string type   { get; set; }
        [JsonProperty("version")]
        public string version { get; set; }
        [JsonProperty("vehicles")]
        public List<Vehicle> vehicles { get; set; }
        [JsonProperty("waypoints")]
        public List<Waypoint> waypoints { get; set; }

    }
    public class Waypoint
    {
        [JsonProperty("LatitudeHemisphere")]
        public string LatitudeHemisphere { get; set; }
        [JsonProperty("Latitude")]
        public string Latitude { get; set; }
        [JsonProperty("LongitudeHemisphere")]
        public string LongitudeHemisphere { get; set; }
        [JsonProperty("Longitude")]
        public string Longitude { get; set; }
        [JsonProperty("Elevation")]
        public string Elevation { get; set; }
        [JsonProperty("WaypointType")]
        public string WaypointType { get; set; }
        [JsonProperty("WaypointTypeIdentification")]
        public string WaypointTypeIdentification { get; set; }
        [JsonProperty("WaypointTypeFreeText")]
        public string WaypointTypeFreeText { get; set; }
    }
    public class Vehicle
    {
        [JsonProperty("vehicle")]
        public string vehicle { get; set; }
    }
}
