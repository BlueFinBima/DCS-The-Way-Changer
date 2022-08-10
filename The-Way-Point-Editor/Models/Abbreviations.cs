using System.Collections.Generic;
using Newtonsoft.Json;

namespace The_Way_Point_Editor.Models
{
        public class Modules
        {
            [JsonProperty("modules")]
            public List<Module> modules { get; set; }
        }
        public class Module
        {
            [JsonProperty("module")]
            public string module { get; set; }
            [JsonProperty("abbreviationdata")]
            public List<AbbreviationData> abbreviationdata { get; set; }
        }
        public class AbbreviationData
        {
            [JsonProperty("waypointclasses")]
            public List<WaypointClass> waypointclasses { get; set; }
        }
        public class WaypointClass
        {
            [JsonProperty("classname")]
            public string classname { get; set; }
            [JsonProperty("abbreviations")]
            public List<Abbreviation> abbreviations { get; set; }
        }
        public class Abbreviation
        {
            [JsonProperty("code")]
            public string code { get; set; }
            [JsonProperty("description")]
            public string description { get; set; }
        }
}
