using System.Threading.Tasks;
using Notes.Models;

namespace Notes.Services
{
    public interface ITaxService
    {
        Task<OrderTax> GetTaxForOrder(Order order);

        Task<TaxRate> GetTaxForLocation(string zip, string country);
    }
}
