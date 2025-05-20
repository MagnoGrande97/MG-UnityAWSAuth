
# Modular Authentication System with AWS Cognito for Unity

This project implements a complete authentication system in Unity using AWS Cognito as a backend. It is designed with a clean, decoupled architecture for ease of maintenance and extensibility.

## Features

- User registration with mail verification
- Login with handling of unconfirmed users
- Password reset by code
- Logout
- User data storage in Amazon DynamoDB
- Architecture based on interfaces and use cases
- Easily interchangeable with other authentication services

---

## Project Structure

```
Assets/
├── Bean/
│   ├── AuthResult.cs
│   └── UserData.cs
│
├── Controller/
│   └── AuthController.cs
│
├── Repository/
│   ├── Interfaces/
│   │   ├── IAuthRepository.cs
│   │   └── IUserRepository.cs
│   ├── CognitoAuthService.cs
│   └── DynamoUserRepository.cs
│
├── UI/
│   ├── LoginPanel.cs
│   ├── RegisterPanel.cs
│   ├── ForgotPasswordPanel.cs
│   └── DashboardPanel.cs
│
├── UseCases/
│   ├── LoginUseCase.cs
│   ├── RegisterUseCase.cs
│   ├── ForgotPasswordUseCase.cs
│   ├── ConfirmPasswordResetUseCase.cs
│   └── LogoutUseCase.cs
│
└── AuthProviderFactory.cs
```

---

## General Flow

1. **Home**.
   - The user opens the app and sees the login or registration screens.
2. **SignUp**
   - The user is created in Cognito and a confirmation code is requested.
   - When the code is confirmed, the user is stored in DynamoDB.
3. **SignIn**
   - If the user is confirmed, the user is signed in.
   - If not, email confirmation is requested.
4. **Forgot my password**
   - A recovery code is sent to the email.
   - By entering the code and the new password, access is restored.
5. **Logout**
   - The user can logout from the Dashboard.

---

## Dependencies

- Unity 2021 or higher
- Amazon Cognito SDK for .NET
  - `Amazon.Extensions.CognitoAuthentication`
  - `AWSSDK.CognitoIdentityProvider`
  - `AWSSDK.DynamoDBv2`
  - `AWSSDK.Core`
- TMPro

---

## Considerations

- The user data is stored in DynamoDB after a successful confirmation.
- You can easily change the authentication provider by editing `AuthProviderFactory`.
- Use `DontDestroyOnLoad` to keep the authentication driver between scenes.
- All relevant events (confirmation required, user not confirmed) are handled by `Action<string>` events.

---

## Author

Magno Grande
