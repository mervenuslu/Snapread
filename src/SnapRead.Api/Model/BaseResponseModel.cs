using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnapRead.Api.Model
{
    public class BaseResponseModel
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
