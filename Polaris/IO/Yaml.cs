//  This file is part of Polaris-IO - An IO wrapper for Unity.
//  https://github.com/dynamiquel/Polaris-IO
//  Copyright (c) 2020 dynamiquel and contributors

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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polaris.IO
{
    public static class Yaml
    {
        /// <summary>
        /// If the Yaml Default Extension is enabled and the file location doesn't currently have an extension, give it the Default Yaml Extension.
        /// </summary>
        /// <param name="fileLocation">The file location to check and alter.</param>
        private static void CheckExtension(ref string fileLocation)
        {
            if (!Path.HasExtension(fileLocation) && (Settings.UseDefaultExtensions & FileType.Yaml) == FileType.Yaml)
                fileLocation = Path.ChangeExtension(fileLocation, Settings.DefaultYamlExtension);
        }
        
        /// <summary>
        /// Creates a file at the given file location and writes the given object to it as YAML. By default, only properties can be serialised on custom objects.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="obj">The object to write.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static bool Write(string fileLocation, object obj)
        {
            return Write(fileLocation, obj, new PascalCaseNamingConvention());
        }

        /// <summary>
        /// Creates a file at the given file location and writes the given object to it as YAML. By default, only properties can be serialised on custom objects.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="obj">The object to write.</param>
        /// <param name="namingConvention">The naming convention to use when serialising the object as YAML.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static bool Write(string fileLocation, object obj, INamingConvention namingConvention)
        {
            CheckExtension(ref fileLocation);

            var serialiser = new SerializerBuilder()
                .WithNamingConvention(namingConvention)
                .EmitDefaults()
                .Build();
            
            var yamlString = serialiser.Serialize(obj);

            return Text.Write(fileLocation, yamlString);
        }

        /// <summary>
        /// Attempts to create a file at the given file location and writes the given object to it as YAML. Catches all exceptions.
        /// </summary>
        /// <param name="fileLocation">The location of the file to write.</param>
        /// <param name="obj">The object to write.</param>
        /// <param name="namingConvention">The naming convention to use when serialising the object as YAML.</param>
        /// <returns>True if the file was written successfully.</returns>
        public static bool TryWrite(string fileLocation, object obj, INamingConvention namingConvention)
        {
            try
            {
                Write(fileLocation, obj, namingConvention);
                return true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Error whiles writing to '{fileLocation}'. Details: {e}.");
                return false;
            }
        }
        
        /// <summary>
        /// Creates a file at the given file location and writes the given object to it as YAML. By default, only properties can be serialised on custom objects.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="obj">The object to write.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static async Task<bool> WriteAsync(string fileLocation, object obj)
        {
            return await WriteAsync(fileLocation, obj, new PascalCaseNamingConvention());
        }

        /// <summary>
        /// Creates a file at the given file location and writes the given object to it as YAML. By default, only properties can be serialised on custom objects.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="obj">The object to write.</param>
        /// <param name="namingConvention">The naming convention to use when serialising the object as YAML.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static async Task<bool> WriteAsync(string fileLocation, object obj, INamingConvention namingConvention)
        {
            CheckExtension(ref fileLocation);
                
            var serialiser = new SerializerBuilder()
                .WithNamingConvention(namingConvention)
                .EmitDefaults()
                .Build();
            
            var yamlString = serialiser.Serialize(obj);

            return await Text.WriteAsync(fileLocation, yamlString);
        }

        /// <summary>
        /// Opens the file at the given location, reads all the text as YAML, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <returns>The object within the YAML file or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static T Read<T>(string fileLocation)
        {
            return Read<T>(fileLocation, new PascalCaseNamingConvention());
        }
        
        /// <summary>
        /// Opens the file at the given location, reads all the text as YAML, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <param name="namingConvention">The naming convention to use when deserialising the object as YAML. Must match the naming convention used to serialise.</param>
        /// <returns>The object within the YAML file or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static T Read<T>(string fileLocation, INamingConvention namingConvention)
        {
            CheckExtension(ref fileLocation);
            
            string yamlString = Text.Read(fileLocation);
            var obj = ReadString<T>(yamlString, namingConvention, true);

            return obj;
        }

        /// <summary>
        /// Attempts to open the file at the given location, reads all the text as YAML, converts it to the given object type and returns it. Catches all exceptions.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <param name="namingConvention">The naming convention to use when deserialising the object as YAML. Must match the naming convention used to serialise.</param>
        /// <param name="obj">The object retrieved from the YAML file.</param>
        /// <returns>True if the file was read successfully.</returns>
        public static bool TryRead<T>(string fileLocation, INamingConvention namingConvention, out T obj)
        {
            obj = default;
            
            try
            {
                obj = Read<T>(fileLocation, namingConvention);
                return true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Error whiles reading from '{fileLocation}'. Details: {e}.");
                return false;
            }
        }

        /// <summary>
        /// Opens the file at the given location, reads all the text as YAML, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <returns>The object within the YAML file or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static async Task<T> ReadAsync<T>(string fileLocation)
        {
            return await ReadAsync<T>(fileLocation, new PascalCaseNamingConvention());
        }
        
        /// <summary>
        /// Opens the file at the given location, reads all the text as YAML, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <param name="namingConvention">The naming convention to use when deserialising the object as YAML. Must match the naming convention used to serialise.</param>
        /// <returns>The object within the YAML file or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static async Task<T> ReadAsync<T>(string fileLocation, INamingConvention namingConvention)
        {
            CheckExtension(ref fileLocation);

            string yamlString = await Text.ReadAsync(fileLocation);
            var obj = ReadString<T>(yamlString, namingConvention, true);
            
            return obj;
        }

        /// <summary>
        /// Reads the given string as YAML, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="yamlString">The string to convert to YAML.</param>
        /// <param name="namingConvention">The naming convention to use when deserialising the object as YAML. Must match the naming convention used to serialise.</param>
        /// <param name="useJsonSchema">Enabling may fix deserialisation issues where everything is converted to a string instead of the expected data type.</param>
        /// <returns>The object within the string or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static T ReadString<T>(string yamlString, INamingConvention namingConvention, bool useJsonSchema)
        {
            T obj = default;

            var deserialiser = CreateDeserialiser(namingConvention, useJsonSchema);
            obj = deserialiser.Deserialize<T>(yamlString);

            return obj;
        }

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
        public class ScalarNodeTypeResolver : INodeTypeResolver
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