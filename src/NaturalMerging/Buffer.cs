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
        private string[] Reserved;
        private int CurrentReserved = 0;
        private int Size;
        private int ReservedSpace;

        public int Capacity { get; private set; }
        public bool IsFull { get => Size == Capacity; }
        public bool IsReservedFinished { get => CurrentReserved == ReservedSpace-1; }
        public bool IsEmpty { get => Size == 0; }

        public Buffer(int capacity, int reservedCapacity)
        {
            Capacity = capacity;
            ReservedSpace = reservedCapacity;
            Records = new string[capacity];
            Reserved = new string[reservedCapacity];
            Size = 0;
            for(int i = 0; i < ReservedSpace; i++) 
            {
                Reserved[i] = int.MinValue.ToString();
            }
        }

        public void Append(string record)
        {
            if (IsFull) throw new Exception("Is not possible to append to a full buffer");

            Records[Size] = record;
            Size++;
        }
        public void Reserve(string record)
        {
            Reserved[CurrentReserved] = record;
        }

        public void NextReserved()
        {
            if (IsReservedFinished) throw new Exception("Is not possible to outbreak reserve capacity");
            CurrentReserved++;
        }

        public string PeekReserved()
        {
            return Reserved[CurrentReserved];
        }

        public void Clear()
        {
            Size = 0;
        }
        public void ClearReservedMarker()
        {
            CurrentReserved = 0;
        }
        public void ClearAll()
        {
            Clear();
            ClearReservedMarker();
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
