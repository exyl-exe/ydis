using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ydis.ViewModels
{
    /// <summary>
    /// Base view model for every viewmodel.
    /// Can be used to notify views about a change.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the view that the given property has been updated.
        /// </summary>
        /// <param name="propertyName">The name of the updated property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Method to resfresh the view model, to adapt to changes made to the model
        /// </summary>
        public virtual void UpdateFromModel() { }
    }
}
