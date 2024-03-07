namespace Models.Response_Wrapper
{

    public class SuccessResponseWrapper
    {
        public const string _Version = "1.1";
        public SuccessResponseWrapper(object data)
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
        public object Data { get; set; }


        public static SuccessResponseWrapper SuccessApi(object data)
        {
            return new SuccessResponseWrapper(data);
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
        public ErrorResponseWrapper(string message)
        {
            Errors = new List<ErrorDetails>()
            {
                new ErrorDetails
                {
                    Code = 400,
                    Message = message

                }
            };
        }

        public static ErrorResponseWrapper ErrorApi(string message)
        {
            return new ErrorResponseWrapper(message);
        }
    }





    public class ErrorDetails
    {
        public string Title { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
