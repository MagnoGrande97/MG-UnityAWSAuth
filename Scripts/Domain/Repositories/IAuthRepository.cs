using System.Threading.Tasks;

public interface IAuthRepository
{
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RegisterAsync(string email, string password);
    Task<bool> SendPasswordResetAsync(string email);
    Task<bool> ConfirmPasswordResetAsync(string email, string code, string newPassword);
    Task<bool> LogoutAsync();
}