namespace Waffle.Models;

public class TResult<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public bool Succeeded { get; set; }
    public static TResult<T> Ok(T? data)
    {
        return new TResult<T>
        {
            Succeeded = true,
            Data = data
        };
    }

    public static TResult<T> Failed(string message) => new() { Succeeded = false, Message = message };
}

public class TResult
{
    public string? Message { get; set; }
    public bool Succeeded { get; set; }
    public static TResult Success => new() { Succeeded = true };

    public static TResult Failed(string message) => new() { Succeeded = false, Message = message };
}