using Painting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapefileLibrary
{
    public class BoundingBox
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }

        public BoundingBox(double xMin, double xMax, double yMin, double yMax)
        {
            this.XMin = xMin;
            this.XMax = xMax;
            this.YMin = yMin;
            this.YMax = yMax;
        }

        //public static bool operator ==(BoundingBox box1,BoundingBox box2)
        //{
        //    if (box1 == null && box2 == null)
        //    {
        //        return true;
        //    }
        //    else if (box1 == null || box2 == null)
        //    {
        //        return false;
        //    }

        //    if (box1.XMin == box2.XMin && box1.XMax == box2.XMax &&
        //        box1.YMin == box2.YMin && box1.YMax == box2.YMax)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public static bool operator !=(BoundingBox box1, BoundingBox box2)
        //{
        //    return !(box1 == box2);
        //}

        public static bool operator <(BoundingBox box1, BoundingBox box2)
        {
            if (box1.XMin > box2.XMin && box1.XMax < box2.XMax &&
                box1.YMin > box2.YMin && box1.YMax < box2.YMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator >(BoundingBox box1, BoundingBox box2)
        {
            if (box1.XMin < box2.XMin && box1.XMax > box2.XMax &&
                box1.YMin < box2.YMin && box1.YMax > box2.YMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator <=(BoundingBox box1, BoundingBox box2)
        {
            if (box1.XMin >= box2.XMin && box1.XMax <= box2.XMax &&
                box1.YMin >= box2.YMin && box1.YMax <= box2.YMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator >=(BoundingBox box1, BoundingBox box2)
        {
            if (box1.XMin <= box2.XMin && box1.XMax >= box2.XMax &&
                box1.YMin <= box2.YMin && box1.YMax >= box2.YMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal protected static BoundingBox GetBoundingBox(Shape shape)
        {
            double xMin = 0,
                xMax = 0,
                yMin = 0,
                yMax = 0;

            foreach (var vertex in shape.Vertexes)
            {
                if (xMin > vertex.X)
                {
                    xMin = vertex.X;
                }
                if (xMax < vertex.X)
                {
                    xMax = vertex.X;
                }
                if (yMin > vertex.Y)
                {
                    yMin = vertex.Y;
                }
                if (yMax < vertex.Y)
                {
                    yMax = vertex.Y;
                }
            }

            return new BoundingBox(xMin, xMax, yMin, yMax);
        }

        internal protected static BoundingBox GetBoundingBox(IEnumerable<Shape> shapes)
        {
            double xMin = 0,
                xMax = 0,
                yMin = 0,
                yMax = 0;

            foreach (var shape in shapes)
            {
                foreach (var vertex in shape.Vertexes)
                {
                    if (xMin > vertex.X)
                    {
                        xMin = vertex.X;
                    }
                    if (xMax < vertex.X)
                    {
                        xMax = vertex.X;
                    }
                    if (yMin > vertex.Y)
                    {
                        yMin = vertex.Y;
                    }
                    if (yMax < vertex.Y)
                    {
                        yMax = vertex.Y;
                    }
                }
            }

            return new BoundingBox(xMin, xMax, yMin, yMax);
        }
    }
}
