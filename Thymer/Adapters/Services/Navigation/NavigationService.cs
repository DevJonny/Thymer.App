using System.Threading.Tasks;
using Thymer.Adapters.ViewModels;
using Xamarin.Forms;
using System.Linq;

namespace Thymer.Adapters.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IHaveMainPage _presentationRoot;
        private readonly IViewLocator _viewLocator;

        private INavigation Navigator => _presentationRoot.MainPage.Navigation;

        public NavigationService(IHaveMainPage presentationRoot, IViewLocator viewLocator)
        {
            _presentationRoot = presentationRoot;
            _viewLocator = viewLocator;
        }

        public async Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var page = _viewLocator.CreateAndBindPageFor<TViewModel>(out var viewModel);

            await viewModel.BeforeFirstShown();

            await Navigator.PushAsync(page);
        }

        public async Task NavigateBack()
        {
            var dismissing = Navigator.NavigationStack.Last().BindingContext as ViewModelBase;

            await Navigator.PopAsync();

            dismissing?.AfterDismissed();
        }

        public async Task NavigateBackToRoot()
        {
            var toDismiss = Navigator
                .NavigationStack
                .Skip(1)
                .Select(vw => vw.BindingContext)
                .OfType<ViewModelBase>()
                .ToArray();

            await Navigator.PopToRootAsync();

            foreach (ViewModelBase viewModel in toDismiss)
            {
                viewModel.AfterDismissed();
            }
        }        
    }
}
