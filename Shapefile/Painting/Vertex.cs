using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painting
{
    public class Vertex
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vertex(double x,double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
