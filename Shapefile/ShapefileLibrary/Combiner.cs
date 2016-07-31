using Painting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapefileLibrary
{
    public class Combiner
    {
        public Combiner()
        {

        }

        public Shapefile Combine(IEnumerable<Shapefile> shapefiles)
        {
            if (shapefiles == null)
            {
                throw new ArgumentNullException("shapefiles");
            }

            Shapefile sf = shapefiles.FirstOrDefault();
            if (sf != null)
            {
                ShapeType type = sf.GetShapes()[0].Type;
                List<Shape> shapes = new List<Shape>();

                foreach (var shapefile in shapefiles)
                {
                    foreach (var shape in shapefile.GetShapes())
                    {
                        if (type != shape.Type)
                        {
                            throw new ArgumentException("The types of shapes in shapefile are different", "shapefile");
                        }
                        else
                        {
                            shapes.Add(shape);
                        }
                    }
                }

                return new Shapefile(shapes);
            }
            else
            {
                throw new ArgumentException("There is no elements in shapefiles");
            }
        }
    }
}
