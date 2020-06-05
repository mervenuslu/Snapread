using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Core.Entities
{
    public class UserFile:BaseEntity
    {
        public string UserId { get; private set; }
        public string FilePath { get; private set; }
        public UserFileOcrText UserFileOcrText { get; set; }
        public UserFile(string userId, string filePath)
        {
            UserId = userId;
            FilePath = filePath;
        }
    }
}
