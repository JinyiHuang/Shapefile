using Painting;
using System.Collections.Generic;

namespace ShapefileLibrary
{
    public class Transformer
    {
        public Transformer()
        {

        }

        public List<Shape> Translate(IEnumerable<Shape> shapes, BoundingBox box)
        {
            List<Shape> transformedShapes = new List<Shape>();
            List<Vertex> vertexes;
            Shape s = null;

            foreach (var shape in shapes)
            {
                vertexes = new List<Vertex>();

                foreach (var vertex in shape.Vertexes)
                {
                    vertexes.Add(new Vertex(vertex.X - box.XMin, vertex.Y - box.YMin));
                }

                switch (shape.Type)
                {
                    case ShapeType.NullType:
                        break;
                    case ShapeType.Point:
                        s = new Point(vertexes[0]);
                        break;
                    case ShapeType.PolyLine:
                        s = new PolyLine(vertexes);
                        break;
                    case ShapeType.Polygon:
                        s = new Polygon(vertexes);
                        break;
                    default:
                        break;
                }

                transformedShapes.Add(s);
            }

            return transformedShapes;
        }

        public List<Shape> Scale(IEnumerable<Shape> shapes, double power)
        {
            List<Shape> transformedShapes = new List<Shape>();
            List<Vertex> vertexes;
            Shape s = null;

            foreach (var shape in shapes)
            {
                vertexes = new List<Vertex>();

                foreach (var vertex in shape.Vertexes)
                {
                    vertexes.Add(new Vertex(vertex.X * power, vertex.Y * power));
                }

                switch (shape.Type)
                {
                    case ShapeType.NullType:
                        break;
                    case ShapeType.Point:
                        s = new Point(vertexes[0]);
                        break;
                    case ShapeType.PolyLine:
                        s = new PolyLine(vertexes);
                        break;
                    case ShapeType.Polygon:
                        s = new Polygon(vertexes);
                        break;
                    default:
                        break;
                }

                transformedShapes.Add(s);
            }

            return transformedShapes;
        }
    }
}
