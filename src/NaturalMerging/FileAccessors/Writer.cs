using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NaturalMerging.FileAccessors
{

    internal class Writer : IDisposable
    {
        private static Mutex writerMutex = new Mutex();
        private bool disposed = false;

        FileStream fileStream;
        BufferedStream bufferedStream;
        StreamWriter streamWriter;

        public Writer(string filename)
        {
            fileStream = new FileStream(filename, FileMode.OpenOrCreate);
            bufferedStream = new BufferedStream(fileStream, Constants.GenBufferSize);
            streamWriter = new StreamWriter(bufferedStream);
        }

        public async Task WriteAsync(string content)
        {
            writerMutex.WaitOne();
            try
            {
                await streamWriter.WriteAsync(content);
            }
            finally
            {
                writerMutex.ReleaseMutex();
            }
        }

        public void Write(string content)
        {
            streamWriter.Write(content);
        }
        public long GetLength()
        {
            return streamWriter.BaseStream.Length;
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
                    streamWriter.Dispose();
                    bufferedStream.Dispose();
                    fileStream.Dispose();
                }

                writerMutex.Dispose();

                disposed = true;
            }
        }
    }

}
