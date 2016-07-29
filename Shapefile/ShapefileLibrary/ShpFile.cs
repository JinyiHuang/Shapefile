using Painting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShapefileLibrary
{
    internal class ShpFile
    {
        private List<Shape> shapes;
        private BoundingBox boundingBox;

        public string FilePath { get; protected set; }

        internal protected ShpFile(string path)
        {
            this.FilePath = path;
        }

        internal protected BoundingBox ReadBoundingBox()
        {
            if (this.boundingBox != null)
            {
                return this.boundingBox;
            }
            else
            {
                double xMin, xMax, yMin, yMax;
                using (FileStream fs = new FileStream(this.FilePath, FileMode.Open))
                {
                    FileReader fr = new FileReader(fs);

                    xMin = fr.ReadDouble(36);
                    yMin = fr.ReadDouble(44);
                    xMax = fr.ReadDouble(52);
                    yMax = fr.ReadDouble(60);

                    this.boundingBox = new BoundingBox(xMin, xMax, yMin, yMax);
                }
                return this.boundingBox;
            }
        }

        internal List<Shape> ReadShapes(Dictionary<int, int> index)
        {
            if (this.shapes != null)
            {
                return this.shapes;
            }
            else
            {
                using (FileStream fs = new FileStream(this.FilePath, FileMode.Open))
                {
                    FileReader fr = new FileReader(fs);

                    ShapeType type = (ShapeType)fr.ReadInt(32);
                    switch (type)
                    {
                        case ShapeType.NullType:
                            break;
                        case ShapeType.Point:
                            this.shapes = ReadPoints(fs, index);
                            break;
                        case ShapeType.PolyLine:
                            this.shapes = ReadPolyLines(fs, index);
                            break;
                        case ShapeType.Polygon:
                            this.shapes = ReadPolygons(fs, index);
                            break;
                        default:
                            break;
                    }
                }
                return this.shapes;
            }
        }

        protected List<Shape> ReadPoints(FileStream fs, Dictionary<int, int> index)
        {
            List<Shape> shapes = new List<Shape>();
            Point point;
            Vertex vertex;
            double x, y;
            FileReader fr = new FileReader(fs);

            foreach (var offset in index.Keys)
            {
                x = fr.ReadDouble(offset + 12);
                y = fr.ReadDouble(offset + 20);
                vertex = new Vertex(x, y);
                point = new Point(vertex);
                shapes.Add(point);
            }

            return shapes;
        }

        protected List<Shape> ReadPolyLines(FileStream fs, Dictionary<int, int> index)
        {
            List<Shape> shapes = new List<Shape>();
            PolyLine polyLine;
            List<Vertex> vertexes = new List<Vertex>();
            double x, y;
            int partsCount, vertexesCount;
            FileReader fr = new FileReader(fs);

            foreach (var offset in index.Keys)
            {
                int part = 0, nextPart = 0;
                partsCount = fr.ReadInt(offset + 44);
                vertexesCount = fr.ReadInt(offset + 48);

                for (int i = 0; i < partsCount - 1; i++)
                {
                    part = fr.ReadInt(offset + 52 + 4 * i);
                    nextPart = fr.ReadInt(offset + 52 + 4 * (i + 1));
                    vertexes = new List<Vertex>();

                    for (int j = part; j < nextPart; j++)
                    {
                        x = fr.ReadDouble(offset + 52 + 4 * partsCount + 16 * j);
                        y = fr.ReadDouble(offset + 60 + 4 * partsCount + 16 * j);
                        vertexes.Add(new Vertex(x, y));
                    }
                    polyLine = new PolyLine(vertexes);
                    shapes.Add(polyLine);
                }

                vertexes = new List<Vertex>();
                for (int j = nextPart; j < vertexesCount; j++)
                {
                    x = fr.ReadDouble(offset + 52 + 4 * partsCount + 16 * j);
                    y = fr.ReadDouble(offset + 60 + 4 * partsCount + 16 * j);
                    vertexes.Add(new Vertex(x, y));
                }
                polyLine = new PolyLine(vertexes);
                shapes.Add(polyLine);
            }

            return shapes;
        }

        protected List<Shape> ReadPolygons(FileStream fs, Dictionary<int, int> index)
        {
            List<Shape> shapes = new List<Shape>();
            Polygon polygon;
            List<Vertex> vertexes = new List<Vertex>();
            double x, y;
            int partsCount, vertexesCount;
            FileReader fr = new FileReader(fs);

            foreach (var offset in index.Keys)
            {
                int part = 0, nextPart = 0;
                partsCount = fr.ReadInt(offset + 44);
                vertexesCount = fr.ReadInt(offset + 48);

                for (int i = 0; i < partsCount - 1; i++)
                {
                    part = fr.ReadInt(offset + 52 + 4 * i);
                    nextPart = fr.ReadInt(offset + 52 + 4 * (i + 1));
                    vertexes = new List<Vertex>();

                    for (int j = part; j < nextPart; j++)
                    {
                        x = fr.ReadDouble(offset + 52 + 4 * partsCount + 16 * j);
                        y = fr.ReadDouble(offset + 60 + 4 * partsCount + 16 * j);
                        vertexes.Add(new Vertex(x, y));
                    }
                    polygon = new Polygon(vertexes);
                    shapes.Add(polygon);
                }

                vertexes = new List<Vertex>();
                for (int j = nextPart; j < vertexesCount; j++)
                {
                    x = fr.ReadDouble(offset + 52 + 4 * partsCount + 16 * j);
                    y = fr.ReadDouble(offset + 60 + 4 * partsCount + 16 * j);
                    vertexes.Add(new Vertex(x, y));
                }
                polygon = new Polygon(vertexes);
                shapes.Add(polygon);
            }

            return shapes;
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

        private static void WritePointRecord(string path, List<Shape> shapes)
        {
            using (FileStream fs = new FileStream(path, FileMode.Append))
            {
                FileWriter fw = new FileWriter(fs);

                for (int i = 0; i < shapes.Count; i++)
                {
                    fw.WriteReverseInt(100 + i * 28, i);
                    fw.WriteReverseInt(104 + i * 28, 10);

                    fw.WriteInt(108 + i * 28, 1);
                    fw.WriteDouble(112 + i * 28, shapes[i].Vertexes[0].X);
                    fw.WriteDouble(120 + i * 28, shapes[i].Vertexes[0].Y);
                }
            }
        }

        private static void WritePolyLineRecord(string path, List<Shape> shapes)
        {
            using (FileStream fs = new FileStream(path, FileMode.Append))
            {
                FileWriter fw = new FileWriter(fs);

                Dictionary<int, int> index = new ShxFile(path.Replace(Path.GetExtension(path), "") + ".shx").ReadContent();
                int number = 0;
                foreach (var item in index)
                {
                    fw.WriteReverseInt(item.Key, ++number);
                    fw.WriteReverseInt(item.Key + 4, item.Value);

                    fw.WriteInt(item.Key + 8, 3);

                    BoundingBox box = BoundingBox.GetBoundingBox(shapes[number - 1]);
                    fw.WriteDouble(item.Key + 12, box.XMin);
                    fw.WriteDouble(item.Key + 20, box.XMax);
                    fw.WriteDouble(item.Key + 28, box.YMin);
                    fw.WriteDouble(item.Key + 36, box.YMax);

                    fw.WriteInt(item.Key + 44, 1);
                    fw.WriteInt(item.Key + 48, shapes[number - 1].Vertexes.Count);

                    fw.WriteInt(item.Key + 44, 0);

                    for (int i = 0; i < shapes[number-1].Vertexes.Count; i++)
                    {
                        fw.WriteDouble(item.Key + 48 + i * 16, shapes[number - 1].Vertexes[i].X);
                        fw.WriteDouble(item.Key + 56 + i * 16, shapes[number - 1].Vertexes[i].Y);
                    }
                }
            }
        }

        private static void WritePolygonRecord(string path, List<Shape> shapes)
        {
            using (FileStream fs = new FileStream(path, FileMode.Append))
            {
                FileWriter fw = new FileWriter(fs);

                Dictionary<int, int> index = new ShxFile(path.Replace(Path.GetExtension(path), "") + ".shx").ReadContent();
                int number = 0;
                foreach (var item in index)
                {
                    fw.WriteReverseInt(item.Key, ++number);
                    fw.WriteReverseInt(item.Key + 4, item.Value);

                    fw.WriteInt(item.Key + 8, 3);

                    BoundingBox box = BoundingBox.GetBoundingBox(shapes[number - 1]);
                    fw.WriteDouble(item.Key + 12, box.XMin);
                    fw.WriteDouble(item.Key + 20, box.XMax);
                    fw.WriteDouble(item.Key + 28, box.YMin);
                    fw.WriteDouble(item.Key + 36, box.YMax);

                    fw.WriteInt(item.Key + 44, 1);
                    fw.WriteInt(item.Key + 48, shapes[number - 1].Vertexes.Count);

                    fw.WriteInt(item.Key + 44, 0);

                    for (int i = 0; i < shapes[number - 1].Vertexes.Count; i++)
                    {
                        fw.WriteDouble(item.Key + 48 + i * 16, shapes[number - 1].Vertexes[i].X);
                        fw.WriteDouble(item.Key + 56 + i * 16, shapes[number - 1].Vertexes[i].Y);
                    }
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
                int fileLength = 100;
                switch (shapes[0].Type)
                {
                    case ShapeType.NullType:
                        break;
                    case ShapeType.Point:
                        fileLength += shapes.Count * 28;
                        break;
                    case ShapeType.PolyLine:
                        for (int i = 0; i < shapes.Count; i++)
                        {
                            fileLength += 44 + 4 + 16 * shapes[i].Vertexes.Count + 8;
                        }
                        break;
                    case ShapeType.Polygon:
                        break;
                    default:
                        break;
                }
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
