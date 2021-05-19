namespace Worker.Utils.Result
{
    public class SuccessResult : IResult
    {
        public SuccessResult()
        {
            HasSucceeded = true;
        }
        public bool HasSucceeded { get; private set; }
    }

    public class SuccessResult<T> : IResult<T>
    {
        public SuccessResult() => HasSucceeded = true;
        public SuccessResult(T value) : this() => Value = value;
        public T Value { get; }

        public bool HasSucceeded { get; }
    }
}
