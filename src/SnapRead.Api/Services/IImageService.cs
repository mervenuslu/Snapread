using Microsoft.AspNetCore.Http;
using SnapRead.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnapRead.Api.Services
{
    public interface IImageService
    {
        public BaseResponseModel SaveImageLocalDirectory(IFormFile file,string filename);

        public Task<string> GetOcrTextFromFile(IFormFile file);
    }
}
