namespace PizzaApiTesterApp;

public record Tokens(string jwt, string refreshToken);

public class TokenStorage
{
    public const string jwtKey = "jwt";
    public const string refreshTokenKey = "refreshToken";
    public async Task StoreTokens(Tokens t)
    {
        await SecureStorage.SetAsync(jwtKey, t.jwt);
        await SecureStorage.SetAsync(refreshTokenKey, t.refreshToken);
    }

    public async Task<Tokens> GetTokens()
    {
        var jwt = await SecureStorage.GetAsync(jwtKey);
        var refreshToken = await SecureStorage.GetAsync(refreshTokenKey);
        return new Tokens(jwt, refreshToken);
    }

    public void ClearTokens()
    {
        SecureStorage.Remove(jwtKey);
        SecureStorage.Remove(refreshTokenKey);
    }
}
