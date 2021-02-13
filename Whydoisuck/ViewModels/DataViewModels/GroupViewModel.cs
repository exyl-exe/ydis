using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Model.DataStructures;

namespace Ydis.ViewModels.DataViewModels
{
    /// <summary>
    /// Simple view models to display information about session groups.
    /// </summary>
    public class GroupViewModel : BaseViewModel
    {
        /// <summary>
        /// Name of the group.
        /// </summary>
        public string GroupName
        {
            get
            {
                return Group.DisplayedName;
            } set
            {
                if (value.Trim().Length > 0)
                {
                    Group.DisplayedName = value;
                }
            }
        }
        private SessionGroup Group { get; set; }

        public GroupViewModel(SessionGroup group)
        {
            Group = group;
            Group.OnDisplayedNameChanges += UpdateName;
        }

        // Called to update elements using displayed name
        private void UpdateName()
        {
            OnPropertyChanged(nameof(GroupName));
        }
    }
}
