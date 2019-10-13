using Thymer.Adapters.ViewModels;
using Xamarin.Forms;

namespace Thymer.Adapters.Services.Navigation
{
    public interface IViewLocator
    {
        Page CreateAndBindPageFor<TViewModel>(out TViewModel viewModel) where TViewModel : ViewModelBase;
    }
}
