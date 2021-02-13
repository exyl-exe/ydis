using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ydis.ViewModels
{
    /// <summary>
    /// Interface for viewmodels of views which have a part than can be swapped.
    /// </summary>
    public interface IReplaceableViewViewModel
    {
        /// <summary>
        /// Method to change a part of the view
        /// </summary>
        /// <param name="m">New model of the transformables part of the view</param>
        void ReplaceView(BaseViewModel m);
    }
}
