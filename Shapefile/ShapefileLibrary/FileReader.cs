using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShapefileLibrary
{
    internal class FileReader
    {
        private FileStream fs;

        internal FileReader(FileStream fs)
        {
            this.fs = fs;
        }

        internal int ReadInt(int offset)
        {
            byte[] bs = new byte[4];
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Read(bs, 0, 4);
            return BitConverter.ToInt32(bs, 0);
        }

        internal int ReadReverseInt(int offset)
        {
            byte[] bs = new byte[4];
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Read(bs, 0, 4);
            bs = bs.Reverse().ToArray();
            return BitConverter.ToInt32(bs, 0);
        }

        internal double ReadDouble(int offset)
        {
            byte[] bs = new byte[8];
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Read(bs, 0, 8);
            return BitConverter.ToDouble(bs, 0);
        }
    }
}
