using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataModel
{
    public interface IWDISSerializable
    {
        string GetSerializedObject();
        void InitFromSerialized(string value);
    }
}
