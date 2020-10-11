using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    static class SerializationManager
    {
        public static void Serialize(IWDISSerializable item, string filePath)
        {
            var serializedItem = item.GetSerializedObject();
            SafeFile.WriteAllText(filePath, serializedItem);
        }

        public static T Deserialize<T>(string filePath) where T : IWDISSerializable, new()
        {
            var item = new T();
            var value = SafeFile.ReadAllText(filePath);
            item.InitFromSerialized(value);
            return item;
        }
    }
}
