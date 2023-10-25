using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging.FileAccessors
{
    internal class Reader : IDisposable
    {
        private FileStream fileStream;
        private BufferedStream bufferedStream;
        private StreamReader streamReader;
        private bool disposed = false;
        public Reader(string filename)
        {
            fileStream = new FileStream(filename, FileMode.Open);
            bufferedStream = new BufferedStream(fileStream, Constants.GenBufferSize);
            streamReader = new StreamReader(bufferedStream);
        }
        public void ReadSerie(Buffer readBuffer) 
        {
            bool IsSerieFinished = false;
            while (!IsSerieFinished)
            {
                if(!streamReader.EndOfStream) {
                    string readRecord = streamReader.ReadLine().TrimEnd('\n');
                    if (readBuffer.IsEmpty || int.Parse(readBuffer.Peek()) < int.Parse(readRecord)) 
                    { 
                        readBuffer.Append(readRecord); 
                        readBuffer.Next(); 
                    }
                } 
            }
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
                    streamReader.Dispose();
                    bufferedStream.Dispose();
                    fileStream.Dispose();
                }
                disposed = true;
            }
        }
    }
}
