using Notes.Services;
using Xamarin.Forms;

namespace Notes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            DependencyService.Register<IRestClient, RestClient>();
            DependencyService.Register<ITaxService, TaxService>();
            DependencyService.Register<IDialogService, DialogService>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
