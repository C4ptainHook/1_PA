using NaturalMerging.Writers;
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
            Buffer writeBuffer = new Buffer(Constants.GenBufferSize);
            AsyncWriter writer = new AsyncWriter(Filename);
            
                var recordGenerator = new RecordGenerator(writeBuffer, 1, 10000);
                short recordSize = recordGenerator.RecordSize;
                long currentSize = 0;
                while (currentSize < Filesize)
                {
                    recordGenerator.Generate();
                    writer.Write(writeBuffer);
                    currentSize += Constants.GenBufferSize * recordSize;
                }
            
        }
    }
}