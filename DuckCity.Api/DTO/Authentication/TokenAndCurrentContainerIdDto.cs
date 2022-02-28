namespace DuckCity.Api.DTO.Authentication
{
    public class TokenAndCurrentContainerIdDto
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public string Token { get; set; }

        public string? ContainerId { get; set; }

        public TokenAndCurrentContainerIdDto(string id, string name, string token, string? containerId)
        {
            Token = token;
            ContainerId = containerId;
            Id = id;
            Name = name;
        }
    }
}
