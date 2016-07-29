using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Painting
{
    public class BitmapCanvas:Canvas
    {
        public BitmapCanvas(int width,int height)
            :base(width,height)
        {
            this.image = new Bitmap(width, height);
        }

        public BitmapCanvas(int width,int height,Color backgroundColor)
            :base(width,height,backgroundColor)
        {
            this.image = new Bitmap(width, height);
        }

        public override Image GetImage()
        {
            return this.image;
        }
    }
}
