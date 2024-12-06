using System.Text.RegularExpressions;

namespace MLMS.Domain.Common;

public static class Utilities
{
    public static bool IsStrongPassword(string password) =>
        Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
}