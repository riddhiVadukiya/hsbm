using System;
using System.Web;

namespace HSBM.Service.ServiceContext
{
    public class RequestResponseServiceContext
    {
        private readonly Guid _targetID = Guid.Empty;


        public RequestResponseServiceContext()
        {
            Request = new RequestHeader();
            Response = new ResponseHeader();
        }

        public RequestResponseServiceContext(Guid p_TargetID)
        {
            Request = new RequestHeader();
            Response = new ResponseHeader();
            _targetID = p_TargetID;
        }

        public RequestHeader Request { get; private set; }
        public ResponseHeader Response { get; private set; }
        public HttpContext Context { get; set; }

        public Guid TargetID
        {
            get { return _targetID; }
        }
    }

    public class RequestHeader
    {
        public int StatusCode { get; set; }

        public String[] StatusParameters { get; set; }
    }

    public class ResponseHeader
    {
        public int StatusCode { get; set; }

        public String[] StatusParameters { get; set; }

        public String StatusMessage
        {
            get
            {
                if (StatusParameters != null && StatusParameters.Length > 0)
                {
                    return StatusParameters[0];
                }
                return "";
            }
            set { }
        }
    }

    public class StandardStatusCodes
    {
        public static readonly int SUCCESS = 0;
        public static readonly int BAD_REQUEST = 400;
        public static readonly int NOT_AUTHORIZED = 401;
        public static readonly int NOT_AUTHENTICATED = 402;
        public static readonly int NOT_FOUND = 404;
        public static readonly int INTERNAL_SERVER_ERROR = 500;
        public static readonly int NOT_IMPLEMENTED = 501;
        public static readonly int SERVICE_NOT_AVAILABLE = 503;
        public static readonly int POLICY_NOT_FULFILLED = 420;
        public static readonly int CURRENT_PASSWORD_INCORRECT = 421;
        public static readonly int IS_EXIST = 421;
    }
}
