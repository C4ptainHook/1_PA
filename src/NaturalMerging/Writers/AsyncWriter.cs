using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NaturalMerging.Writers
{

    internal class AsyncWriter: IDisposable
    {
        private static Mutex writerMutex = new Mutex();
        private const int _fileBufferSize = 64000;
        FileStream fileStream;
        BufferedStream bufferedStream;
        StreamWriter streamWriter;
        public AsyncWriter(string filename)
        {
            fileStream = new FileStream(filename, FileMode.Create);
            bufferedStream = new BufferedStream(fileStream, _fileBufferSize);
            streamWriter = new StreamWriter(bufferedStream);
        }
        public void Write(string[] gen_buffer)
        {
            string content = string.Join("", gen_buffer);
            Task.Run(() => 
            {
                writerMutex.WaitOne();
                streamWriter.Write(content);
                writerMutex.ReleaseMutex();
            });
        }
        public void Dispose() 
        {
            streamWriter.Dispose();
            bufferedStream.Dispose();
            fileStream.Dispose();
        }
    }
}
