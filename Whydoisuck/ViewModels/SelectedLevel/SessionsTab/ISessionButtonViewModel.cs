using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Model.DataStructures;

namespace Ydis.ViewModels.SelectedLevel.SessionsTab
{
    public interface ISessionButtonViewModel
    {
        ISession Session { get; }
    }
}
