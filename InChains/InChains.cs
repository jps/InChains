using System;

namespace InChains
{
    public static class InChains
    {
        public static IResponse<TValue, TError> ContineIfNotError<TValue, TError>(
            this IResponse<TValue, TError> response,
            Func<IResponse<TValue, TError>, IResponse<TValue, TError>> action)
            where TError : new()
        {
            switch (response)
            {
                case ErrorResponse<TError> errorResponse:
                {
                    return response;
                }
                case OkResponse<TValue> okResponse:
                {
                    return action.Invoke(response);
                }
                default:
                {
                    return new ErrorResponse<TValue, TError>(new TError());
                    //throw new NotImplementedException("Response type not known");
                }
            }
        }

        //public static Response<TOut> ContinueIfNotErrorAndTransformType<TIn, TOut>(this Response<TIn> response, Func<Response<TIn>, Response<TOut>> action)
        //    where TOut : new()
        //{
        //    switch (response)
        //    {
        //        case ErrorResponse<TIn> errorResponse:
        //        {
        //            return new ErrorResponse<TOut>(errorResponse.Errors);
        //        }
        //        case OkResponse<TIn> okResponse:
        //        {
        //            return action.Invoke(okResponse);
        //        }
        //        default:
        //        {
        //            throw new NotImplementedException("Response type not known");
        //        }
        //    }
        //}

        //public static Response<T> OnErrorType<T, TErrorType>(this Response<T> response, Func<Response<T>, Response<T>> onErrorAction)
        //{
        //    switch (response)
        //    {
        //        case ErrorResponse<T> errorResponse:
        //        {
        //            if (errorResponse.Error != null && errorResponse.Error.GetType() == typeof(TErrorType))
        //            {
        //                return onErrorAction.Invoke(response);
        //            }
        //            return response;
        //        }
        //        case OkResponse<T> okResponse:
        //        {
        //            return okResponse;
        //        }
        //        default:
        //        {
        //            throw new NotImplementedException("Response type not known");
        //        }
        //    }
        //}
    }

    public interface IResponse<out TValue, out TError>
    {
        TValue Value { get; }
        TError Error { get; }
    };

    public interface IErrorResponse<out TValue, out TError> : IResponse<TValue, TError>
    {
        //TValue Value { get; }
        //TError Error { get; }
    };

    public class Response<TValue, TError> : IResponse<TValue, TError>
    {
        public Response(TValue value)
        {
            Value = value;
        }

        public Response(TError error)
        {
            Error = error;
        }

        public TValue Value { get; }
        public TError Error { get; }
    }

    public abstract class Response<TValue> : Response<TValue, Error>
    {
        protected Response(TValue error)
            : base(error)
        {
        }

        protected Response(Error error)
            : base(error)
        {
        }
    }

    public class ErrorResponse<TValue, TError> : Response<TValue, TError>
    {
        public ErrorResponse(TError error)
            : base(error)
        {
        }
    }

    public class ErrorResponse<TError> : Response<object, TError>
    {
        public ErrorResponse(TError error)
            : base(error)
        {
        }
    }


    public class ErrorResponse : Response<Error>
    {
        public ErrorResponse(Error error)
            : base(error)
        {
        }
    }

    public class OkResponse<TValue> : Response<TValue, object>
    {
        public OkResponse(TValue value)
            : base(value)
        {
        }
    }

    public class Error
    {
        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
