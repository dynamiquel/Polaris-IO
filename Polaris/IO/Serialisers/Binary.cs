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
using Polaris.IO.Compression;
using System.Threading.Tasks;

namespace Polaris.IO
{
    public static class Binary
    {
        #region Write

        public static void Write(string fileLocation, object value)
        {
            throw new NotImplementedException();
        }

        public static void Write(string fileLocation, object value, CompressionType compressionType)
        {
            throw new NotImplementedException();
        }

        public static Task WriteAsync(string fileLocation, object value)
        {
            throw new NotImplementedException();
        }

        public static Task WriteAsync(string fileLocation, object value, CompressionType compressionType)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Read

        public static T Read<T>(string fileLocation)
        {
            throw new NotImplementedException();
        }

        public static T Read<T>(string fileLocation, CompressionType compressionType)
        {
            throw new NotImplementedException();
        }

        public static Task<T> ReadAsync<T>(string fileLocation)
        {
            throw new NotImplementedException();
        }

        public static Task<T> ReadAsync<T>(string fileLocation, CompressionType compressionType)
        {
            throw new NotImplementedException();
        }
        
        
        #endregion


        #region Try

        public static bool TryRead<T>(string fileLocation, out T result)
        {
            throw new NotImplementedException();
        }

        public static bool TryRead<T>(string fileLocation, CompressionType compressionType, out T result)
        {
            throw new NotImplementedException();
        }

        public static bool TryWrite(string fileLocation, object value)
        {
            throw new NotImplementedException();
        }

        public static bool TryWrite(string fileLocation, object value, CompressionType compressionType)
        {
            throw new NotImplementedException();
        }
        
        #endregion


        #region Read Bytes

        public static T ReadFromBytes<T>(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public static T ReadFromBytes<T>(byte[] bytes, CompressionType compressionType)
        {
            throw new NotImplementedException();
        }

        public static Task<T> ReadFromBytesAsync<T>(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public static Task<T> ReadFromBytesAsync<T>(byte[] bytes, CompressionType compressionType)
        {
            throw new NotImplementedException();
        }
        
        #endregion


        #region Get Bytes

        public static byte[] GetBytes(object value)
        {
            throw new NotImplementedException();
        }

        public static byte[] GetBytes(object value, CompressionType compressionType)
        {
            throw new NotImplementedException();
        }

        public static Task<byte[]> GetBytesAsync(object value)
        {
            throw new NotImplementedException();
        }

        public static Task<byte[]> GetBytesAsync(object value, CompressionType compressionType)
        {
            throw new NotImplementedException();
        }

        #endregion
        
        
        #region Direct
        
        public static void WriteDirect(string fileLocation, object value)
        {
            throw new NotImplementedException();
        }

        public static Task WriteDirectAsync(string fileLocation, object value)
        {
            throw new NotImplementedException();
        }

        public static T ReadDirect<T>(string fileLocation)
        {
            throw new NotImplementedException();
        }

        public static Task<T> ReadDirectAsync<T>(string fileLocation)
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}