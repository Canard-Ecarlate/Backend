namespace DuckCity.Api.DTO.Authentication
{
    public class TokenAndCurrentContainerIdDto
    {
        public string Token { get; set; }

        public string? CurrentContainerId { get; set; }

        public TokenAndCurrentContainerIdDto(string token, string? currentContainerId)
        {
            Token = token;
            CurrentContainerId = currentContainerId;
        }
    }
}
