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

using System.Threading.Tasks;
using Polaris.IO.Compression;

namespace Polaris.IO
{
    /// <summary>
    /// Essential methods for every file type. Enforces consistency between the different file types.
    /// Use this if you wish to implement another file type.
    /// </summary>
    /*
    internal interface ISerialiser
    {
        void Write(string fileLocation, object value);
        void Write(string fileLocation, object value, CompressionType compressionType);
        Task WriteAsync(string fileLocation, object value);
        Task WriteAsync(string fileLocation, object value, CompressionType compressionType);
        
        T Read<T>(string fileLocation);
        T Read<T>(string fileLocation, CompressionType compressionType);
        Task<T> ReadAsync<T>(string fileLocation);
        Task<T> ReadAsync<T>(string fileLocation, CompressionType compressionType);
        
        bool TryRead<T>(string fileLocation, out T result);
        bool TryRead<T>(string fileLocation, CompressionType compressionType, out T result);
        bool TryWrite(string fileLocation, object value);
        bool TryWrite(string fileLocation, object value, CompressionType compressionType);

        T ReadFromBytes<T>(byte[] bytes);
        T ReadFromBytes<T>(byte[] bytes, CompressionType compressionType);
        Task<T> ReadFromBytesAsync<T>(byte[] bytes);
        Task<T> ReadFromBytesAsync<T>(byte[] bytes, CompressionType compressionType);

        byte[] GetBytes(object value);
        byte[] GetBytes(object value, CompressionType compressionType);
        Task<byte[]> GetBytesAsync(object value);
        Task<byte[]> GetBytesAsync(object value, CompressionType compressionType);
    }
    */
}