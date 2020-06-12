using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SnapRead.Api.Model;
using SnapRead.Core.Entities;
using SnapRead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tesseract;

namespace SnapRead.Api.Services
{
    public class ImageService : IImageService
    {
        private IWebHostEnvironment _env;
        public const string folderName = @"images\";
        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }


        public BaseResponseModel SaveImageLocalDirectory(IFormFile file,string filename)
        {
            BaseResponseModel baseResponseModel = new BaseResponseModel();
            string path = Path.Combine(_env.WebRootPath, folderName + filename);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                try
                {
                    file.CopyTo(fileStream);
                    baseResponseModel.IsSuccess = true;
                    baseResponseModel.Message = "File copy successfully";
                    return baseResponseModel;
                }
               
                catch(IOException ioex)
                {
                    baseResponseModel.IsSuccess = false;
                    baseResponseModel.Message = ioex.Message.ToString();
                    return baseResponseModel;
                }
            }
        }
        public async Task<string> GetOcrTextFromFile(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var img = System.Drawing.Image.FromStream(memoryStream))
                {
                    string tesspath = Path.Combine(_env.WebRootPath, "tessdata");
                    using (var engine = new TesseractEngine(tesspath, "tur", EngineMode.Default))
                    {
                        using (var image = new System.Drawing.Bitmap(img))
                        {
                            using (var pix = PixConverter.ToPix(image))
                            {
                                using (var page = engine.Process(pix))
                                {
                                    return page.GetText();
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
