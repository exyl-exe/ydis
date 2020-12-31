using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.UserSettings;

namespace Whydoisuck.Model.DataStructures
{
    /// <summary>
    /// WDIS objects that can be converted to a format used for saving data.
    /// </summary>
    public abstract class WDISSerializable
    {
        [JsonIgnore] public const string VersionPropertyName = "Version";
        [JsonProperty(PropertyName = VersionPropertyName)] const int Version = WDISSettings.SerializationVersion;
        /// <summary>
        /// Converts the object into a json object
        /// </summary>
        public JToken ToJsonObject()
        {
            var jo = JToken.FromObject(this);
            return jo;
        }
        /// <summary>
        /// Initiliaze the object based on its json object.
        /// </summary>
        public void FromJsonObject(JToken value)
        {
            var reader = value.CreateReader();
            JsonSerializer.CreateDefault().Populate(reader, this);
        }
    }
}
