using ErrorOr;
using FluentValidation.Results;

namespace MLMS.Application.Common;

public static class ErrorExtensions
{
    public static List<Error> ExtractErrors(this ValidationResult result)
    {
        if (result.Errors.Count == 0)
        {
            return [];
        }

        return result.Errors.Select(e => Error.Validation(
                code: e.ErrorCode,
                description: e.ErrorMessage))
            .ToList();
    }
}