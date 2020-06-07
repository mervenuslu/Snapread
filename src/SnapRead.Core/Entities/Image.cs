using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Core.Entities
{
    public class Image:BaseEntity
    {
        public string UserId { get; private set; }
        public string FilePath { get; private set; }
        public ImageOcrText OcrText { get; set; }
        private Image()
        {
        }
        public Image(string userId, string filePath)
        {
            UserId = userId;
            FilePath = filePath;
        }
    }
}
