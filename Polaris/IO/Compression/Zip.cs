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
using System.IO.Compression;
using System.Threading.Tasks;

namespace Polaris.IO.Compression
{
    internal static class Zip
    {
        internal static async Task Compress(Stream inputStream, Stream outputStream)
        {
            using (var compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
                await inputStream.CopyToAsync(compressionStream).ConfigureAwait(false);
        }
        
        internal static async Task Decompress(Stream inputStream, Stream outputStream)
        {
            using (var decompressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
                await decompressionStream.CopyToAsync(outputStream).ConfigureAwait(false);
        }
    }
}