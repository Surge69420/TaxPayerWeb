using Data.Data;
namespace TaxPayerWeb.Dtos
{
    public class TaxPayerDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public IFormFile ImageData { get; set; }
    }
    public class LoginDto()
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterDto()
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
