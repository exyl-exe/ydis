using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.ViewModels.CommonViewModels;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    public class GraphTabViewModel
    {
        public LevelDataGridViewModel DataGrid { get; set; }
        public GraphTabViewModel(SessionGroup g)
        {
            DataGrid = new LevelDataGridViewModel(g);
        }
    }
}
