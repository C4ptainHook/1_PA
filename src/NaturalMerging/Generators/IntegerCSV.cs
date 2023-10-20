using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NaturalMerging.Generators
{
    internal class IntegerCSV : IGenerator
    {
        private Random _gen = new Random();
        private Mutex writerMutex = new Mutex();
        private const int _genBufferSize = 200;
        private const int _fileBufferSize = 64000;
        Task write;
        public string Filename { get; set; }
        public long Filesize { get; set; }
        public int Lowerbound { get; set; }
        public int Upperbound { get; set; }

        public IntegerCSV(string filename) => this.Filename = filename;
        public IntegerCSV(string filename, long filesize) : this(filename) => this.Filesize = filesize;
        public IntegerCSV(string filename, long filesize, int lowerbound, int upperbound) : this(filename, filesize) 
        {
            this.Lowerbound = lowerbound;
            this.Upperbound = upperbound;
        }
        public void Generate()
        {
            short _recordSize = (short)(Math.Floor(Math.Log10(Upperbound) + 1) / 2 * sizeof(char));
            long currentSize = 0;
            string[] gen_buffer = new string[_genBufferSize];

            FileStream fileStream = new FileStream(Filename, FileMode.Create);
            BufferedStream bufferedStream = new BufferedStream(fileStream, _fileBufferSize);
            StreamWriter streamWriter = new StreamWriter(bufferedStream);
            
                while (currentSize < Filesize)
                {
                    for (int i = 0; i < gen_buffer.Length; i++)
                    {
                        gen_buffer[i] = _gen.Next(Lowerbound, Upperbound).ToString() + ',';
                    }
                   write = new(()=>
                    { 
                        string writeBuffer = string.Join("", gen_buffer); 
                        writerMutex.WaitOne(); streamWriter.Write(writeBuffer); writerMutex.ReleaseMutex(); 
                    });
                    write.Start();
                    currentSize += _recordSize * _genBufferSize;
                }

        }
    }
}
