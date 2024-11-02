using System.Text.RegularExpressions;

namespace MLMS.Application.Common;

public static class Utilities
{
    public static bool IsStrongPassword(string password)
    {
        return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
    }
}