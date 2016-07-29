using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
