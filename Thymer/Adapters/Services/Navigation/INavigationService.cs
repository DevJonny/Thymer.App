using System.Threading.Tasks;
using Thymer.Adapters.ViewModels;

namespace Thymer.Adapters.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase;

        /// <summary>
        /// Navigate back to the element at the root of the navigation stack
        /// </summary>
        Task NavigateBackToRoot();
    }
}
