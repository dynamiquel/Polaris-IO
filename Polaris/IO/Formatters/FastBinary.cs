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
using System.Text;
using System.Threading.Tasks;
using Polaris.IO.Compression;
using UnityEngine;
using ZeroFormatter;
using CompressionType = Polaris.IO.Compression.CompressionType;

namespace Polaris.IO
{
    public static class FastBinary
    {
        #region Write

        /// <summary>
        /// Creates a new file, converts the value to binary, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to binary.</param>
        public static void Write<T>(string fileLocation, T value) =>
            Write(fileLocation, value, CompressionType.None);

        /// <summary>
        /// Creates a new file, converts the value to binary, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to binary.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static void Write<T>(string fileLocation, T value, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Binary);

            Text.Write(fileLocation, ZeroFormatterSerializer.Serialize(value), compressionType);
        }

        /// <summary>
        /// Creates a new file, converts the value to binary, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to binary.</param>
        public static Task WriteAsync<T>(string fileLocation, T value) =>
            WriteAsync(fileLocation, value, CompressionType.None);

        /// <summary>
        /// Creates a new file, converts the value to binary, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to binary.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static async Task WriteAsync<T>(string fileLocation, T value, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Binary);
            
            var bytes = ZeroFormatterSerializer.Serialize(value);
            await Text.WriteAsync(fileLocation, bytes, compressionType).ConfigureAwait(false);
        }

        #endregion


        #region Read

        /// <summary>
        /// Opens a binary file, parses the text in the file into a single object specified by a generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T Read<T>(string fileLocation) => 
            Read<T>(fileLocation, CompressionType.None);

        /// <summary>
        /// Opens a binary file, parses the text in the file into a single object specified by a generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T Read<T>(string fileLocation, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Binary);
            
            var bytes = Text.ReadAsBytes(fileLocation, compressionType);
            return ZeroFormatterSerializer.Deserialize<T>(bytes);
        }

        /// <summary>
        /// Opens a binary file, parses the text in the file into a single object specified by a generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static Task<T> ReadAsync<T>(string fileLocation) =>
            ReadAsync<T>(fileLocation, CompressionType.None);

        /// <summary>
        /// Opens a binary file, parses the text in the file into a single object specified by a generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static async Task<T> ReadAsync<T>(string fileLocation, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Binary);

            var bytes = await Text.ReadAsBytesAsync(fileLocation, compressionType).ConfigureAwait(false);
            return ZeroFormatterSerializer.Deserialize<T>(bytes);
        }
        
        
        #endregion


        #region Try

        /// <summary>
        /// Attempts to open a binary file,
        /// parse the text in the file into a single object specified by a generic type parameter,
        /// and then close the file. Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="result">The object of the specified generic type parameter parsed from JSON.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>Returns true if successful.</returns>
        public static bool TryRead<T>(string fileLocation, out T result) => 
            TryRead<T>(fileLocation, CompressionType.None, out result);

        /// <summary>
        /// Attempts to open a binary file,
        /// parse the text in the file into a single object specified by a generic type parameter,
        /// and then close the file. Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <param name="result">The object of the specified generic type parameter parsed from JSON.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>Returns true if successful.</returns>
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

        /// <summary>
        /// Attempts to create a new file, convert the value to binary, write the contents to the file, and then close the file.
        /// If the target file already exists, it is overwritten.
        /// Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to parse to binary.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool TryWrite(string fileLocation, object value) =>
            TryWrite(fileLocation, value, CompressionType.None);

        /// <summary>
        /// Attempts to create a new file, convert the value to binary, write the contents to the file, and then close the file.
        /// If the target file already exists, it is overwritten.
        /// Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to parse to binary.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>Returns true if successful.</returns>
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

        /// <summary>
        /// Parses the  encoded bytes representing a binary-formatted value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T FromBytes<T>(byte[] bytes)
        {
            return ZeroFormatterSerializer.Deserialize<T>(bytes);
        }
        
        /// <summary>
        /// Parses the  encoded bytes representing a binary-formatted value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T FromBytes<T>(byte[] bytes, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return FromBytes<T>(bytes);
            
            using (var decompressedStream = new MemoryStream())
            {
                using (var compressedStream = new MemoryStream(bytes))
                {
                    Task.Run(() =>
                        Compressor.Decompress(compressedStream, decompressedStream, compressionType)).GetAwaiter().GetResult();
                }

                return ZeroFormatterSerializer.Deserialize<T>(decompressedStream);
            }
        }

        /// <summary>
        /// Parses the  encoded bytes representing a binary-formatted value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static async Task<T> FromBytesAsync<T>(byte[] bytes)
        {
            return await Task.Run(() => FromBytes<T>(bytes)).ConfigureAwait(false);
        }

        /// <summary>
        /// Parses the  encoded bytes representing a binary-formatted value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static async Task<T> FromBytesAsync<T>(byte[] bytes, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return await FromBytesAsync<T>(bytes).ConfigureAwait(false);
            
            using (var decompressedStream = new MemoryStream())
            {
                using (var compressedStream = new MemoryStream(bytes))
                {
                    await Compressor.Decompress(compressedStream, decompressedStream, compressionType)
                        .ConfigureAwait(false);
                }

                return ZeroFormatterSerializer.Deserialize<T>(decompressedStream);
            }
        }
        
        #endregion


        #region Get Bytes

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a binary-formatted array of bytes.
        /// </summary>
        /// <param name="value">The object to parse to binary.</param>
        /// <returns>A binary-formatted array of bytes, parsed from the given object.</returns>
        public static byte[] GetBytes(object value)
        {
            return ZeroFormatterSerializer.Serialize(value);
        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a binary-formatted array of bytes.
        /// </summary>
        /// <param name="value">The object to parse to binary.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>A binary-formatted array of bytes, parsed from the given object.</returns>
        public static byte[] GetBytes(object value, CompressionType compressionType)
        {
            var bytes = GetBytes(value);

            if (compressionType == CompressionType.None)
                return bytes;

            using (var compressedStream = new MemoryStream())
            {
                using (var decompressedStream = new MemoryStream(bytes))
                {
                    Task.Run(() =>
                            Compressor.Compress(decompressedStream, compressedStream, compressionType)).GetAwaiter()
                        .GetResult();
                }

                return compressedStream.ToArray();
            }
        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a binary-formatted array of bytes.
        /// </summary>
        /// <param name="value">The object to parse to binary.</param>
        /// <returns>A binary-formatted array of bytes, parsed from the given object.</returns>
        public static async Task<byte[]> GetBytesAsync(object value)
        {
            return await Task.Run(() => GetBytes(value)).ConfigureAwait(false);
        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a binary-formatted array of bytes.
        /// </summary>
        /// <param name="value">The object to parse to binary.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>A binary-formatted array of bytes, parsed from the given object.</returns>
        public static async Task<byte[]> GetBytesAsync(object value, CompressionType compressionType)
        {
            var bytes = await GetBytesAsync(value).ConfigureAwait(false);

            if (compressionType == CompressionType.None)
                return bytes;

            using (var compressedStream = new MemoryStream())
            {
                using (var decompressedStream = new MemoryStream(bytes))
                {
                    await Compressor.Compress(decompressedStream, compressedStream, compressionType)
                        .ConfigureAwait(false);
                }

                return compressedStream.ToArray();
            }
        }

        #endregion
    }
}