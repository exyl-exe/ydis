using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataModel.SerializedData
{
    public class SerializedLevel : IWdisSerialized
    {
        [JsonProperty(PropertyName = "ID")] public int ID { get; set; }
        [JsonProperty(PropertyName = "IsOnline")] public bool IsOnline { get; set; }
        [JsonProperty(PropertyName = "OriginalID")] public int OriginalID { get; set; }
        [JsonProperty(PropertyName = "IsOriginal")] public bool IsOriginal { get; set; }
        [JsonProperty(PropertyName = "Name")] public string Name { get; set; }
        [JsonProperty(PropertyName = "Revision")] public int Revision { get; set; }
        [JsonProperty(PropertyName = "PhysicalLength")] public float PhysicalLength { get; set; }
        [JsonProperty(PropertyName = "IsCustomMusic")] public bool IsCustomMusic { get; set; }
        [JsonProperty(PropertyName = "MusicID")] public int MusicID { get; set; }
        [JsonProperty(PropertyName = "OfficialMusicID")] public int OfficialMusicID { get; set; }
        [JsonProperty(PropertyName = "MusicOffset")] public float MusicOffset { get; set; }

        public SerializedLevel() { }

        public SerializedLevel(Level l)
        {
            ID = l.ID;
            IsOnline = l.IsOnline;
            OriginalID = l.OriginalID;
            IsOriginal = l.IsOriginal;
            Name = l.Name;
            Revision = l.Revision;
            PhysicalLength = l.PhysicalLength;
            IsCustomMusic = l.IsCustomMusic;
            MusicID = l.MusicID;
            OfficialMusicID = l.OfficialMusicID;
            MusicOffset = l.MusicOffset;
        }

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
