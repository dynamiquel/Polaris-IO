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
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Polaris.IO
{
    public static class Json
    {
        /// <summary>
        /// If the Json Default Extension is enabled and the file location doesn't currently have an extension, give it the Default Json Extension.
        /// </summary>
        /// <param name="fileLocation">The file location to check and alter.</param>
        private static void CheckExtension(ref string fileLocation)
        {
            if (!Path.HasExtension(fileLocation) && (Settings.UseDefaultExtensions & FileType.Json) == FileType.Json)
                fileLocation = Path.ChangeExtension(fileLocation, Settings.DefaultJsonExtension);
        }
        
        /// <summary>
        /// Creates a file at the given file location and writes the given object to it as JSON.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="obj">The object to write.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static bool Write(string fileLocation, object obj)
        {
            CheckExtension(ref fileLocation);

            var jsonString = ToString(obj);

            return Text.Write(fileLocation, jsonString);
        }

        /// <summary>
        /// Attempts to create a file at the given file location and writes the given object to it as JSON. Catches all exceptions.
        /// </summary>
        /// <param name="fileLocation">The location of the file to write.</param>
        /// <param name="obj">The object to write.</param>
        /// <returns>True if the file was written successfully.</returns>
        public static bool TryWrite(string fileLocation, object obj)
        {
            try
            {
                Write(fileLocation, obj);
                return true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Error whiles writing to '{fileLocation}'. Details: {e}.");
                return false;
            }
        }

        /// <summary>
        /// Creates a file at the given file location and writes the given object to it as JSON.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="obj">The object to write.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static async Task<bool> WriteAsync(string fileLocation, object obj)
        {
            CheckExtension(ref fileLocation);
                
            var jsonString = ToString(obj);

            return await Text.WriteAsync(fileLocation, jsonString);
        }

        /// <summary>
        /// Creates a file at the given file location and directly writes the given object to it as JSON. May not work on all platforms.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="obj">The object to write.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static bool WriteDirect(string fileLocation, object obj)
        {
            CheckExtension(ref fileLocation);
                
            var serializer = new JsonSerializer();

            using (var sw = new StreamWriter(fileLocation))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Newtonsoft.Json.Formatting.Indented;
                serializer.Serialize(writer, obj);
            }

            return true;
        }

        /// <summary>
        /// Opens the file at the given location, reads all the text as JSON, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <returns>The object within the JSON file or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static T Read<T>(string fileLocation)
        {
            CheckExtension(ref fileLocation);
            
            string jsonString = Text.Read(fileLocation);
            var obj = FromString<T>(jsonString);

            return obj;
        }

        /// <summary>
        /// Attempts to open the file at the given location, reads all the text as JSON, converts it to the given object type and returns it. Catches all exceptions.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <param name="obj">The object retrieved from the JSON file.</param>
        /// <returns>True if the file was read successfully.</returns>
        public static bool TryRead<T>(string fileLocation, out T obj)
        {
            obj = default;
            
            try
            {
                obj = Read<T>(fileLocation);
                return true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Error whiles reading from '{fileLocation}'. Details: {e}.");
                return false;
            }
        }

        /// <summary>
        /// Opens the file at the given location, reads all the text as JSON, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <returns>The object within the JSON file or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static async Task<T> ReadAsync<T>(string fileLocation)
        {
            CheckExtension(ref fileLocation);
            
            string jsonString = await Text.ReadAsync(fileLocation);
            var obj = FromString<T>(jsonString);

            return obj;
        }

        /// <summary>
        /// Reads the given string as JSON, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="jsonString">The string to convert to JSON.</param>
        /// <returns>The object within the string or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static T FromString<T>(string jsonString)
        {
            var obj = JsonConvert.DeserializeObject<T>(jsonString);

            return obj;
        }

        /// <summary>
        /// Converts the object to a JSON-formatted string and returns it.
        /// </summary>
        /// <param name="obj">The object to convert to a JSON-formatted string.</param>
        /// <returns>The JSON-formatted string created from the object.</returns>
        public static string ToString(object obj)
        {
            var jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
            
            return jsonString;
        }
    }
}