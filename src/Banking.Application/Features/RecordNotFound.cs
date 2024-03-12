namespace Banking.Application.Features;

public readonly record struct RecordNotFound<TId>(TId Id);