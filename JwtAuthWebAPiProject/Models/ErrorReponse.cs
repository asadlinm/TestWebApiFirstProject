using System.Diagnostics.Eventing.Reader;

namespace JwtAuthWebAPiProject.Models
{
    public class ErrorReponse
    {
        public string Message { get; set; }
        public string StatusCode { get; set; }
        public bool Success { get; set; }
    }
}
