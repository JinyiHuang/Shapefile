using Painting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShapefileLibrary
{
    internal class ShxFile
    {
        private Dictionary<int, int> content;
        public string Path { get; set; }

        internal protected ShxFile(string path)
        {
            this.Path = path;
        }

        internal protected Dictionary<int, int> ReadContent()
        {
            if (this.content != null)
            {
                return this.content;
            }
            else
            {
                this.content = new Dictionary<int, int>();
                using (FileStream fs = new FileStream(this.Path, FileMode.Open))
                {
                    FileReader fr = new FileReader(fs);

                    int length = fr.ReadReverseInt(24) * 2;
                    int count = (length - 100) / 8;

                    int offset, contentLength;
                    for (int i = 0; i < count; i++)
                    {
                        offset = fr.ReadReverseInt(100 + 8 * i) * 2;
                        contentLength = fr.ReadReverseInt(104 + 8 * i) * 2;
                        content.Add(offset, contentLength);
                    }
                }

                return content;
            }
        }

        internal protected void Save() { }

        internal static void Save(string path, BoundingBox boundingBox, List<Shape> shapes)
        {
            WriteHeader(path, boundingBox, shapes);

            switch (shapes[0].Type)
            {
                case ShapeType.NullType:
                    break;
                case ShapeType.Point:
                    WritePointRecord(path, shapes);
                    break;
                case ShapeType.PolyLine:
                    WritePolyLineRecord(path, shapes);
                    break;
                case ShapeType.Polygon:
                    WritePolygonRecord(path, shapes);
                    break;
                default:
                    break;
            }
        }

        protected static void WritePointRecord(string path, List<Shape> shapes)
        {
            using (FileStream fs=new FileStream(path,FileMode.Append))
            {
                FileWriter fw = new FileWriter(fs);

                for (int i = 0; i < shapes.Count; i++)
                {
                    fw.WriteReverseInt(100 + i * 8, (100 + i * 28) / 2);
                    fw.WriteReverseInt(104 + i * 8, 10);
                }
            }
        }

        protected static void WritePolyLineRecord(string path, List<Shape> shapes)
        {
            using (FileStream fs = new FileStream(path, FileMode.Append))
            {
                FileWriter fw = new FileWriter(fs);

                int offset = 0;
                for (int i = 0; i < shapes.Count; i++)
                {
                    fw.WriteReverseInt(100 + i * 8, (100 + offset) / 2);
                    offset += 44 + 4 + 16 * shapes[i].Vertexes.Count + 8;
                    fw.WriteReverseInt(104 + i * 8, (44 + 4 + 16 * shapes[i].Vertexes.Count) / 2);
                }
            }
        }

        protected static void WritePolygonRecord(string path, List<Shape> shapes)
        {
            using (FileStream fs = new FileStream(path, FileMode.Append))
            {
                FileWriter fw = new FileWriter(fs);

                int offset = 0;
                for (int i = 0; i < shapes.Count; i++)
                {
                    fw.WriteReverseInt(100 + i * 8, (100 + offset) / 2);
                    offset += 44 + 4 + 16 * shapes[i].Vertexes.Count + 8;
                    fw.WriteReverseInt(104 + i * 8, (44 + 4 + 16 * shapes[i].Vertexes.Count) / 2);
                }
            }
        }

        protected static void WriteHeader(string path, BoundingBox boundingBox, List<Shape> shapes)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                FileWriter fw = new FileWriter(fs);

                //Write header
                //Write file code
                fw.WriteReverseInt(0, 9994);

                //Write unused fields
                for (int i = 0; i < 4; i++)
                {
                    fw.WriteReverseInt(i * 4 + 4, 0);
                }

                //Write file length
                int fileLength = (100 + 8 * shapes.Count) / 2;
                fw.WriteReverseInt(24, fileLength);

                //Write version
                fw.WriteInt(28, 1000);

                //Write shape type
                if (shapes == null)
                {
                    fw.WriteInt(32, 0);
                }
                else
                {
                    fw.WriteInt(32, (int)shapes[0].Type);
                }

                //Write bounding box
                fw.WriteDouble(36, boundingBox.XMin);
                fw.WriteDouble(44, boundingBox.YMin);
                fw.WriteDouble(52, boundingBox.XMax);
                fw.WriteDouble(60, boundingBox.YMax);
                fw.WriteDouble(68, 0);
                fw.WriteDouble(76, 0);
                fw.WriteDouble(84, 0);
                fw.WriteDouble(92, 0);

            }
        }
    }
}
