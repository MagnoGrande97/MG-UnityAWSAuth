using System.Threading.Tasks;

public class LogoutUseCase
{
    private readonly IAuthRepository _authRepository;

    public LogoutUseCase(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<bool> Execute()
    {
        return await _authRepository.LogoutAsync();
    }
}