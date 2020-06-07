using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Core.Entities
{
    public class ImageOcrText:BaseEntity
    {
        public int ImageId { get; private set; }
        public string OcrText { get; private set; }
        public Image Image { get; private set; }
       



        public ImageOcrText(int imageId,string ocrtext)
        {
            ImageId = imageId;
            OcrText = ocrtext;
        }
        public void UpdateText(string ocrtext)
        {
            OcrText = ocrtext;
        }
    }
}
