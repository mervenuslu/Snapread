using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnapRead.Api.Model
{
    public class BaseResponseModel
    {
        public BaseResponseModel()
        {
            Errors = new List<string>();
        }
        public bool IsSuccess { get; set; }
       
        public List<string> Errors { get; set; }
        public string Message { get; set; }
    }
}
