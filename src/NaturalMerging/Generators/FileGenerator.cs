using NaturalMerging.FileAccessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging.Generators
{
    internal class FileGenerator : IGenerator
    {
        public string Filename { get; private set; }
        public long Filesize { get; set; }

        public FileGenerator(string filename) => this.Filename = filename;
        public FileGenerator(string filename, long filesize) : this(filename) => this.Filesize = filesize;

        public void Generate()
        {
            Buffer writeBuffer = new Buffer(Constants.GenBufferSize,0);

            using (Writer writer = new Writer(Filename, FileMode.Create))
            {
                var recordGenerator = new RecordGenerator(writeBuffer, 1, 10000);

                while (writer.GetLength() < Filesize)
                {
                    recordGenerator.Generate();
                    writer.WriteAsync(writeBuffer.ToString());
                    writeBuffer.Clear();
                }
            } 
        }
    }
}