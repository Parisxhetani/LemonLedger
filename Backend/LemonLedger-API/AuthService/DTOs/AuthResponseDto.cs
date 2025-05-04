namespace AuthService.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
