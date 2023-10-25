using NaturalMerging.FileAccessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging
{
    internal class Transmitter
    {
        private string fileName;
        private Buffer buffer;
        private Lazy<Writer> writer;
        private Lazy<Reader> reader;

        public Transmitter(string fileName)
        {
            this.fileName = fileName;
            writer = new Lazy<Writer>();
            reader = new Lazy<Reader>();
        }
        public string Read() { }
        public string Write() { }

    }
}
