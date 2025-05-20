public static class AuthProviderFactory
{
    public enum ProviderType { Firebase, Cognito }

    public static ProviderType CurrentProvider = ProviderType.Cognito;

    public static IAuthRepository CreateProvider()
    {
        switch (CurrentProvider)
        {
            case ProviderType.Cognito:
                return new CognitoAuthService();
            case ProviderType.Firebase:
            default:
                return new CognitoAuthService();
        }
    }
}