//  This file is part of Polaris-IO - An IO wrapper for Unity.
//  https://github.com/dynamiquel/Polaris-IO
//  Copyright (c) 2020 dynamiquel

//  MIT License
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using System.IO;
using SevenZip;
using SevenZip.Compression.LZMA;

namespace Polaris.IO.Compression
{
    internal static class Lzma
    {
        internal static void Compress(Stream inputStream, Stream outputStream)
        {
            // I don't think these properties are needed but it works with them.
            var dictionary = 1 << 23;
            var posStateBits = 2;
            var litContextBits = 3; // for normal files
            // UInt32 litContextBits = 0; // for 32-bit data
            var litPosBits = 0;
            // UInt32 litPosBits = 2; // for 32-bit data
            var algorithm = 2;
            var numFastBytes = 128;

            var mf = "bt4";
            var eos = true;
            var stdInMode = false;


            CoderPropID[] propIDs =  {
                CoderPropID.DictionarySize,
                CoderPropID.PosStateBits,
                CoderPropID.LitContextBits,
                CoderPropID.LitPosBits,
                CoderPropID.Algorithm,
                CoderPropID.NumFastBytes,
                CoderPropID.MatchFinder,
                CoderPropID.EndMarker
            };

            object[] properties = {
                dictionary,
                posStateBits,
                litContextBits,
                litPosBits,
                algorithm,
                numFastBytes,
                mf,
                eos
            };
            
            var encoder = new Encoder();
            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(outputStream);
            
            Int64 fileSize;
            if (eos || stdInMode)
                fileSize = -1;
            else
                fileSize = inputStream.Length;
            
            for (byte i = 0; i < 8; i++)
                outputStream.WriteByte((byte)(fileSize >> (8 * i)));
            
            encoder.Code(inputStream, outputStream, -1, -1, null);
        }
        
        internal static void Decompress(Stream inputStream, Stream outputStream)
        {
            var decoder = new Decoder();
            
            byte[] properties = new byte[5];
            if (inputStream.Read(properties, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));
            decoder.SetDecoderProperties(properties);
            
            long outSize = 0;
            
            for (int i = 0; i < 8; i++)
            {
                int v = inputStream.ReadByte();
                if (v < 0)
                    throw (new Exception("Can't Read 1"));
                outSize |= ((long)(byte)v) << (8 * i);
            }
            
            long compressedSize = inputStream.Length - inputStream.Position;

            decoder.Code(inputStream, outputStream, inputStream.Length, -1, null);
        }
    }
}