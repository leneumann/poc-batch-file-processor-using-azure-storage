using System.Collections.Generic;

namespace Worker.Utils.Result
{
    public class FailureResult : IResult
    {
        private readonly IEnumerable<FailureDetail> failureDetails;

        public FailureResult() => HasSucceeded = false;

        public FailureResult(IEnumerable<FailureDetail> failureDetails) : this()
        {
            this.failureDetails = failureDetails;
        }

        public FailureResult(string message)
        {
            this.failureDetails = new[] { new FailureDetail(message) };
        }

        public bool HasSucceeded { get; }

        public IEnumerable<FailureDetail> FailureDetails => failureDetails;
    }

    public class FailureResult<T> : FailureResult, IResult<T>
    {
        public FailureResult(IEnumerable<FailureDetail> failureDetails) : base(failureDetails)
        {
        }

        public FailureResult(string message) : base(message) { }

        public T Value { get; }
    }
}
