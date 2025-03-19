using Data.Data;
namespace TaxPayerWeb.Models
{
    public class TaxPayerFormModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public IFormFile ImageData { get; set; }
    }
}
