using System.Collections.Generic;
using System.Linq;

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
