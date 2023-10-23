using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NaturalMerging.Writers
{

    internal class AsyncWriter : IDisposable
    {
        private static Mutex writerMutex = new Mutex();
        private bool disposed = false;

        FileStream fileStream;
        BufferedStream bufferedStream;
        StreamWriter streamWriter;

        public AsyncWriter(string filename)
        {
            fileStream = new FileStream(filename, FileMode.Create);
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
