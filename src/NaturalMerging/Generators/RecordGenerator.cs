using Microsoft.VisualBasic;
using NaturalMerging.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NaturalMerging.Generators
{
    internal class RecordGenerator : IGenerator
    {
        private Random _gen = new Random();
        private const int _genBufferSize = 200;
        public string Filename { get; set; }
        public long Filesize { get; set; }
        public int Lowerbound { get; set; }
        public int Upperbound { get; set; }

        public RecordGenerator(string filename) => this.Filename = filename;
        public RecordGenerator(string filename, long filesize) : this(filename) => this.Filesize = filesize;
        public RecordGenerator(string filename, long filesize, int lowerbound, int upperbound) : this(filename, filesize) 
        {
            this.Lowerbound = lowerbound;
            this.Upperbound = upperbound;
        }
        public void Generate()
        {
            short _recordSize = (short)(Math.Floor(Math.Log10(Upperbound) + 1) / 2 * sizeof(char));
            long currentSize = 0;
            string[] gen_buffer = new string[_genBufferSize];
            AsyncWriter writer = new AsyncWriter(Filename);
           
            while (currentSize < Filesize)
            {
                for (int i = 0; i < gen_buffer.Length; i++)
                {
                    gen_buffer[i] = _gen.Next(Lowerbound, Upperbound).ToString() + '\n';
                }
                writer.Write(gen_buffer);
                currentSize += _recordSize * _genBufferSize;
            }

            writer.Dispose();


        }
    }
}
