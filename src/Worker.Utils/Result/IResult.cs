namespace Worker.Utils.Result
{

    public interface IResult
    {
        bool HasSucceeded { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Value { get; }
    }
}
