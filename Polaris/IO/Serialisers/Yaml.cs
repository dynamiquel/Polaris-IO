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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Polaris.IO.Compression;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polaris.IO
{
    public static class Yaml /* : IHumanSerialiser, IStringSerialiser */
    {
        private static INamingConvention GetNamingConvention(NamingConvention namingConvention)
        {
            switch (namingConvention)
            {
                case NamingConvention.None:
                    return new NullNamingConvention();
                case NamingConvention.PascalCase:
                    return new PascalCaseNamingConvention();
                case NamingConvention.CamelCase:
                    return new CamelCaseNamingConvention();
                case NamingConvention.KebabCase:
                    return new HyphenatedNamingConvention();
                case NamingConvention.SnakeCase:
                    return new UnderscoredNamingConvention();
                default:
                    return new NullNamingConvention();
            }
        }
        
        
        #region Write

        /// <summary>
        /// Creates a new file, converts the value to YAML, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to YAML.</param>
        public static void Write(string fileLocation, object value) =>
            Write(fileLocation, value, NamingConvention.None, CompressionType.None);

        /// <summary>
        /// Creates a new file, converts the value to YAML, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to YAML.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static void Write(string fileLocation, object value, CompressionType compressionType) =>
            Write(fileLocation, value, NamingConvention.None, compressionType);

        /// <summary>
        /// Creates a new file, converts the value to YAML, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to YAML.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        public static void Write(string fileLocation, object value, NamingConvention namingConvention) =>
            Write(fileLocation, value, namingConvention, CompressionType.None);

        /// <summary>
        /// Creates a new file, converts the value to YAML, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to YAML.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static void Write(string fileLocation, object value, NamingConvention namingConvention, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Yaml);
            
            // Fallback.
            var yamlBytes = GetBytes(value, namingConvention);
            Text.Write(fileLocation, yamlBytes, compressionType);
        }

        /// <summary>
        /// Creates a new file, converts the value to YAML, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to YAML.</param>
        public static Task WriteAsync(string fileLocation, object value) =>
            WriteAsync(fileLocation, value, NamingConvention.None);

        /// <summary>
        /// Creates a new file, converts the value to YAML, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to YAML.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static Task WriteAsync(string fileLocation, object value, CompressionType compressionType) =>
            WriteAsync(fileLocation, value, NamingConvention.None, compressionType);

        /// <summary>
        /// Creates a new file, converts the value to YAML, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to YAML.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        public static Task WriteAsync(string fileLocation, object value, NamingConvention namingConvention) =>
            WriteAsync(fileLocation, value, namingConvention, CompressionType.None);

        /// <summary>
        /// Creates a new file, converts the value to YAML, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to convert to YAML.</param>
        /// <param name="namingConvention">The naming convention to write in.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static async Task WriteAsync(string fileLocation, object value, NamingConvention namingConvention, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Yaml);
            
            // Fallback.
            var yamlBytes = await GetBytesAsync(value, namingConvention).ConfigureAwait(false);
            await Text.WriteAsync(fileLocation, yamlBytes, compressionType).ConfigureAwait(false);
        }
        
        #endregion


        #region Read

        /// <summary>
        /// Opens a YAML text file, parses the text in the file into a single object specified by a
        /// generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T Read<T>(string fileLocation) =>
            Read<T>(fileLocation, NamingConvention.None);

        /// <summary>
        /// Opens a YAML text file, parses the text in the file into a single object specified by a
        /// generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="namingConvention">The naming convention to use.
        /// Must match the naming convention used to write.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T Read<T>(string fileLocation, NamingConvention namingConvention) =>
            Read<T>(fileLocation, namingConvention, CompressionType.None);

        /// <summary>
        /// Opens a YAML text file, parses the text in the file into a single object specified by a
        /// generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T Read<T>(string fileLocation, CompressionType compressionType) =>
            Read<T>(fileLocation, NamingConvention.None, compressionType);
        
        /// <summary>
        /// Opens a YAML text file, parses the text in the file into a single object specified by a
        /// generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="namingConvention">The naming convention to use.
        /// Must match the naming convention used to write.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T Read<T>(string fileLocation, NamingConvention namingConvention, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Yaml);

            // Fallback.
            var yamlBytes = Text.ReadAsBytes(fileLocation, compressionType);
            return ReadFromBytes<T>(yamlBytes, namingConvention);
        }

        /// <summary>
        /// Opens a YAML text file, parses the text in the file into a single object specified by a
        /// generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static Task<T> ReadAsync<T>(string fileLocation) =>
            ReadAsync<T>(fileLocation, NamingConvention.None);

        /// <summary>
        /// Opens a YAML text file, parses the text in the file into a single object specified by a
        /// generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="namingConvention">The naming convention to use.
        /// Must match the naming convention used to write.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static Task<T> ReadAsync<T>(string fileLocation, NamingConvention namingConvention) =>
            ReadAsync<T>(fileLocation, namingConvention, CompressionType.None);

        /// <summary>
        /// Opens a YAML text file, parses the text in the file into a single object specified by a
        /// generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static Task<T> ReadAsync<T>(string fileLocation, CompressionType compressionType) =>
            ReadAsync<T>(fileLocation, NamingConvention.None, compressionType);
        
        /// <summary>
        /// Opens a YAML text file, parses the text in the file into a single object specified by a
        /// generic type parameter, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="namingConvention">The naming convention to use.
        /// Must match the naming convention used to write.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static async Task<T> ReadAsync<T>(string fileLocation, NamingConvention namingConvention, CompressionType compressionType)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Yaml);
            
            // Fallback.
            var yamlBytes = await Text.ReadAsBytesAsync(fileLocation, compressionType).ConfigureAwait(false);
            return await ReadFromBytesAsync<T>(yamlBytes, namingConvention).ConfigureAwait(false);
        }

        #endregion


        #region Try

        /// <summary>
        /// Attempts to open a YAML text file,
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
        /// Attempts to open a YAML text file,
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
        /// Attempts to create a new file, convert the value to YAML, write the contents to the file, and then close the file.
        /// If the target file already exists, it is overwritten.
        /// Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to parse to YAML.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool TryWrite(string fileLocation, object value) =>
            TryWrite(fileLocation, value, CompressionType.None);

        /// <summary>
        /// Attempts to create a new file, convert the value to YAML, write the contents to the file, and then close the file.
        /// If the target file already exists, it is overwritten.
        /// Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="value">The object to parse to YAML.</param>
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
        /// Parses the UTF-8 encoded bytes representing a YAML value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T ReadFromBytes<T>(byte[] bytes) =>
            ReadFromBytes<T>(bytes, NamingConvention.None);
        
        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a YAML value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="namingConvention">The naming convention to use.
        /// Must match the naming convention used to write.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T ReadFromBytes<T>(byte[] bytes, NamingConvention namingConvention)
        {
            var deserialiser = CreateDeserialiser(GetNamingConvention(namingConvention), true);
            
            return deserialiser.Deserialize<T>(Encoding.UTF8.GetString(bytes));
        }

        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a YAML value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T ReadFromBytes<T>(byte[] bytes, CompressionType compressionType) =>
            ReadFromBytes<T>(bytes, NamingConvention.None, compressionType);
        
        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a YAML value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <param name="namingConvention">The naming convention to use.
        /// Must match the naming convention used to write.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static T ReadFromBytes<T>(byte[] bytes, NamingConvention namingConvention, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return ReadFromBytes<T>(bytes, namingConvention);

            var decompressedBytes = Utility.DecompressHelper(bytes, compressionType);
            
            return ReadFromBytes<T>(decompressedBytes, namingConvention);
        }

        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a YAML value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static Task<T> ReadFromBytesAsync<T>(byte[] bytes) =>
            ReadFromBytesAsync<T>(bytes, NamingConvention.None);
        
        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a YAML value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="namingConvention">The naming convention to use.
        /// Must match the naming convention used to write.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static async Task<T> ReadFromBytesAsync<T>(byte[] bytes, NamingConvention namingConvention)
        {
            return await Task.Run(() => ReadFromBytes<T>(bytes, namingConvention)).ConfigureAwait(false);
        }

        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a YAML value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static Task<T> ReadFromBytesAsync<T>(byte[] bytes, CompressionType compressionType) =>
            ReadFromBytesAsync<T>(bytes, NamingConvention.None, compressionType);
        
        /// <summary>
        /// Parses the UTF-8 encoded bytes representing a YAML value into an instance of the type specified by
        /// a generic type parameter.
        /// </summary>
        /// <param name="bytes">The bytes to parse.</param>
        /// <param name="compressionType">The type of decompression to use.
        /// Must match the type of compression used to write the file.</param>
        /// <param name="namingConvention">The naming convention to use.
        /// Must match the naming convention used to write.</param>
        /// <typeparam name="T">The type of object to create an instance of.</typeparam>
        /// <returns>An instance of the specified generic type parameter.</returns>
        public static async Task<T> ReadFromBytesAsync<T>(byte[] bytes, NamingConvention namingConvention, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return await ReadFromBytesAsync<T>(bytes).ConfigureAwait(false);

            var decompressedBytes = await Utility.DecompressHelperAsync(bytes, compressionType).ConfigureAwait(false);
            
            return await ReadFromBytesAsync<T>(decompressedBytes, namingConvention).ConfigureAwait(false);
        }
        
        #endregion


        #region Get Bytes

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a YAML-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to YAML.</param>
        /// <returns>A YAML-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static byte[] GetBytes(object value) => 
            GetBytes(value, NamingConvention.None);

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a YAML-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to YAML.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>A YAML-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static byte[] GetBytes(object value, CompressionType compressionType) =>
            GetBytes(value, NamingConvention.None, compressionType);

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a YAML-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to YAML.</param>
        /// <param name="namingConvention">The naming convention to use.</param>
        /// <returns>A YAML-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static byte[] GetBytes(object value, NamingConvention namingConvention)
        {
            var serialiser = new SerializerBuilder()
                .WithNamingConvention(GetNamingConvention(namingConvention))
                .EmitDefaults()
                .Build();

            return Encoding.UTF8.GetBytes(serialiser.Serialize(value));
        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a YAML-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to YAML.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <param name="namingConvention">The naming convention to use.</param>
        /// <returns>A YAML-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static byte[] GetBytes(object value, NamingConvention namingConvention, CompressionType compressionType)
        {
            var bytes = GetBytes(namingConvention);

            return compressionType == CompressionType.None ? bytes : Utility.CompressHelper(bytes, compressionType);
        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a YAML-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to YAML.</param>
        /// <returns>A YAML-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static Task<byte[]> GetBytesAsync(object value) =>
            GetBytesAsync(value, NamingConvention.None);

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a YAML-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to YAML.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>A YAML-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static Task<byte[]> GetBytesAsync(object value, CompressionType compressionType) =>
            GetBytesAsync(value, NamingConvention.None, compressionType);
        
        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a YAML-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to YAML.</param>
        /// <param name="namingConvention">The naming convention to use.</param>
        /// <returns>A YAML-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static Task<byte[]> GetBytesAsync(object value, NamingConvention namingConvention)
        {
            return Task.Run(() => GetBytes(value, namingConvention));
        }

        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a YAML-formatted array of UTF-8
        /// encoded bytes.
        /// </summary>
        /// <param name="value">The object to parse to YAML.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <param name="namingConvention">The naming convention to use.</param>
        /// <returns>A YAML-formatted UTF-8 encoded array of bytes, parsed from the given object.</returns>
        public static async Task<byte[]> GetBytesAsync(object value, NamingConvention namingConvention, CompressionType compressionType)
        {
            var bytes = await GetBytesAsync(namingConvention).ConfigureAwait(false);

            return compressionType == CompressionType.None
                ? bytes
                : await Utility.CompressHelperAsync(bytes, compressionType).ConfigureAwait(false);
        }
        
        #endregion


        private static Deserializer CreateDeserialiser(INamingConvention namingConvention, bool jsonSchema)
        {
            Deserializer deserialiser;

            if (jsonSchema)
            {
                deserialiser = new DeserializerBuilder()
                    .WithNamingConvention(namingConvention)
                    .WithNodeTypeResolver(new ScalarNodeTypeResolver())
                    .Build();
            }
            else
            {
                deserialiser = new DeserializerBuilder()
                    .WithNamingConvention(namingConvention)
                    .Build();
            }

            return deserialiser;
        }
        
        // This node type resolver makes it easier for the YAML deserialiser to deserialise primitive types correctly,
        // instead of deserialising everything to a string.
        // Seems to only be required for dynamic dictionaries.
        private class ScalarNodeTypeResolver : INodeTypeResolver
        {
            bool INodeTypeResolver.Resolve(NodeEvent nodeEvent, ref Type currentType)
            {
                if (currentType != typeof(object))
                    return false;
                
                var scalar = nodeEvent as Scalar;
                if (scalar != null)
                {
                    // Expressions taken from https://github.com/aaubry/YamlDotNet/blob/feat-schemas/YamlDotNet/Core/Schemas/JsonSchema.cs

                    // Check if boolean.
                    if (Regex.IsMatch(scalar.Value, @"^(true|false)$", RegexOptions.IgnorePatternWhitespace))
                    {
                        currentType = typeof(bool);
                        return true;
                    }

                    // Check if int or long.
                    if (Regex.IsMatch(scalar.Value, @"^-? ( 0 | [1-9] [0-9]* )$", RegexOptions.IgnorePatternWhitespace))
                    {
                        if (int.TryParse(scalar.Value, out var i))
                        {
                            currentType = typeof(int);
                            return true;
                        }
                        
                        if (long.TryParse(scalar.Value, out var l))
                        {
                            currentType = typeof(long);
                            return true;
                        }
                    }

                    // Check if floating point number.
                    if (Regex.IsMatch(scalar.Value, @"^-? ( 0 | [1-9] [0-9]* ) ( \. [0-9]* )? ( [eE] [-+]? [0-9]+ )?$", RegexOptions.IgnorePatternWhitespace))
                    {
                        // Check if the range and precision is float-like.
                        if (Regex.IsMatch(scalar.Value, @"^-? ( 0 | [1-9] [0-9]* ) ( \. [0-9]{0,7} )? ( [eE] [-+]? [0-9]+ )?$", RegexOptions.IgnorePatternWhitespace) 
                            && float.TryParse(scalar.Value, out var s))
                        {
                            currentType = typeof(float);
                            return true;
                        }

                        // If the number is in the range of a double (don't care about precision).
                        if (double.TryParse(scalar.Value, out var d))
                        {
                            currentType = typeof(double);
                            return true;
                        }
                    }

                    // Add more cases here if needed
                }
                
                return false;
            }
        }
    }
}