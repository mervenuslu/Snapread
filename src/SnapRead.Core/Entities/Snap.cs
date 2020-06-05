using System;
using System.Collections.Generic;
using System.Text;

namespace SnapRead.Core.Entities
{
    public class Snap:BaseEntity
    {
        public string ImageName { get; private set; }
        public string Text { get; private set; }
        public Snap(string imageName,string text)
        {
            ImageName = imageName;
            Text = text;
        }
    }
}
