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
using System.Runtime.Serialization.Formatters.Binary;
using Polaris.IO.Compression;
using System.Threading.Tasks;

namespace Polaris.IO
{
    public static class Binary
    {
        #region Write

        public static void Write(string fileLocation, object value) =>
            Write(fileLocation, value, CompressionType.None);

        public static void Write(string fileLocation, object value, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Binary);

            using (var stream = GetStream(value))
            {
                Text.Write(fileLocation, stream, compressionType);
            }
        }

        public static Task WriteAsync(string fileLocation, object value) =>
            WriteAsync(fileLocation, value, CompressionType.None);

        public static async Task WriteAsync(string fileLocation, object value, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Binary);

            using (var stream = await GetStreamAsync(value).ConfigureAwait(false))
            {
                await Text.WriteAsync(fileLocation, stream, compressionType).ConfigureAwait(false);
            }
        }

        #endregion


        #region Read

        public static T Read<T>(string fileLocation) => 
            Read<T>(fileLocation, CompressionType.None);

        public static T Read<T>(string fileLocation, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Binary);

            var bytes = Text.ReadAsBytes(fileLocation, compressionType);
            return FromBytes<T>(bytes);
        }

        public static Task<T> ReadAsync<T>(string fileLocation) => 
            ReadAsync<T>(fileLocation, CompressionType.None);

        public static async Task<T> ReadAsync<T>(string fileLocation, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Binary);

            var bytes = await Text.ReadAsBytesAsync(fileLocation, compressionType).ConfigureAwait(false);
            return await FromBytesAsync<T>(bytes).ConfigureAwait(false);
        }
        
        
        #endregion


        #region Try

        public static bool TryRead<T>(string fileLocation, out T result) =>
            TryRead(fileLocation, CompressionType.None, out result);

        public static bool TryRead<T>(string fileLocation, CompressionType compressionType, out T result)
        {
            try
            {
                result = Read<T>(fileLocation, compressionType);
                return true;
            }
            catch (Exception e)
            {
                result = default;
                return false;
            }
        }

        public static bool TryWrite(string fileLocation, object value) =>
            TryWrite(fileLocation, value, CompressionType.None);

        public static bool TryWrite(string fileLocation, object value, CompressionType compressionType)
        {
            try
            {
                Write(fileLocation, value, compressionType);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        #endregion


        #region Read Bytes

        public static T FromBytes<T>(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var bf = new BinaryFormatter();
                return (T)bf.Deserialize(stream);
            }
        }

        public static T FromBytes<T>(byte[] bytes, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return FromBytes<T>(bytes);
            
            var decompressedBytes = Utility.DecompressHelper(bytes, compressionType);
            return FromBytes<T>(decompressedBytes);
        }

        // TODO: Make deserialisation async.
        public static async Task<T> FromBytesAsync<T>(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var bf = new BinaryFormatter();
                return (T)bf.Deserialize(stream);
            }
        }

        // TODO: Make deserialisation async.
        public static async Task<T> FromBytesAsync<T>(byte[] bytes, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return await FromBytesAsync<T>(bytes).ConfigureAwait(false);
            
            var decompressedBytes = await Utility.DecompressHelperAsync(bytes, compressionType);
            return await FromBytesAsync<T>(decompressedBytes).ConfigureAwait(false);
        }
        
        #endregion


        #region Get Bytes

        public static byte[] GetBytes(object value) =>
            GetStream(value).ToArray();

        public static byte[] GetBytes(object value, CompressionType compressionType) =>
            GetStream(value, compressionType).ToArray();

        public static async Task<byte[]> GetBytesAsync(object value) =>
            (await GetStreamAsync(value).ConfigureAwait(false)).ToArray();

        public static async Task<byte[]> GetBytesAsync(object value, CompressionType compressionType) =>
            (await GetStreamAsync(value, compressionType).ConfigureAwait(false)).ToArray();

        #endregion


        #region Get Stream

        public static MemoryStream GetStream(object value)
        {
            var stream = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(stream, value);

            return stream;
        }

        public static MemoryStream GetStream(object value, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return GetStream(value);

            var decompressedStream = new MemoryStream();
            using (var stream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(stream, value);

                Task.Run(() => Compressor.Compress(stream, decompressedStream, compressionType))
                    .GetAwaiter().GetResult();
            }

            return decompressedStream;
        }
        
        // TODO: Make serialisation async.
        public static async Task<MemoryStream> GetStreamAsync(object value)
        {
            var stream = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(stream, value);

            return stream;
        }

        // TODO: Make serialisation async.
        public static async Task<MemoryStream> GetStreamAsync(object value, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return await GetStreamAsync(value).ConfigureAwait(false);

            var decompressedStream = new MemoryStream();
            using (var stream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(stream, value);

                await Compressor.Compress(stream, decompressedStream, compressionType).ConfigureAwait(false);
            }

            return decompressedStream;
        }

        #endregion
    }
}