using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    /// <summary>
    /// Clase hecha para manejar con estructura personalizada las respuestas que se dan a los endpoint, aún faltan controladores para implementarles la clase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServicesResult<T>
    {
        public T Result { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

        public static ServicesResult<T> SuccessfulOperation(T result)
        {
            return new ServicesResult<T>
            {
                Success = true,
                Result = result
            };
        }

        public static ServicesResult<T> FailedOperation(int statusCode, string message, Exception exception = null/*, List<T> result = null*/)
        {
            return new ServicesResult<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Exception = exception?.ToString()
            };
        }
    }
}
