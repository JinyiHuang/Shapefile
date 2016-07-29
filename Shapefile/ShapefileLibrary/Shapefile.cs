using Painting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShapefileLibrary
{
    public class Shapefile
    {
        private List<Shape> shapes;

        public string FilePath { get; set; }
        public BoundingBox Box { get; protected set; }

        public Shapefile(string path)
        {
            this.FilePath = path;
            string shxPath = path.Replace(Path.GetExtension(path), "") + ".shx";
            ShxFile shxFile = new ShxFile(shxPath);
            Dictionary<int, int> indexes = shxFile.ReadContent();
            string shpPath= path.Replace(Path.GetExtension(path), "") + ".shp";
            ShpFile shpFile = new ShpFile(shpPath);
            this.Box = shpFile.ReadBoundingBox();
            this.shapes = shpFile.ReadShapes(indexes);
        }

        public Shapefile(IEnumerable<Shape> shapes)
        {
            this.shapes = shapes.ToList();
            this.Box = BoundingBox.GetBoundingBox(shapes);
        }

        public List<Shape> GetShapes()
        {
            return shapes;
        }

        public List<Shape> GetShapes(bool isTranslated)
        {
            if (isTranslated)
            {
                Transformer tf = new Transformer();
                return tf.Translate(this.shapes, this.Box);
            }
            else
            {
                return GetShapes();
            }
        }

        public List<Shape> GetShapes(BoundingBox boundingBox, bool isInside)
        {
            if (boundingBox > Box && isInside)
            {
                return null;
            }
            else
            {
                List<Shape> shapes = Spliter.Split(this.shapes, boundingBox, isInside);
                return shapes;
            }
        }

        public bool Sava(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            else
            {
                this.FilePath = path;
                string shxPath = this.FilePath.Replace(Path.GetExtension(this.FilePath), "") + ".shx";
                ShxFile.Save(shxPath, Box, shapes);
                string shpPath = this.FilePath.Replace(Path.GetExtension(this.FilePath), "") + ".shp";
                ShpFile.Save(shpPath, Box, shapes);
                return true;
            }
        }
    }
}
