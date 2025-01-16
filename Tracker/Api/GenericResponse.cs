using System.Collections.Generic;

namespace Tracker.Api
{
    public class GenericResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public List<string> ValidationErrors { get; set; }
        public string Message { get; set; }
    }
}
