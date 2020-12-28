using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.ViewModels
{
	/// <summary>
	/// Class that allows to prepare a view model for views that isn't loaded
	/// until the view must be displayed (improves loading times)
	/// </summary>
    public class DelayedViewModel : BaseViewModel
    {
		/// <summary>
		/// Delegate for a function generating the view model when needed
		/// </summary>
		public delegate BaseViewModel GeneratorFunc();
		/// <summary>
		/// Delayed view model, is loaded on first access
		/// </summary>
		public BaseViewModel ViewModel
		{
			get
			{
				if (!_loaded)
				{
					_loaded = true;
					_viewModel = _generator();
				}
				return _viewModel;
			}
		}

		// function for generating the view model
		private GeneratorFunc _generator;
		// wether the view model is loaded or not
		private bool _loaded;
		// generated view model (null if not loaded)
		private BaseViewModel _viewModel;

		public DelayedViewModel(GeneratorFunc generator)
		{
			_generator = generator;
			_loaded = false;
			_viewModel = null;
		}

		public override void UpdateFromModel()
		{
			if (_loaded)
			{
				ViewModel.UpdateFromModel();
			}
		}

		/// <summary>
		/// Checks content instead of itself if a view model is given
		/// </summary>
		public override bool Equals(object obj)
		{
			if(obj is BaseViewModel bm)
			{
				return ViewModel.Equals(bm);
			} else
			{
				return base.Equals(obj);
			}
		}

	}
}
