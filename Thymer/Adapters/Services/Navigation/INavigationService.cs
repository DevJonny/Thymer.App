using System.Threading.Tasks;
using MvvmHelpers;

namespace Thymer.Adapters.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateTo<TViewModel>(params (string name, string value)[] queryParams) where TViewModel : BaseViewModel;

        Task NavigateBackToRoot();

        Task NavigateBack();
    }
}
