using System;
using Thymer.Adapters.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace Thymer.Adapters.Services.Navigation
{
    public class ViewLocator : IViewLocator
    {
        public Page CreateAndBindPageFor<TViewModel>(out TViewModel viewModel) where TViewModel : ViewModelBase
        {
            var pageType = FindPageForViewModel(typeof(TViewModel));

            var page = TinyIoCContainer.Current.Resolve(pageType) as Page;

            viewModel = TinyIoCContainer.Current.Resolve<TViewModel>(); 
            
            page.BindingContext = viewModel;

            return page;
        }

        private static Type FindPageForViewModel(Type viewModelType)
        {
            var pageTypeName = viewModelType
                .AssemblyQualifiedName
                .Replace("Thymer.Adapters.ViewModels", "Thymer.Adapters.Views")
                .Replace("ViewModel", "Page");

            var pageType = Type.GetType(pageTypeName);
            if (pageType == null)
                throw new ArgumentException(pageTypeName + " type does not exist");

            return pageType;
        }
    }
}
