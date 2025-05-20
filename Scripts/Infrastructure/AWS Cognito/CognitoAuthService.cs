using System;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using UnityEngine;

public class CognitoAuthService : IAuthRepository
{
    private readonly string clientId = "YOUR_CLIENT_ID";
    private readonly string poolId = "YOUR_POOL_ID";
    private readonly RegionEndpoint region = RegionEndpoint.USEast1;

    private AmazonCognitoIdentityProviderClient _provider;
    private CognitoUserPool _userPool;

    private CognitoUser _user;

    public CognitoAuthService()
    {
        _provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), region);
        _userPool = new CognitoUserPool(poolId, clientId, _provider);
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        try
        {
            _user = new CognitoUser(email, clientId, _userPool, _provider);
            var authRequest = new InitiateSrpAuthRequest
            {
                Password = password
            };

            var authResponse = await _user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

            _user.SessionTokens = new CognitoUserSession(
                authResponse.AuthenticationResult.IdToken,
                authResponse.AuthenticationResult.AccessToken,
                authResponse.AuthenticationResult.RefreshToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddSeconds(authResponse.AuthenticationResult.ExpiresIn)
            );


            return new AuthResult
            {
                Success = true,
                Message = "Successful login"
            };
        }
        catch (UserNotConfirmedException)
        {
            await ResendConfirmationCodeAsync(email);
            return new AuthResult
            {
                Success = false,
                Message = "User not confirmed. Code forwarded to email."
            };
        }
        catch (Exception ex)
        {
            Debug.LogError($"Login Error: {ex.Message}");
            return new AuthResult
            {
                Success = false,
                Message = "Login failed"
            };
        }
    }

    public async Task<AuthResult> RegisterAsync(string email, string password)
    {
        var signUpRequest = new SignUpRequest
        {
            ClientId = clientId,
            Username = email,
            Password = password
        };

        signUpRequest.UserAttributes.Add(new AttributeType { Name = "email", Value = email });

        try
        {
            var response = await _provider.SignUpAsync(signUpRequest).ConfigureAwait(false);
            return new AuthResult { Success = true, Message = "Successful registration. Verify your email." };
        }
        catch (Exception ex)
        {
            Debug.LogError("Register Error: " + ex.Message);
            return new AuthResult { Success = false, Message = ex.Message };
        }
    }

    public async Task<AuthResult> ConfirmSignUpAsync(string email, string confirmationCode)
    {
        var request = new ConfirmSignUpRequest
        {
            ClientId = clientId,
            Username = email,
            ConfirmationCode = confirmationCode
        };

        try
        {
            var response = await _provider.ConfirmSignUpAsync(request).ConfigureAwait(false);
            return new AuthResult { Success = true, Message = "User successfully confirmed." };
        }
        catch (Exception ex)
        {
            Debug.LogError("ConfirmSignUp Error: " + ex.Message);
            return new AuthResult { Success = false, Message = ex.Message };
        }
    }

    public async Task<bool> SendPasswordResetAsync(string email)
    {
        var request = new ForgotPasswordRequest
        {
            ClientId = clientId,
            Username = email
        };

        try
        {
            var response = await _provider.ForgotPasswordAsync(request).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Forgot Password Error: " + ex.Message);
            return false;
        }
    }

    public async Task<AuthResult> ResendConfirmationCodeAsync(string email)
    {
        try
        {
            var request = new ResendConfirmationCodeRequest
            {
                ClientId = clientId,
                Username = email
            };

            var response = await _provider.ResendConfirmationCodeAsync(request);

            return new AuthResult
            {
                Success = true,
                Message = "Code forwarded to email."
            };
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error resending code: {ex.Message}");
            return new AuthResult
            {
                Success = false,
                Message = "Error resending code"
            };
        }
    }

    public async Task<bool> ConfirmPasswordResetAsync(string email, string code, string newPassword)
    {
        var request = new ConfirmForgotPasswordRequest
        {
            ClientId = clientId,
            Username = email,
            ConfirmationCode = code,
            Password = newPassword
        };

        var response = await _provider.ConfirmForgotPasswordAsync(request);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> LogoutAsync()
    {
        if (_user != null && _user.SessionTokens != null && !string.IsNullOrEmpty(_user.SessionTokens.AccessToken))
        {
            try
            {
                await _user.GlobalSignOutAsync();
                _user = null;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Error logging out: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        Debug.LogWarning("No user or session token for logging out.");
        return false;
    }
}