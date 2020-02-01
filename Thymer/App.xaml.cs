using System.ComponentModel;
using Thymer.Adapters.Services.Navigation;
using Thymer.Adapters.Views;
using Xamarin.Forms;

namespace Thymer
{
    public partial class App : Application, IHaveMainPage
    {

        public App()
        {
            InitializeComponent();
            
            ContainerRegistration.Register();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
