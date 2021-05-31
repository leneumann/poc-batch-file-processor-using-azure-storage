using System;

namespace Worker.Utils.Result
{
    public class ExceptionDetail
    {
        public string Message { get; }
        public Type ExceptionType { get; }

        public ExceptionDetail(string message, Type exceptionType)
        {
            Message = message;
            ExceptionType = exceptionType;
        }
    }
}
