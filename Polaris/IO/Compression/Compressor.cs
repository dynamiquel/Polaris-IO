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

using System.IO;
using System.Threading.Tasks;

namespace Polaris.IO.Compression
{
    public static class Compressor
    {
        public static async Task Compress(Stream inputStream, Stream outputStream, CompressionType compressionType = CompressionType.Zip)
        {
            switch (compressionType)
            {
                // A contingency. This shouldn't be called if conditions were done correctly early on.
                case CompressionType.None:
                    await inputStream.CopyToAsync(outputStream).ConfigureAwait(false);
                    return;
                case CompressionType.Zip:
                    await Zip.Compress(inputStream, outputStream).ConfigureAwait(false);
                    break;
                case CompressionType.Lz4:
                    await Lz4.Compress(inputStream, outputStream).ConfigureAwait(false);
                    break;
                case CompressionType.Lzma:
                    Lzma.Compress(inputStream, outputStream);
                    break;
                default:
                    return;
            }
        }
        
        public static async Task Decompress(Stream inputStream, Stream outputStream, CompressionType compressionType = CompressionType.Zip)
        {
            switch (compressionType)
            {
                // A contingency. This shouldn't be called if conditions were done correctly early on.
                case CompressionType.None:
                    await inputStream.CopyToAsync(outputStream).ConfigureAwait(false);
                    break;
                case CompressionType.Zip:
                    await Zip.Decompress(inputStream, outputStream).ConfigureAwait(false);
                    break;
                case CompressionType.Lz4:
                    await Lz4.Decompress(inputStream, outputStream).ConfigureAwait(false);
                    break;
                case CompressionType.Lzma:
                    Lzma.Decompress(inputStream, outputStream);
                    break;
                default:
                    return;
            }
        }
    }
    
    /// <summary>
    /// The type of compression to use for serialisation and deserialisation.
    /// Note: The type used for deserialisation must match the type used for serialisation.
    /// <br></br>
    /// <br></br> None: No compression or decompression will be done.
    /// <br></br> Gzip: Mediocre ratio, mediocre time. Ideal for general use. Typical example: Loading data at scene load. (Can be opened by 7-Zip).
    /// <br></br> Lz4: High ratio, fast time. Ideal for frequent access/network messages. Typical example: Loading data on-demand.
    /// <br></br> Lzma: Low ratio, slow time (especially compression time). Ideal for archiving. Typical example: Loading data at game start-up. (Can be opened by 7-Zip).
    /// </summary>
    public enum CompressionType : byte
    {
        None,
        Zip,
        Lz4,
        Lzma
    }
}