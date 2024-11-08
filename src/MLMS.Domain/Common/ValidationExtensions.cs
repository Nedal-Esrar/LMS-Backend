using FluentValidation;

namespace MLMS.Domain.Common;

public static class ValidationExtensions
{
    /// <summary>
    ///   Validates that the string property represents a strong password.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    /// <remarks>
    ///   The strong password pattern enforces a combination of uppercase and lowercase letters,
    ///   digits, and special characters, with a minimum length of 8 characters.
    /// </remarks>
    public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilderOptions<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(Utilities.IsStrongPassword)
            .WithMessage("Weak Password");
    }
    
    /// <summary>
    ///   Validates that the string property represents a valid phone number.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    /// <remarks>
    ///   The phone number pattern allows various formats including international codes, area codes, and optional separators.
    /// </remarks>
    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")
            .WithMessage("Not a valid phone number");
    }
}