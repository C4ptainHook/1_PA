﻿using NaturalMerging.FileAccessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMerging
{
    internal class Transmitter : IDisposable
    {
        private string fileName;
        private Buffer buffer;
        private Lazy<Writer> writer;
        private Lazy<Reader> reader;
        private bool disposed;

        public Transmitter(string fileName, Buffer buffer)
        {
            this.fileName = fileName;
            this.buffer = buffer;
            writer = new Lazy<Writer>();
            reader = new Lazy<Reader>();
        }
        public bool ExtractAll()
        {
            try
            {
                return reader.Value.ReadSerie(buffer);
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); return false; }
        }
        public async void Write() 
        {
            string currentSerie = buffer.ToString();
            buffer.Clear();
            await writer.Value.WriteAsync(currentSerie);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (writer.IsValueCreated)
                    { 
                        writer.Value.Dispose(); 
                    }
                    if (reader.IsValueCreated) 
                    {
                        reader.Value.Dispose();
                    }
                }
                disposed = true;
            }
        }
    }
}