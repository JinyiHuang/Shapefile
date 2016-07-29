using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painting
{
    public class Polygon:Shape
    {
        public Polygon(IEnumerable<Vertex> vertexes)
        {
            this.Type = ShapeType.Polygon;
            this.Vertexes = vertexes.ToList();
        }
    }
}
