using NaturalMerging.FileAccessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging
{
    internal class Transmitter : IDisposable
    {
        private Buffer buffer;
        private Lazy<Writer> writer;
        private Lazy<Reader> reader;
        private bool disposed;

        public Transmitter(string fileName, Buffer buffer)
        {
            this.buffer = buffer;
            writer = new Lazy<Writer>(() => new Writer(fileName));
            reader = new Lazy<Reader>(() => new Reader(fileName));
        }
        public Tuple<bool,bool> PassRun()
        {
            bool EOR = reader.Value.CopyRun(buffer);
            if (!reader.Value.IsUsed())
            {
                return new(EOR, false);
            }
            else return new(EOR, true);
        }
        public void Write() 
        {
            string currentSerie = buffer.ToString();
            buffer.Clear();
            writer.Value.Write(currentSerie);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (writer.IsValueCreated)
                    { 
                        writer.Value.Dispose(); 
                    }
                    if (reader.IsValueCreated) 
                    {
                        reader.Value.Dispose();
                    }
                }
                disposed = true;
            }
        }
    }
}
