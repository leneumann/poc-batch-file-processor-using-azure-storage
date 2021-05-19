using System.Collections.Generic;

namespace Worker.Utils.Result
{
    public abstract class Result : IResult
    {
        public bool HasSucceeded { get; protected set; }
    }




}
