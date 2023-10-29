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
        public bool CopyRun(Buffer readBuffer) 
        {
            bool EOR = false;
            int currentRecord;
            int nextRecord;
            if(int.MinValue < int.Parse(readBuffer.PeekReserved()))
            {
                currentRecord = int.Parse(readBuffer.PeekReserved());
                readBuffer.Append(currentRecord.ToString());
            }
            else 
            {
                int.TryParse(streamReader.ReadLine(), out currentRecord);
                readBuffer.Append(currentRecord.ToString());
            }
           

            while (streamReader.Peek() >= 0)
            {
                int.TryParse(streamReader.ReadLine(), out nextRecord);
                readBuffer.Reserve(nextRecord.ToString());
                int delta = currentRecord - nextRecord;
                if (delta <= 0)
                {
                    if (!readBuffer.IsFull)
                    {
                        readBuffer.Append(nextRecord.ToString());
                        currentRecord = nextRecord;
                    }
                    else break;
                }
                else 
                { 
                    EOR = true;
                    break;
                }
            }

            return EOR;
        }

        public Tuple<bool,bool> CopyRecord(Buffer readBuffer) 
        {
            bool EOR = false;
            bool EOF;
            if (streamReader.Peek() >= 0)
            {
                EOF = false;
                string nextRecord = streamReader.ReadLine();
                if (int.Parse(readBuffer.PeekReserved()) > int.Parse(nextRecord))
                {
                    EOR = true;
                }
                readBuffer.Reserve(nextRecord);
            }
            else 
            {
                EOF = true;
                EOR = true;
            }
            return new(EOR,EOF);
        }
        public bool IsUsed()
        {
            return streamReader.Peek() <= 0;
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
