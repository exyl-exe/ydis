using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Model.UserSettings;

namespace Ydis.Model.DataStructures
{
    /// <summary>
    /// YDIS objects that can be saved in a file.
    /// </summary>
    public abstract class YDISSerializable
    {
        [JsonIgnore] public const string VersionPropertyName = "Version";
        [JsonProperty(PropertyName = VersionPropertyName)] const int Version = YDISSettings.SerializationVersion;
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
