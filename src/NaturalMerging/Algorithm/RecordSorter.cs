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
                Tuple<bool, bool> marker;
                do
                {
                    marker = mainFileBridge.PassRun();
                    if (!ToRight)
                        AFileBridge.Write();
                    else
                        BFileBridge.Write();

                    if (marker.Item1)
                        ToRight = !ToRight;

                }while(!marker.Item2);
            }
        }
        private void Merge() 
        {
            using (var mainFileBridge = new Transmitter(fileName, sharedBuffer))
            using (var AFileBridge = new Transmitter(AFile, sharedBuffer))
            using (var BFileBridge = new Transmitter(BFile, sharedBuffer))
            {
                bool FAE = AFileBridge.IsReadUsed();
                bool FBE = BFileBridge.IsReadUsed();
                bool AER = false;
                bool BER = false;
                Tuple<bool, bool> bridgeResponse;
                if (!FAE && !FBE)
                {
                    AFileBridge.PassRecord();
                    BFileBridge.PassRecord();
                    sharedBuffer.ClearReservedMarker();
                }
                while (!FAE && !FBE)
                {
                    switch (mainFileBridge.CompareRecords()) 
                    {
                        case 0: 
                            {
                                bridgeResponse = AFileBridge.PassRecord();
                                AER = bridgeResponse.Item1;
                                FAE = bridgeResponse.Item2;
                                break;
                            }
                        case 1:
                            {
                                sharedBuffer.NextReserved();
                                bridgeResponse = BFileBridge.PassRecord();
                                sharedBuffer.ClearReservedMarker();
                                BER = bridgeResponse.Item1;
                                FBE = bridgeResponse.Item2;
                                break;
                            }
                        
                    }

                    while (!AER && BER)
                    {
                        bridgeResponse = AFileBridge.PassRecord();
                        AER = bridgeResponse.Item1;
                        FAE = bridgeResponse.Item2;
                    }
                    while (AER && !BER)
                    {
                        sharedBuffer.NextReserved();
                        bridgeResponse = BFileBridge.PassRecord();
                        sharedBuffer.ClearReservedMarker();
                        BER = bridgeResponse.Item1;
                        FBE = bridgeResponse.Item2;
                    }
                }
                while(!FAE && FBE)
                {
                    bridgeResponse = AFileBridge.PassRun();
                    FAE = bridgeResponse.Item2;
                    mainFileBridge.Write();
                }
                while (FAE && !FBE)
                {
                    bridgeResponse = BFileBridge.PassRun();
                    FBE = bridgeResponse.Item2;
                    mainFileBridge.Write();
                }
            }
        }

        public RecordSorter(string fileName)
        {
            this.fileName = fileName;
            sharedBuffer = new Buffer(Constants.GenBufferSize,1);
        }

        public void Sort() 
        {
            Merge();
            //Distribute();
        }
    }
}
