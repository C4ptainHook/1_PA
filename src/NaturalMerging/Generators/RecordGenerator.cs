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
        private Buffer _buffer;
        public short RecordSize { get; private set; }
        private int Lowerbound;
        private int Upperbound;
        public RecordGenerator(Buffer buffer,int Lowerbound,int Upperbound) 
        {
            _buffer = buffer;
            this.Lowerbound = Lowerbound;
            this.Upperbound = Upperbound;
            RecordSize = (short)(Math.Floor(Math.Log10(Upperbound) + 1) / 2 * sizeof(char));
        }
        public void Generate()
        { 
            while (!_buffer.IsFull)
            {
                _buffer.Append(_gen.Next(Lowerbound, Upperbound).ToString());
            }
        }
    }
}
