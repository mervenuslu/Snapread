using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Api.Model
{
    public class ApiResponseModel<T>:BaseResponseModel
    {
        public T Data { get; set; }
    }
}
