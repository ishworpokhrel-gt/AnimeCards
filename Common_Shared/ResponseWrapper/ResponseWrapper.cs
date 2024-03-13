namespace Common_Shared.ResponseWrapper
{

    public class SuccessResponseWrapper<T>
    {
        public const string _Version = "1.1";
        public SuccessResponseWrapper(T data)
        {
            Data = data;
        }
        public Dictionary<string, object> Meta { get; set; } = new Dictionary<string, object>
        {
            {
                "api",
                new
                {
                    apiVersion = _Version

                }
            }


        };
        public T Data { get; set; }


        public static SuccessResponseWrapper<T> SuccessApi(T data)
        {
            return new SuccessResponseWrapper<T>(data);
        }

    }

    public class ErrorResponseWrapper
    {
        public const string _Version = "1.1";
        public Dictionary<string, object> Meta { get; set; } = new Dictionary<string, object>
        {
            {
                "api",
                new
                {
                     apiVersion = _Version
                }
            }
        };

        public List<ErrorDetails> Errors { get; set; }
        public ErrorResponseWrapper(string message , int code)
        {
            Errors = new List<ErrorDetails>()
            {
                new ErrorDetails
                {
                    Code = code,
                    Message = message

                }
            };
        }

        public static ErrorResponseWrapper ErrorApi(string message , int code = 400)
        {
            return new ErrorResponseWrapper(message,code);
        }
    
    }





    public class ErrorDetails
    {
        public string Title { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
