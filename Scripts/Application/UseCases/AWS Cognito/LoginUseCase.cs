using System.Threading.Tasks;

public class LoginUseCase
{
    private readonly IAuthRepository _authRepo;

    public LoginUseCase(IAuthRepository authRepo)
    {
        _authRepo = authRepo;
    }

    public async Task<AuthResult> Execute(string email, string password)
    {
        return await _authRepo.LoginAsync(email, password);
    }
}