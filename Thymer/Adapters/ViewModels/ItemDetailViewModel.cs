using Thymer.Adapters.Services.Navigation;
using Thymer.Models;

namespace Thymer.Adapters.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase
    {
        public Item Item { get; set; }
        
        public ItemDetailViewModel(INavigationService navigationService, Item item = null) : base(navigationService)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
