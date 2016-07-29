using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painting
{
    public class Point : Shape
    {
        public Point(Vertex vertex)
        {
            this.Type = ShapeType.Point;
            this.Vertexes = new List<Vertex>();
            Vertexes.Add(vertex);
        }

    }
}
