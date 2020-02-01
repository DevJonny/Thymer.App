using MvvmHelpers;
using Thymer.Models;
using Thymer.Services;
using TinyIoC;

namespace Thymer.Adapters.ViewModels
{
    public abstract class ViewModelBase : BaseViewModel
    {
        public IDataStore<Item> DataStore => TinyIoCContainer.Current.Resolve<IDataStore<Item>>();
    }
}
