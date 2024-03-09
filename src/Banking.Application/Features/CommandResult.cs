namespace Banking.Application.Features;

public abstract record CommandResult;

public record Success : CommandResult;

public record Failure : CommandResult
{
    public required string Message { get; init; }
    public string[] Errors { get; init; } = Array.Empty<string>();

    public void Deconstruct(out string message, out string[] errors)
    {
        message = Message;
        errors = Errors;
    }
}