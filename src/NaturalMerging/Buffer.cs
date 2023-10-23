using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging
{
    public class Buffer : IEnumerable
    {
        private string[] Records;
        private int Current = 0;
        private int Size;

        public int Capacity { get; private set; }
        public bool IsFull { get => Size == Capacity; }
        public bool IsFinished { get => Current == Size; }
        public bool IsEmpty { get => Size == 0; }

        public Buffer(int capacity)
        {
            Capacity = capacity;
            Records = new string[capacity];
            Size = 0;
        }

        public void Append(string record)
        {
            if (IsFull) throw new Exception("Is not possible to append to a full buffer");

            Records[Size] = record;
            Size++;
        }

        public string Peek()
        {
            return Records[Current];
        }
 
        public string Next()
        {
            string record = Records[Current];
            Current++;
            if (IsFinished) Clear();
            return record;
        }

        public void Clear()
        {
            Size = 0;
            Current = 0;
        }
        public override string ToString()
        {
            return string.Join('\n', Records);
        }
        public IEnumerator GetEnumerator()
        {
            for (int index = 0; index < Size; index++)
            {
                yield return Records[index];
            }
        }
    }
}
