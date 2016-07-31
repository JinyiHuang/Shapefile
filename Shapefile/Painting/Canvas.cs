using System.Drawing;

namespace Painting
{
    public abstract class Canvas
    {
        protected Image image;
        public int Width { get; set; }
        public int Height { get; set; }
        public Color BackgroundColor { get; set; }

        protected Canvas(int width, int height)
            :this(width,height,Color.White)
        {
        }

        protected Canvas(int width, int height, Color backgroundColor)
        {
            this.Width = width;
            this.Height = height;
            this.BackgroundColor = backgroundColor;
        }

        public abstract Image GetImage();

        public void Save(string path)
        {
            this.image.Save(path);
        }
    }
}
