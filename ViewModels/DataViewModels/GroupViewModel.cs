using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.DataViewModels
{
    /// <summary>
    /// Simple view models to display information about session groups.
    /// </summary>
    public class GroupViewModel : BaseViewModel
    {
        /// <summary>
        /// Name of the group.
        /// </summary>
        public string GroupName => Group.GroupName;
        private SessionGroup Group { get; set; }

        public GroupViewModel(SessionGroup group)
        {
            Group = group;
        }
    }
}
