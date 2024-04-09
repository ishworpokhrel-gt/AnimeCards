using Models.PaginationModel;

namespace Common_Shared.ResponseResult
{

    public class ResponseResult
    {
        public object Result { get; set; }
        public object Pagination { get; set; }
        public string Message { get; set; } = "Executed Successfully";
        public StatusDetails Status { get; set; }
        public bool IsSuccess => Status == StatusDetails.Success;


        public static ResponseResult Success<T>(T data)
        {
            return new ResponseResult
            {
                Result = data,
                Status = StatusDetails.Success
            };
        }

        public static ResponseResult Success<T>(List<T> data, Pagination pagination)
        {
            return new ResponseResult
            {
                Result = data,
                Status = StatusDetails.Success,
                Pagination = pagination
            };
        }



        public static ResponseResult Failed(object data)
        {
            return new ResponseResult
            {
                Result = data,
                Message = "Operation Failed",
                Status = StatusDetails.failure
            };
        }

        public static ResponseResult Failed(string message)
        {
            return new ResponseResult
            {
                Message = message,
                Status = StatusDetails.failure
            };
        }

    };

    public enum StatusDetails
    {
        Success,
        failure
    };


}


