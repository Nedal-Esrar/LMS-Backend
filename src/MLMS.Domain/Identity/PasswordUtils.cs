using System.Text.RegularExpressions;

namespace MLMS.Domain.Identity;

public static class PasswordUtils
{
    public static bool IsStrongPassword(string password) =>
        Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
}