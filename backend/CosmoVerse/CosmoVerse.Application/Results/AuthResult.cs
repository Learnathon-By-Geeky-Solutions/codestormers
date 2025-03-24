using CosmoVerse.Models.Dto;

namespace CosmoVerse.Services.Results
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public TokenResponseDto? Token { get; set; }
        public string? ErrorMessage { get; set; }

        public static AuthResult SuccessResult(TokenResponseDto token) => new AuthResult { Success = true, Token = token };
        public static AuthResult Failure(string errorMessage) => new AuthResult { Success = false, ErrorMessage = errorMessage };
    }
}
