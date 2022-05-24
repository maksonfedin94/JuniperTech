using Notes.ViewModels;
using Xamarin.Forms;

namespace Notes.Views
{
    public partial class SalesTax : ContentPage
    {
        readonly SalesTaxViewModel vm;
        public SalesTax()
        {
            InitializeComponent();            

            BindingContext = vm = new SalesTaxViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm.OnDisappearing();
        }
    }
}
