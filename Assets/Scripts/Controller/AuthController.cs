using System;
using UnityEngine;

public class AuthController : MonoBehaviour
{
    public static AuthController Instance { get; private set; }

    private IAuthRepository _authRepo;
    private LoginUseCase _loginUseCase;
    private RegisterUseCase _registerUseCase;
    private ForgotPasswordUseCase _forgotPasswordUseCase;
    private ConfirmPasswordResetUseCase _confirmPasswordResetUseCase;
    private LogoutUseCase _logoutUseCase;

    public Action<string> OnUserUnconfirmed;
    public Action<string> OnUserNeedsConfirmation;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _authRepo = AuthProviderFactory.CreateProvider();
        IUserRepository userRepo = new DynamoUserRepository();

        _loginUseCase = new LoginUseCase(_authRepo);
        _registerUseCase = new RegisterUseCase(_authRepo, userRepo);
        _forgotPasswordUseCase = new ForgotPasswordUseCase(_authRepo);
        _confirmPasswordResetUseCase = new ConfirmPasswordResetUseCase(_authRepo);
        _logoutUseCase = new LogoutUseCase(_authRepo);
    }

    public async void Login(string email, string password)
    {
        var result = await _loginUseCase.Execute(email, password);
        Debug.Log(result.Message);

        if (result.Success)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dashboard");
        }
        else
        {
            if (result.Message.Contains("User not confirmed"))
            {
                OnUserUnconfirmed?.Invoke(email);
            }

            Debug.LogWarning("Login failed: " + result.Message);
        }
    }

    public async void ConfirmSignUp(string email, string code)
    {
        if (_authRepo is CognitoAuthService cognito)
        {
            var result = await cognito.ConfirmSignUpAsync(email, code);
            Debug.Log(result.Message);
        }
        else
        {
            Debug.LogWarning("Code confirmation not applicable to this supplier.");
        }
    }

    public async void Register(string email, string password)
    {
        var result = await _registerUseCase.Execute(email, password);
        Debug.Log(result.Message);

        if (result.Success)
        {
            OnUserNeedsConfirmation?.Invoke(email);
        }
        else
        {
            Debug.LogWarning("Failed registration: " + result.Message);
        }
    }

    public async void ForgotPassword(string email)
    {
        var success = await _forgotPasswordUseCase.Execute(email);
        Debug.Log("Reset sent: " + success);
    }

    public async void ConfirmPasswordReset(string email, string code, string newPassword)
    {
        var success = await _confirmPasswordResetUseCase.Execute(email, code, newPassword);
        if (success)
        {
            Debug.Log("Password successfully reset.");
        }
        else
        {
            Debug.LogError("Error resetting password.");
        }
    }

    public async void Logout()
    {
        bool result = await _logoutUseCase.Execute();
        if (result)
        {
            Debug.Log("Session closed correctly.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("SignIn_SignUp");
        }
        else
        {
            Debug.LogWarning("There was an error logging out.");
        }
    }
}