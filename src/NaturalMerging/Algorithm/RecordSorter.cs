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
            sharedBuffer = new Buffer(Constants.GenBufferSize, 1);
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

                } while (!marker.Item2);
            }
        }
        private int Merge()
        {
            int numberOfRuns = 0;
            sharedBuffer = new Buffer(Constants.GenBufferSize, 2);
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
                    sharedBuffer.NextReserved();
                    BFileBridge.PassRecord();
                    sharedBuffer.ClearReservedMarker();
                

                    while (!FAE && !FBE)
                    {
                        AER = false;
                        BER = false;
                        switch (mainFileBridge.CompareRecords(false))
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
                            mainFileBridge.CompareRecords(true);
                            bridgeResponse = AFileBridge.PassRecord();
                            AER = bridgeResponse.Item1;
                            FAE = bridgeResponse.Item2;
                        }
                        while (AER && !BER)
                        {
                            sharedBuffer.NextReserved();
                            mainFileBridge.CompareRecords(true);
                            bridgeResponse = BFileBridge.PassRecord();
                            sharedBuffer.ClearReservedMarker();
                            BER = bridgeResponse.Item1;
                            FBE = bridgeResponse.Item2;
                        }
                        numberOfRuns++;
                    }
                    mainFileBridge.Write();
            }

            while (!FAE && FBE)
            {
                sharedBuffer.ClearReservedMarker();
                mainFileBridge.CompareRecords(true);
                bridgeResponse = AFileBridge.PassRecord();
                FAE = bridgeResponse.Item2;
            }
            while (FAE && !FBE)
            {
                sharedBuffer.NextReserved();
                mainFileBridge.CompareRecords(true);
                bridgeResponse = BFileBridge.PassRecord();
                sharedBuffer.ClearReservedMarker();
                FBE = bridgeResponse.Item2;
            }
            if (!sharedBuffer.IsEmpty) mainFileBridge.Write();
            }
            return numberOfRuns;
        }

        public RecordSorter(string fileName)
        {
            this.fileName = fileName;
        }

        public void Sort()
        {

        }
    }
}
