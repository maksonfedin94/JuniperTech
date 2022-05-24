using System.Threading.Tasks;
using Notes.Models;
using Notes.StringConstants;
using Xamarin.Forms;

namespace Notes.Services
{
    public class TaxService : ITaxService
    {
        private readonly IRestClient restClient;

        public TaxService()
        {
            restClient = DependencyService.Get<IRestClient>();
        }

        public async Task<TaxRate> GetTaxForLocation(string zip, string country)
        {
            var result = await restClient.GetAsync<TaxRate>($"{Constants.GetRatesForLocationUrl}{zip}?country={country}");
            return result.Value;
        }

        public async Task<OrderTax> GetTaxForOrder(Order order)
        {
            var result = await restClient.PostAsync<Order, OrderTax>(order, Constants.GetTaxForOrderUrl);
            return result.Value;
        }
    }
}
