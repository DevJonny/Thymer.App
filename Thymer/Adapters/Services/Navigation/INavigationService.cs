using System.Threading.Tasks;
using Thymer.Adapters.ViewModels;

namespace Thymer.Adapters.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase;

        /// <summary>
        /// Navigate to the previous item in the navigation stack
        /// </summary>
        Task NavigateBack();

        /// <summary>
        /// Navigate back to the element at the root of the navigation stack
        /// </summary>
        Task NavigateBackToRoot();
    }
}
