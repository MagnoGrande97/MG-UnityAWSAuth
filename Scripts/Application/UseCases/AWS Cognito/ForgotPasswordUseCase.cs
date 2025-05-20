using System.Threading.Tasks;

public class ForgotPasswordUseCase
{
    private readonly IAuthRepository _authRepo;

    public ForgotPasswordUseCase(IAuthRepository authRepo)
    {
        _authRepo = authRepo;
    }

    public async Task<bool> Execute(string email)
    {
        return await _authRepo.SendPasswordResetAsync(email);
    }
}