namespace MLMS.Application.Common;

public static class Utilities
{
    public static bool IsStrongPassword(string password)
    {
        return password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsLower) &&
               password.Any(char.IsDigit) && password.Any(char.IsSymbol);
    }
}