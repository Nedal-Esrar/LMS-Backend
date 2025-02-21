using ErrorOr;
using FluentValidation.Results;

namespace MLMS.Domain.Common;

public static class ErrorExtensions
{
    public static List<Error> ExtractErrors(this ValidationResult result)
    {
        // TODO: enhance validation errors.
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