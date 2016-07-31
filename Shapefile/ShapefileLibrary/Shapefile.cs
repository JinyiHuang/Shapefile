using Painting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShapefileLibrary
{
    public class Shapefile
    {
        private List<Shape> shapes;

        public string FilePath { get; set; }
        public BoundingBox Box { get; protected set; }

        /// <summary>
        /// Initialize Shapefile class using existing .shp and .shx file.
        /// </summary>
        public Shapefile(string directory, string name)
        {
            this.FilePath = directory + "\\" + name;
            string shxPath = this.FilePath + ".shx";
            ShxFile shxFile = new ShxFile(shxPath);
            Dictionary<int, int> indexes = shxFile.ReadContent();
            string shpPath = this.FilePath + ".shp";
            ShpFile shpFile = new ShpFile(shpPath);
            this.Box = shpFile.ReadBoundingBox();
            this.shapes = shpFile.ReadShapes(indexes);
        }

        /// <summary>
        /// Initialize Shapefile class using the collection of Shape instance.
        /// </summary>
        /// <param name="shapes"></param>
        public Shapefile(IEnumerable<Shape> shapes)
        {
            this.shapes = shapes.ToList();
            this.Box = BoundingBox.GetBoundingBox(shapes);
        }

        /// <summary>
        /// Get the collection of raw shape.
        /// </summary>
        /// <returns></returns>
        public List<Shape> GetShapes()
        {
            return shapes;
        }

        /// <summary>
        /// Get the collection of shape translated from world coordinates to screen coordinates when the parameter is true.
        /// </summary>
        /// <param name="isTranslated"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the collection of shape inside or outside the boundingBox.
        /// </summary>
        /// <param name="boundingBox"></param>
        /// <param name="isInside"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Save the Shapefile as .shp and .shx files.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Save(string directory, string name)
        {
            if (string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(name))
            {
                return false;
            }
            else
            {
                this.FilePath = directory + "\\" + name;
                string shxPath = this.FilePath + ".shx";
                ShxFile.Save(shxPath, Box, shapes);
                string shpPath = this.FilePath + ".shp";
                ShpFile.Save(shpPath, Box, shapes);
                return true;
            }
        }
    }
}
