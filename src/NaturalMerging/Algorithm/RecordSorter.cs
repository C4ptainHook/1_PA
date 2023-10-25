using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging.Algorithm
{
    internal class RecordSorter
    {
        private string AFile;
        private string BFile;
        private string fileName;
        private Buffer sharedBuffer;
        private void Distribute()
        {
            bool ToRight = false;
            using (var mainFileBridge = new Transmitter(fileName, sharedBuffer))
            using (var AFileBridge = new Transmitter(AFile, sharedBuffer))
            using (var BFileBridge = new Transmitter(BFile, sharedBuffer))
            {
                while (!mainFileBridge.ExtractAll())
                {
                    if (!ToRight)
                    {
                        AFileBridge.Write();
                    }
                    else
                    {
                        BFileBridge.Write();
                    }
                }
            }
        }
        public RecordSorter(string fileName) 
        {
            this.fileName = fileName;
            sharedBuffer = new Buffer(Constants.GenBufferSize);
        }
    }
}
