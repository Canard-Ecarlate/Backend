namespace DuckCity.Api.DTO.Authentication
{
    public class UserWithTokenDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
