namespace CanardEcarlate.Api.Models.Authentication
{
    public class UserWithToken
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
