using System.Collections.Generic;
using System.Linq;

namespace Painting
{
    public class PolyLine:Shape
    {
        public PolyLine(IEnumerable<Vertex> vertexes)
        {
            this.Type = ShapeType.PolyLine;
            this.Vertexes = vertexes.ToList();
        }
    }
}
