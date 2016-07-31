using System.Collections.Generic;

namespace Painting
{
    public class Shape
    {
        public ShapeType Type { get; protected set; }
        public List<Vertex> Vertexes { get; protected set; }

        protected Shape() { }
    }
}
