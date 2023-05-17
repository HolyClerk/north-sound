using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthSound.Domain.POCO;

public class Response<T>
{
    public T Data { get; set; }
    public string Details { get; set; }
    public ResponseStatus Status { get; set; }

    public static Response<T> Success(T data)
    {
        return new Response<T>
        {
            Status = ResponseStatus.Success,
            Data = data,
        };
    }

    public static Response<T> Failed(string deatils)
    {
        return new Response<T>
        {
            Status = ResponseStatus.Failed,
            Details = deatils,
        };
    }
}

public enum ResponseStatus
{
    Success,
    Failed
}
