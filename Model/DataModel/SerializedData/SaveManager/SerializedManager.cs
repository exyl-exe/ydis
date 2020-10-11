using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.DataModel.SerializedData.SaveManager
{
    class SerializedManager : IWdisSerialized
    {
        [JsonProperty(PropertyName = "Entries")] public List<SerializedManagerEntry> Entries { get; set; }

        public SerializedManager(string value)
        {
            var jsonObject = JObject.Parse(value);
            //TODO throw exception if deserialization fails
            Update(jsonObject["Version"].ToObject<int>(), jsonObject);
            Entries = jsonObject["Entries"].ToObject<List<SerializedManagerEntry>>();
        }

        public SerializedManager(SessionManager m)
        {
            Entries = m.Entries.Select(e => new SerializedManagerEntry(e)).ToList();
        }

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public JObject Update(int version, JObject manager)
        {
            return manager;
        }
    }
}
