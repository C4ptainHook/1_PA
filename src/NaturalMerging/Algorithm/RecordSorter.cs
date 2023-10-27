﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging.Algorithm
{
    internal class RecordSorter
    {
        private string AFile = @"C:\Users\boyko\Desktop\AFile.csv";
        private string BFile = @"C:\Users\boyko\Desktop\BFile.csv";
        private string fileName;
        private Buffer sharedBuffer;
        private void Distribute()
        {
            bool ToRight = false;
            using (var mainFileBridge = new Transmitter(fileName, sharedBuffer))
            using (var AFileBridge = new Transmitter(AFile, sharedBuffer))
            using (var BFileBridge = new Transmitter(BFile, sharedBuffer))
            {
                Tuple<bool, bool> indicator;
                while (!(indicator.Item1 = mainFileBridge.ExtractAll().Item1))
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
        public void Sort() 
        {
            Distribute();
        }
        public RecordSorter(string fileName) 
        {
            this.fileName = fileName;
            sharedBuffer = new Buffer(Constants.GenBufferSize);
        }
    }
}
