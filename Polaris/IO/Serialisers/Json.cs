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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polaris.IO.Compression;

namespace Polaris.IO
{
    public static class Json /* : IHumanSerialiser, IStringSerialiser */
    {
        private static IContractResolver GetNamingConvention(NamingConvention namingConvention)
        {
            switch (namingConvention)
            {
                case NamingConvention.None:
                    return new DefaultContractResolver();
                case NamingConvention.PascalCase:
                    return new DefaultContractResolver();
                case NamingConvention.CamelCase:
                    return new CamelCasePropertyNamesContractResolver();
                case NamingConvention.KebabCase:
                    return new DefaultContractResolver();
                case NamingConvention.SnakeCase:
                    return new DefaultContractResolver();
                default:
                    return new DefaultContractResolver();
            }
        }
        
        
        #region Write

        /// <summary>
        /// Creates a new file, converts the value to JSON, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to JSON.</param>
        public static void Write(string fileLocation, object value) =>
            Write(fileLocation, value, NamingConvention.None, CompressionType.None);

        /// <summary>
        /// Creates a new file, converts the value to JSON, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to JSON.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static void Write(string fileLocation, object value, CompressionType compressionType) =>
            Write(fileLocation, value, NamingConvention.None, compressionType);

        /// <summary>
        /// Creates a new file, converts the value to JSON, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to JSON.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        public static void Write(string fileLocation, object value, NamingConvention namingConvention) =>
            Write(fileLocation, value, namingConvention, CompressionType.None);

        /// <summary>
        /// Creates a new file, converts the value to JSON, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to JSON.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static void Write(string fileLocation, object value, NamingConvention namingConvention, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Json);

            // Fallback.
            var jsonBytes = GetBytes(value, namingConvention);
            Text.Write(fileLocation, jsonBytes, compressionType);
        }

        /// <summary>
        /// Creates a new file, converts the value to JSON, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to JSON.</param>
        public static Task WriteAsync(string fileLocation, object value) =>
            WriteAsync(fileLocation, value, NamingConvention.None, CompressionType.None);

        /// <summary>
        /// Creates a new file, converts the value to JSON, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to JSON.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static Task WriteAsync(string fileLocation, object value, CompressionType compressionType) =>
            WriteAsync(fileLocation, value, NamingConvention.None, compressionType);

        /// <summary>
        /// Creates a new file, converts the value to JSON, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to JSON.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        public static Task WriteAsync(string fileLocation, object value, NamingConvention namingConvention) =>
            WriteAsync(fileLocation, value, namingConvention, CompressionType.None);

        /// <summary>
        /// Creates a new file, converts the value to JSON, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to JSON.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static async Task WriteAsync(string fileLocation, object value, NamingConvention namingConvention, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Json);

            // Fallback.
            var jsonBytes = await GetBytesAsync(value, namingConvention);
            await Text.WriteAsync(fileLocation, jsonBytes, compressionType).ConfigureAwait(false);
        }
        
        #endregion


        #region Read

        /// <summary>
        /// Opens a JSON text file, parses the text in the file into a single object specified by a generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T Read<T>(string fileLocation) =>
            Read<T>(fileLocation, CompressionType.None);

        /// <summary>
        /// Opens a JSON text file, parses the text in the file into a single object specified by a generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T Read<T>(string fileLocation, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Json);

            var jsonBytes = Text.ReadAsBytes(fileLocation, compressionType);
            return ReadFromBytes<T>(jsonBytes);
        }

        /// <summary>
        /// Opens a JSON text file, parses the text in the file into a single object specified by a generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static Task<T> ReadAsync<T>(string fileLocation) =>
            ReadAsync<T>(fileLocation, CompressionType.None);

        /// <summary>
        /// Opens a JSON text file, parses the text in the file into a single object specified by a generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static async Task<T> ReadAsync<T>(string fileLocation, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Json);

            // Fallback.
            var jsonBytes = await Text.ReadAsBytesAsync(fileLocation, compressionType).ConfigureAwait(false);
            return await ReadFromBytesAsync<T>(jsonBytes).ConfigureAwait(false);
        }

        #endregion


        #region Try

        /// <summary>
        /// Attempts to open a JSON text file,
        /// parse the text in the file into a single object specified by a generic type parameter,
        /// and then close the file. Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="result">The object of the specified generic type parameter parsed from JSON.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>Returns true if successful.</returns>
        public static bool TryRead<T>(string fileLocation, out T result) =>
            TryRead(fileLocation, CompressionType.None, out result);

        /// <summary>
        /// Attempts to open a JSON text file,
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
                var jsonBytes = Text.ReadAsBytes(fileLocation);
                result = ReadFromBytes<T>(jsonBytes, compressionType);
                return true;
            }
            catch (Exception e)
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Attempts to create a new file, convert the value to JSON, write the contents to the file, and then close the file.
        /// If the target file already exists, it is overwritten.
        /// Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to parse to JSON.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool TryWrite(string fileLocation, object value) =>
            TryWrite(fileLocation, value, CompressionType.None);

        /// <summary>
        /// Attempts to create a new file, convert the value to JSON, write the contents to the file, and then close the file.
        /// If the target file already exists, it is overwritten.
        /// Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to parse to JSON.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool TryWrite(string fileLocation, object value, CompressionType compressionType)
        {
            try
            {
                var jsonBytes = GetBytes(value);
                Text.Write(fileLocation, jsonBytes, compressionType);
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
        /// Returns a C# object from the JSON data in the given Stream.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T Deserialise_Internal<T>(Stream inputStream)
        {
            using (var streamReader = new StreamReader(inputStream))
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
            {
                var jsonSerialiser = new JsonSerializer();
                return jsonSerialiser.Deserialize<T>(jsonReader);
            }
        }
        
        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a JSON value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T ReadFromBytes<T>(byte[] bytes)
        {
            //return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));

            // I believe this should be more performant due to no byte -> string conversion.
            using (var stream = new MemoryStream(bytes))
            {
                return Deserialise_Internal<T>(stream);
            }
        }

        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a JSON value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T ReadFromBytes<T>(byte[] bytes, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return ReadFromBytes<T>(bytes);
            
            //return ReadFromBytes<T>(Utility.DecompressHelper(bytes, compressionType));
            
            using (var decompressedStream = new MemoryStream())
            {
                using (var compressedStream = new MemoryStream(bytes))
                {
                    Task.Run(() =>
                        Compressor.Decompress(compressedStream, decompressedStream, compressionType)).GetAwaiter().GetResult();
                }

                return Deserialise_Internal<T>(decompressedStream);
            }
        }

        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a JSON value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static async Task<T> ReadFromBytesAsync<T>(byte[] bytes)
        {
            return await Task.Run(() => ReadFromBytes<T>(bytes)).ConfigureAwait(false);
        }

        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a JSON value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static async Task<T> ReadFromBytesAsync<T>(byte[] bytes, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return await ReadFromBytesAsync<T>(bytes).ConfigureAwait(false);
            
            using (var decompressedStream = new MemoryStream())
            {
                using (var compressedStream = new MemoryStream(bytes))
                {
                    await Compressor.Decompress(compressedStream, decompressedStream, compressionType).ConfigureAwait(false);
                }

                return Deserialise_Internal<T>(decompressedStream);
            }
        }
        
        #endregion


        #region Get Bytes

        /// <summary>
        /// Writes the JSON data for the given object into the given Stream.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="value"></param>
        private static void Serialise_Internal(Stream inputStream, object value, NamingConvention namingConvention = NamingConvention.None)
        {
            using (var streamWriter = new StreamWriter(inputStream, Encoding.UTF8))
            using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
            {
                var jsonSerialiser = new JsonSerializer
                {
                    ContractResolver = GetNamingConvention(namingConvention)
                };
                
                jsonSerialiser.Serialize(jsonWriter, value);
            }
        }

        public static byte[] GetBytes(object value) =>
            GetBytes(value, NamingConvention.None);

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to JSON.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>A JSON-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static byte[] GetBytes(object value, CompressionType compressionType) =>
            GetBytes(value, NamingConvention.None, compressionType);

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to JSON.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        /// <returns>A JSON-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static byte[] GetBytes(object value, NamingConvention namingConvention)
        {
            /*var jsonString = JsonConvert.SerializeObject(value);
            return Encoding.UTF8.GetBytes(jsonString);*/
            
            using (var stream = new MemoryStream())
            {
                Serialise_Internal(stream, value, namingConvention);
                
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to JSON.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        /// <returns>A JSON-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static byte[] GetBytes(object value, NamingConvention namingConvention, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return GetBytes(value, namingConvention);
                
            using (var decompressedStream = new MemoryStream())
            {
                Serialise_Internal(decompressedStream, value, namingConvention);

                using (var compressedStream = new MemoryStream())
                {
                    Task.Run(() =>
                        Compressor.Compress(decompressedStream, compressedStream, compressionType)).GetAwaiter().GetResult();
                    
                    return compressedStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to JSON.</param>
        /// <returns>A JSON-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static Task<byte[]> GetBytesAsync(object value) =>
            GetBytesAsync(value, NamingConvention.None);

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to JSON.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>A JSON-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static Task<byte[]> GetBytesAsync(object value, CompressionType compressionType) =>
            GetBytesAsync(value, NamingConvention.None, compressionType);
        
        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to JSON.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        /// <returns>A JSON-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static async Task<byte[]> GetBytesAsync(object value, NamingConvention namingConvention)
        {
            return await Task.Run(() => GetBytes(value, namingConvention)).ConfigureAwait(false);
        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to JSON.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        /// <returns>A JSON-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static async Task<byte[]> GetBytesAsync(object value, NamingConvention namingConvention, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return await GetBytesAsync(value).ConfigureAwait(false);
            
            using (var decompressedStream = new MemoryStream())
            {
                Serialise_Internal(decompressedStream, value, namingConvention);

                using (var compressedStream = new MemoryStream())
                {
                    await Compressor.Compress(decompressedStream, compressedStream, compressionType).ConfigureAwait(false);
                    
                    return compressedStream.ToArray();
                }
            }
        }
        
        #endregion


        #region Read String
        
        // Simply converts string <-> byte[] using Encoding.UTF8. ReadFromBytes and GetBytes is preferred.

        /// <summary>
        /// Parses the string representing a JSON value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="content">The JSON-formatted string to parse and create an object from.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T ReadFromString<T>(string content) =>
            ReadFromBytes<T>(Encoding.UTF8.GetBytes(content));

        /// <summary>
        /// Parses the string representing a JSON value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="content">The JSON-formatted string to parse and create an object from.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T ReadFromString<T>(string content, CompressionType compressionType) =>
            ReadFromBytes<T>(Encoding.UTF8.GetBytes(content), compressionType);

        /// <summary>
        /// Parses the string representing a JSON value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="content">The JSON-formatted string to parse and create an object from.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static Task<T> ReadFromStringAsync<T>(string content) =>
            ReadFromBytesAsync<T>(Encoding.UTF8.GetBytes(content));

        /// <summary>
        /// Parses the string representing a JSON value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="content">The JSON-formatted string to parse and create an object from.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static Task<T> ReadFromStringAsync<T>(string content, CompressionType compressionType) =>
            ReadFromBytesAsync<T>(Encoding.UTF8.GetBytes(content), compressionType);
        
        #endregion


        #region Get String
        
        // Simply converts string <-> byte[] using Encoding.UTF8. ReadFromBytes and GetBytes is preferred.

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted string.
        /// </summary>
        /// <param name="value">The object to create a JSON-formatted string from.</param>
        /// <returns>The JSON-formatted string of the specified object.</returns>
        public static string GetString(object value) =>
            Encoding.UTF8.GetString(GetBytes(value));

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted string.
        /// </summary>
        /// <param name="value">The object to create a JSON-formatted string from.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>The JSON-formatted string of the specified object.</returns>
        public static string GetString(object value, CompressionType compressionType) =>
            Encoding.UTF8.GetString(GetBytes(value, compressionType));
        
        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted string.
        /// </summary>
        /// <param name="value">The object to create a JSON-formatted string from.</param>
        /// <returns>The JSON-formatted string of the specified object.</returns>
        public static async Task<string> GetStringAsync(object value) =>
            Encoding.UTF8.GetString(await GetBytesAsync(value).ConfigureAwait(false));
        
        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON-formatted string.
        /// </summary>
        /// <param name="value">The object to create a JSON-formatted string from.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>The JSON-formatted string of the specified object.</returns>
        public static async Task<string> GetStringAsync(object value, CompressionType compressionType) =>
            Encoding.UTF8.GetString(await GetBytesAsync(value, compressionType).ConfigureAwait(false));

        #endregion
    }
}