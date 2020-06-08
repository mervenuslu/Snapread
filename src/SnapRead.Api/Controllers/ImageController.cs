using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnapRead.Api.Helper;
using SnapRead.Api.Model;
using Tesseract;
using SnapRead.Core.Interfaces;
using SnapRead.Infrastructure.Data;
using SnapRead.Core.Entities;
using SnapRead.Api.Services;

namespace SnapRead.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImageController : BaseApiController
    {
        private IWebHostEnvironment _env;
        private readonly IRepository<Image> _imageRepository;
        private readonly IImageService _imageService;
        public ImageController(IRepository<Image> imageRepository,IImageService imageService)
        {
            _imageRepository = imageRepository;
            _imageService = imageService;
        }


        [HttpPost]
        public async Task<ApiResponseModel<string>> OcrText(IFormFile file)
        {
            

            ApiResponseModel<string> result = new ApiResponseModel<string>();
            if (file.CheckIfImage())
            {
                string fileName = Path.GetRandomFileName();
                var imageLocalDirectoryResult =_imageService.SaveImageLocalDirectory(file, fileName);
                if (imageLocalDirectoryResult.IsSuccess)
                {
                    Image image = new Image(UserHelper.GetUserId(),fileName);

                    await _imageRepository.CreateAsync(image);
                    result.Data =await _imageService.GetOcrTextFromFile(file);
                }
                
            }
            return result;
        }


       
    }
}
