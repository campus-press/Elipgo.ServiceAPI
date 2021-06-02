using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Examen.Elipgo.DAO.Response
{
    public class StatusResponse
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class StatusResponse<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Value { get; set; }
    }
}
