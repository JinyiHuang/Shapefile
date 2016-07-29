using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painting
{
    public class Shape
    {
        public ShapeType Type { get; protected set; }
        public List<Vertex> Vertexes { get; protected set; }

        protected Shape() { }
    }
}
