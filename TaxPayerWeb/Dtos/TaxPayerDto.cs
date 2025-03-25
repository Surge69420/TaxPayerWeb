using Data.Data;
namespace TaxPayerWeb.Dtos
{
    public class TaxPayerDto
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string PostalCode { get; set; }
        public required int VAT { get; set; }
        public required IFormFile ImageData { get; set; }
    }
    public class TaxPayerEditDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string PostalCode { get; set; }
        public required int VAT { get; set; }
        public string? CityName { get; set; }
        public required IFormFile ImageData { get; set; }
        public string? dummyImg { get; set; }
    }
    public class LoginDto()
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class RegisterDto()
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class ResetPassDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        public required string Token { get; set; }
    }
    public class ChangePassDto
    {
        public required string NewPassword { get; set; }
        public required string CurrentPassword { get; set; }
    }
}
