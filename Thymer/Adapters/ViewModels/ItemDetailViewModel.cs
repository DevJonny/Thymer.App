using MvvmHelpers;
using Thymer.Adapters.Services.Navigation;
using Thymer.Models;

namespace Thymer.Adapters.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        
        public ItemDetailViewModel(INavigationService navigationService, Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
