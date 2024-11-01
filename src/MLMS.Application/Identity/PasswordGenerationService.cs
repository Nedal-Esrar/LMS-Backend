using System.Text;
using ErrorOr;
using MLMS.Domain.Identity;

namespace MLMS.Application.Identity;

public class PasswordGenerationService : IPasswordGenerationService
{
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string SpecialChars = "!@#$%^&*()_+-=[]{}|;':\",.<>?";
    
    private const string CharsCombined = Lowercase + Uppercase + Digits + SpecialChars;
    
    public ErrorOr<string> GenerateStrongPassword(int length)
    {
        if (length < 8)
        {
            return Error.Validation("Password length must be at least 8 characters.");
        }

        var random = new Random();
        
        var password = new StringBuilder();
        
        password.Append(Lowercase[random.Next(Lowercase.Length)]);
        password.Append(Uppercase[random.Next(Uppercase.Length)]);
        password.Append(Digits[random.Next(Digits.Length)]);
        password.Append(SpecialChars[random.Next(SpecialChars.Length)]);
        
        for (var i = 4; i < length; i++)
        {
            password.Append(CharsCombined[random.Next(CharsCombined.Length)]);
        }

        return password.ToString();
    }
}