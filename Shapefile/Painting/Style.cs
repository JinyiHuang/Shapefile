using System.Drawing;

namespace Painting
{
    public class Style
    {
        public Color BorderColor { get; set; }
        public float BorderWidth { get; set; }
        public Color FillColor { get; set; }

        public Style(Color borderColor,float borderWidth,Color fillColor)
        {
            this.BorderColor = borderColor;
            this.BorderWidth = borderWidth;
            this.FillColor = fillColor;
        }

        public Style(Color borderColor,float borderWidth)
            :this(borderColor,borderWidth,Color.FromArgb(100,Color.White))
        {

        }

        public Style(Color borderColor,Color fillColor)
            :this(borderColor,1,fillColor)
        {

        }

        public Style(Color borderColor)
            : this(borderColor, 1, Color.FromArgb(100, Color.White))
        {

        }

        public Style()
            : this(Color.Black, 1, Color.FromArgb(100, Color.White))
        {

        }
    }
}
