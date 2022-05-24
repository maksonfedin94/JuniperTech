using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Notes.Models;
using Notes.Services;
using Xamarin.Forms;

namespace Notes.ViewModels
{
    public class SalesTaxViewModel : BaseViewModel
    {
        List<string> errors = new List<string>();
        List<LineItem> lineItems = new List<LineItem>();
        readonly ITaxService taxService;
        readonly IDialogService dialogService;

        public ICommand TaxRateCommand { get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand OrderTaxRateCommand { get; set; }

        string zipCode;
        public string ZipCode
        {
            get => zipCode;
            set
            {
                zipCode = value;
                OnPropertyChanged();
            }
        }

        string countryAbbreviation;
        public string CountryAbbreviation
        {
            get => countryAbbreviation;
            set
            {
                countryAbbreviation = value;
                OnPropertyChanged();
            }
        }

        string toCountry;
        public string ToCountry
        {
            get => toCountry;
            set
            {
                toCountry = value;
                OnPropertyChanged();
            }
        }

        string toState;
        public string ToState
        {
            get => toState;
            set
            {
                toState = value;
                OnPropertyChanged();
            }
        }

        string toZip;
        public string ToZip
        {
            get => toZip;
            set
            {
                toZip = value;
                OnPropertyChanged();
            }
        }

        string shippingPrice;
        public string ShippingPrice
        {
            get => shippingPrice;
            set
            {
                shippingPrice = value;
                OnPropertyChanged();
            }
        }

        string fromCountry;
        public string FromCountry
        {
            get => fromCountry;
            set
            {
                fromCountry = value;
                OnPropertyChanged();
            }
        }

        string fromState;
        public string FromState
        {
            get => fromState;
            set
            {
                fromState = value;
                OnPropertyChanged();
            }
        }

        string fromZip;
        public string FromZip
        {
            get => fromZip;
            set
            {
                fromZip = value;
                OnPropertyChanged();
            }
        }

        string orderQuantity;
        public string OrderQuantity
        {
            get => orderQuantity;
            set
            {
                orderQuantity = value;
                OnPropertyChanged();
            }
        }

        string orderPrice;
        public string OrderPrice
        {
            get => orderPrice;
            set
            {
                orderPrice = value;
                OnPropertyChanged();
            }
        }

        public SalesTaxViewModel(ITaxService taxService = null, IDialogService dialogService = null)
        {
            this.taxService = taxService ?? DependencyService.Get<ITaxService>();
            this.dialogService = dialogService ?? DependencyService.Get<IDialogService>();
            TaxRateCommand = new Command(GetTaxRate);
            AddOrderCommand = new Command(AddOrder);
            OrderTaxRateCommand = new Command(GetOrderTaxRate);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        async void GetTaxRate()
        {
            if (string.IsNullOrEmpty(ZipCode) || ZipCode.Length < 5)
            {
                _ = dialogService.ShowBaseDialog("Tax", "Zip code shpould be 5 digits", "OK");
                return;
            }

            var res = await taxService.GetTaxForLocation(ZipCode, string.IsNullOrEmpty(CountryAbbreviation) ? "US" : CountryAbbreviation);
            if (res?.Rate == null)
            {
                _ = dialogService.ShowBaseDialog("Tax", "Wow something went wrong", "OK");
                return;
            }

            _ = dialogService.ShowBaseDialog("Tax", $"State Tax Rate Is {res.Rate.StateRate}\nCounty Tax Rate is {res.Rate.CityRate}\nCombined Tax Rate is {res.Rate.CombinedRate}", "OK");
        }

        void AddOrder()
        {
            lineItems.Add(new LineItem
            {
                Quantity = Convert.ToInt32(OrderQuantity),
                UnitPrice = Convert.ToDouble(OrderPrice)
            });

            _ = dialogService.ShowBaseDialog("Tax", $"Order has been added", "OK");
            OrderQuantity = string.Empty;
            OrderPrice = string.Empty;
        }

        async void GetOrderTaxRate()
        {
            if (!IsOrderValid() && errors.Any())
            {
                _ = dialogService.ShowBaseDialog("Tax", $"Please solve errors below:\n{string.Join("\n", errors)}", "OK");
                return;
            }

            lineItems.Add(new LineItem
            {
                Quantity = Convert.ToInt32(OrderQuantity),
                UnitPrice = Convert.ToDouble(OrderPrice)
            });

            double totalPrice = 0;
            foreach (var item in lineItems)
            {
                totalPrice += item.UnitPrice;
            }

            var res = await taxService.GetTaxForOrder(new Order
            {
                FromCountry = FromCountry,
                FromState = FromState,
                FromZip = FromZip,
                ToCountry = ToCountry,
                ToState = ToState,
                ToZip = ToZip,
                Amount = lineItems.Any() ? totalPrice : Convert.ToDouble(OrderPrice),
                Shipping = Convert.ToDouble(ShippingPrice),
                LineItems = lineItems
            });

            ClearFields();

            _ = dialogService.ShowBaseDialog("Tax", $"Total Tax for order is {res.Tax.AmountToCollect}", "OK");
        }

        // ugly validator
        bool IsOrderValid()
        {
            errors.Clear();

            if (string.IsNullOrEmpty(ToCountry) || ToCountry.Length != 2)
                errors.Add("ToCountry");
            if (string.IsNullOrEmpty(ToState) || ToState.Length != 2)
                errors.Add("ToState");
            if (string.IsNullOrEmpty(ToZip) || ToZip.Length != 5)
                errors.Add("ToZip");
            if (string.IsNullOrEmpty(FromCountry) || FromCountry.Length != 2)
                errors.Add("FromCountry");
            if (string.IsNullOrEmpty(FromState) || FromState.Length != 2)
                errors.Add("FromState");
            if (string.IsNullOrEmpty(FromZip) || FromZip.Length != 5)
                errors.Add("FromZip");
            if (string.IsNullOrEmpty(OrderQuantity))
                errors.Add("OrderQuantity");
            if (string.IsNullOrEmpty(OrderPrice))
                errors.Add("OrderPrice");

            return !string.IsNullOrEmpty(ToCountry) &&
                ToCountry.Length == 2 &&
                !string.IsNullOrEmpty(ToState) &&
                ToState.Length == 2 &&
                !string.IsNullOrEmpty(ToZip) &&
                ToZip.Length == 5 &&
                !string.IsNullOrEmpty(FromCountry) &&
                FromCountry.Length == 2 &&
                !string.IsNullOrEmpty(FromState) &&
                FromState.Length == 2 &&
                !string.IsNullOrEmpty(FromZip) &&
                FromZip.Length == 5 &&
                !string.IsNullOrEmpty(OrderQuantity) &&
                !string.IsNullOrEmpty(OrderPrice) &&
                !string.IsNullOrEmpty(ShippingPrice);
        }

        void ClearFields()
        {
            ToCountry = string.Empty;
            ToState = string.Empty;
            ToZip = string.Empty;
            FromCountry = string.Empty;
            FromState = string.Empty;
            FromZip = string.Empty;
            OrderQuantity = string.Empty;
            OrderPrice = string.Empty;
            ShippingPrice = string.Empty;

            lineItems.Clear();
        }
    }
}
