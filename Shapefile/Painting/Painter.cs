using System.Collections.Generic;
using System.Drawing;

namespace Painting
{
    public class Painter
    {
        private Canvas canvas;
        private Style style;
        private Pen pen;
        private Brush brush;

        public Painter(Canvas canvas,Style style)
        {
            this.SetCanvas(canvas);
            this.SetStyle(style);
        }

        public Painter(Canvas canvas)
            :this(canvas,new Style())
        {
            
        }

        public void SetStyle(Style style)
        {
            this.style = style;
            this.pen = new Pen(style.BorderColor, style.BorderWidth);
            this.brush = new SolidBrush(style.FillColor);
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Draw(Shape shape)
        {
            Graphics gp = Graphics.FromImage(canvas.GetImage());
            gp.Clear(canvas.BackgroundColor);

            switch (shape.Type)
            {
                case ShapeType.NullType:
                    break;
                case ShapeType.Point:
                    DrawPoint(shape, gp);
                    break;
                case ShapeType.PolyLine:
                    DrawPolyLine(shape, gp);
                    break;
                case ShapeType.Polygon:
                    DrawPolygon(shape, gp);
                    break;
                default:
                    break;
            }
            
            gp.Dispose();
        }

        public void Draw(IEnumerable<Shape> shapes)
        {
            Graphics gp = Graphics.FromImage(canvas.GetImage());
            gp.Clear(Color.White);

            foreach (var shape in shapes)
            {
                switch (shape.Type)
                {
                    case ShapeType.NullType:
                        break;
                    case ShapeType.Point:
                        DrawPoint(shape, gp);
                        break;
                    case ShapeType.PolyLine:
                        DrawPolyLine(shape, gp);
                        break;
                    case ShapeType.Polygon:
                        DrawPolygon(shape,gp);
                        break;
                    default:
                        break;
                }
            }
            gp.Dispose();
        }

        private void DrawPolygon(Shape shape, Graphics gp)
        {
            List<PointF> pfs = new List<PointF>();

            foreach (var vertex in shape.Vertexes)
            {
                pfs.Add(new PointF((float)vertex.X, (float)vertex.Y));
            }

            gp.DrawPolygon(pen, pfs.ToArray());
            gp.FillPolygon(brush, pfs.ToArray());
        }

        protected void DrawPolyLine(Shape shape, Graphics gp)
        {
            List<PointF> pfs = new List<PointF>();

            foreach (var vertex in shape.Vertexes)
            {
                pfs.Add(new PointF((float)vertex.X, (float)vertex.Y));
            }

            gp.DrawLines(pen, pfs.ToArray());
        }

        protected void DrawPoint(Shape shape, Graphics gp)
        {
            float x = (float)shape.Vertexes[0].X,
                y = (float)shape.Vertexes[0].Y,
                width = 5,
                height = 5;
            gp.FillEllipse(brush,x,y,width,height);
        }
    }
}
