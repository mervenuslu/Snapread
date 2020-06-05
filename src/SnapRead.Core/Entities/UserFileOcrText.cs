using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Core.Entities
{
    public class UserFileOcrText:BaseEntity
    {
        public int UserFileId { get; private set; }
        public UserFile UserFile { get; private set; }
        public string OcrText { get; private set; }
        public UserFileOcrText(int userFileId,string ocrtext)
        {
            UserFileId = userFileId;
            OcrText = ocrtext;
        }
        public void UpdateText(string ocrtext)
        {
            OcrText = ocrtext;
        }
    }
}
