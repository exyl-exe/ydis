using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.ViewModels
{
    public interface ReplaceableViewViewModel
    {
        BaseViewModel CurrentView { get; set; }

        void ReplaceView(BaseViewModel m);
    }
}
