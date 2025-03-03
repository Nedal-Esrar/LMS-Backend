using ErrorOr;

namespace MLMS.Domain.Common.Exceptions;

public class UnoccasionalErrorException(List<Error> errors) : Exception
{
    public List<Error> Errors { get; } = errors;
}