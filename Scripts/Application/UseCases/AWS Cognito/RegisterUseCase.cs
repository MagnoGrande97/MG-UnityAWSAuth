using System;
using System.Threading.Tasks;

public class RegisterUseCase
{
    private readonly IAuthRepository _authRepo;
    private readonly IUserRepository _userRepo;

    public RegisterUseCase(IAuthRepository authRepo, IUserRepository userRepo)
    {
        _authRepo = authRepo;
        _userRepo = userRepo;
    }

    public async Task<AuthResult> Execute(string email, string password)
    {
        var result = await _authRepo.RegisterAsync(email, password);

        if (result.Success)
        {
            await _userRepo.SaveUserAsync(new UserData { userID = Guid.NewGuid().ToString(), email = email });
        }

        return result;
    }
}