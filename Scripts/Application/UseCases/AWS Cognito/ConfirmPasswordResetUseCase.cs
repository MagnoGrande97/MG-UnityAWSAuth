using System.Threading.Tasks;

public class ConfirmPasswordResetUseCase
{
    private readonly IAuthRepository _authRepo;

    public ConfirmPasswordResetUseCase(IAuthRepository authRepo)
    {
        _authRepo = authRepo;
    }

    public async Task<bool> Execute(string email, string code, string newPassword)
    {
        return await _authRepo.ConfirmPasswordResetAsync(email, code, newPassword);
    }
}