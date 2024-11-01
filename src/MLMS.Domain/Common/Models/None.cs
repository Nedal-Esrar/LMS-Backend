namespace MLMS.Domain.Entities;

public readonly struct None
{
    private static readonly None _value = new();

    public static ref readonly None Value => ref _value;
};