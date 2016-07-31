using System;
using System.IO;
using System.Linq;

namespace ShapefileLibrary
{
    internal class FileWriter
    {
        private FileStream fs;

        internal FileWriter(FileStream fs)
        {
            this.fs = fs;
        }

        internal void WriteInt(int offset, int subject)
        {
            byte[] bs = BitConverter.GetBytes(subject);
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Write(bs, 0, 4);
        }

        internal void WriteReverseInt(int offset, int subject)
        {
            byte[] bs = BitConverter.GetBytes(subject);
            bs = bs.Reverse().ToArray();
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Write(bs, 0, 4);
        }

        internal void WriteDouble(int offset, double subject)
        {
            byte[] bs = BitConverter.GetBytes(subject);
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Write(bs, 0, 8);
        }
    }
}
