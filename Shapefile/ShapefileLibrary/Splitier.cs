using Painting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShapefileLibrary
{
    internal class Spliter
    {
        internal static List<Shape> Split(IEnumerable<Shape> shapes, BoundingBox box, bool isInside)
        {
            if (isInside)
            {
                return GetInsideShape(shapes, box);
            }
            else
            {
                return GetOutsideShape(shapes, box);
            }
        }

        protected static List<Shape> GetInsideShape(IEnumerable<Shape> shapes, BoundingBox box)
        {
            List<Shape> returnShapes = new List<Shape>();
            Vertex vertex;

            foreach (var shape in shapes)
            {
                for (int i = 0; i < shape.Vertexes.Count; i++)
                {
                    vertex = shape.Vertexes[i];
                    if (vertex.X >= box.XMin && vertex.X <= box.XMax &&
                        vertex.Y >= box.YMin && vertex.Y <= box.YMax)
                    {
                        returnShapes.Add(shape);
                        break;
                    }
                }
            }

            return returnShapes;
        }

        protected static List<Shape> GetOutsideShape(IEnumerable<Shape> shapes, BoundingBox box)
        {
            List<Shape> returnShapes = new List<Shape>();
            Vertex vertex;

            foreach (var shape in shapes)
            {
                for (int i = 0; i < shape.Vertexes.Count; i++)
                {
                    vertex = shape.Vertexes[i];
                    if (vertex.X < box.XMin || vertex.X > box.XMax ||
                        vertex.Y < box.YMin || vertex.Y > box.YMax)
                    {
                        returnShapes.Add(shape);
                        break;
                    }
                }
            }

            return returnShapes;
        }
    }
}
