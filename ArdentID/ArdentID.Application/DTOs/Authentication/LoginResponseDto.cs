namespace ArdentID.Application.DTOs.Authentication
{
    public class LoginResponseDto
    {
        public Guid Id { get; set; }
        public required bool Result { get; set; }
        public required string JwtToken { get; set; }
    }
}
