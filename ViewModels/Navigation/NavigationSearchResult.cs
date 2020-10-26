using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.Navigation
{
    public class NavigationSearchResult
    {
        public NavigatorCommand UpdateCommand { get; set; }
        public SessionGroup Group { get; set; }

        public NavigationSearchResult(SessionGroup g, NavigatorCommand updateCommand)
        {
            Group = g;
            UpdateCommand = updateCommand;
        }
    }
}
