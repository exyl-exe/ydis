using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Model.DataStructures
{
    /// <summary>
    /// Serializable WDIS class.
    /// </summary>
    public abstract class IWDISSerializable
    {
        [JsonIgnore] public const string VersionPropertyName = "Version";
        [JsonIgnore] public const int CurrentVersion = 2;
        [JsonProperty(PropertyName = VersionPropertyName)] const int Version = CurrentVersion;//TODO config?

        /// <summary>
        /// Checks if a version of an object is compatible with the current version
        /// </summary>
        /// <param name="version">The version to check compatibility for</param>
        /// <returns>Wether the given version is compatible with the current version</returns>
        public abstract bool CurrentVersionCompatible(int version);
        /// <summary>
        /// Updates an old version of a IWDISSerializable object
        /// </summary>
        /// <param name="oldObject">The object to update</param>
        public abstract void UpdateOldVersion(ref JObject oldObject);
        /// <summary>
        /// Serializes the object in the JSON format.
        /// </summary>
        /// <returns>A string containing the json object.</returns>
        public abstract string Serialize();
        /// <summary>
        /// Initiliaze the object based on its json string.
        /// </summary>
        /// <param name="value">A string containing the json object.</param>
        public abstract void Deserialize(string value);
    }
}
