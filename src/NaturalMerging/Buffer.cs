using NaturalMerging.ExtensionMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging
{
    public class Buffer : IEnumerable
    {
        private string[] Records;
        private int CurrentGeneral = 0;
        private int CurrentReserved;
        private int Size;
        private int ReservedSpace = 1;

        public int Capacity { get; private set; }
        public bool IsFull { get => Size == Capacity; }
        public bool IsFinished { get => CurrentGeneral == Size; }
        public bool IsEmpty { get => Size == 0; }

        public Buffer(int capacity)
        {
            Capacity = capacity;
            Records = new string[capacity + ReservedSpace];
            CurrentReserved = Capacity;
            Records[CurrentReserved] = int.MinValue.ToString();
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
            return Records[CurrentGeneral];
        }
   
        public string Next()
        {
            if (IsFinished) CurrentGeneral = 0;
            string record = Records[CurrentGeneral];
            CurrentGeneral++;
            return record;
        }

        public void Reserve(string record) 
        {
            Records[CurrentReserved] = record;
        }

        public string PeekReserved()
        {
            return Records[CurrentReserved];
        }

        public void Clear()
        {
            Size = 0;
        }
        public void ClearMarker()
        {
            CurrentGeneral = 0;
        }
        public void ClearAll()
        {
            Clear();
            ClearMarker();
        }

        public override string ToString()
        {
            if (this.IsEmpty) throw new Exception("Attemp to stringify empty buffer");
            ReadOnlySpan<string> numeric = new ReadOnlySpan<string>(Records);
            return StringExtensions.Join('\n', numeric[..Size]);
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
