namespace Worker.Utils.Result
{
    public class FailureDetail
    {
        public FailureDetail(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }
}
