using System.Threading.Tasks;
using MvvmHelpers;

namespace Thymer.Adapters.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateTo<TViewModel>() where TViewModel : BaseViewModel;

        Task NavigateBackToRoot();

        Task NavigateBack();
    }
}
