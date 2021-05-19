using System;
using System.Collections.Generic;

namespace Worker.Utils.Result
{
    public class ExceptionResult : IResult
    {
        private readonly IEnumerable<ExceptionDetail> failureDetails;

        public ExceptionResult() => HasSucceeded = false;

        public ExceptionResult(IEnumerable<ExceptionDetail> failureDetails) : this()
        {
            this.failureDetails = failureDetails;
        }

        public ExceptionResult(string message, Type exceptionType)
        {
            this.failureDetails = new[] { new ExceptionDetail(message, exceptionType) };
        }

        public bool HasSucceeded { get; }

        public IEnumerable<ExceptionDetail> FailureDetails => failureDetails;
    }

    public class ExceptionResult<T> : ExceptionResult, IResult<T>
    {
        public ExceptionResult(IEnumerable<ExceptionDetail> failureDetails) : base(failureDetails)
        {
        }

        public ExceptionResult(string message, Type exceptionType) : base(message, exceptionType) { }

        public T Value { get; }
    }
}
