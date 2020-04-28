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
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Polaris.IO
{
    public static class Binary
    {
        /// <summary>
        /// If the Binary Default Extension is enabled and the file location doesn't currently have an extension, give it the Default Binary Extension.
        /// </summary>
        /// <param name="fileLocation">The file location to check and alter.</param>
        private static void CheckExtension(ref string fileLocation)
        {
            if (!Path.HasExtension(fileLocation) && (Settings.UseDefaultExtensions & FileType.Binary) == FileType.Binary)
                fileLocation = Path.ChangeExtension(fileLocation, Settings.DefaultBinaryExtension);
        }
        
        /// <summary>
        /// Creates a file at the given file location and writes the given object to it as binary using <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="obj">The object to write.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static bool Write(string fileLocation, object obj)
        {
            string originalLocation = fileLocation;
            CheckExtension(ref fileLocation);

            FileStream fs = null;

            try
            {
                if (Settings.EnableFileRecovery)
                    fileLocation = Utility.GetTemporaryPath(fileLocation);
                
                fs = new FileStream(fileLocation, FileMode.Create);

                var bf = new BinaryFormatter();
                bf.Serialize(fs, obj);

                // If recovery is enabled, move the fileLocation.incomplete file to fileLocation.
                if (Settings.EnableFileRecovery)
                {
                    // Closes the file early so we can move it.
                    fs.Close();

                    return File.ReplaceOldWithNew(originalLocation);
                }

                return true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Error whiles writing to '{fileLocation}'. Details: {e}.");
                throw;
            }
            finally
            {
                fs?.Close();
            }

            return false;
        }

        /// <summary>
        /// Attempts to create a file at the given file location and writes the given object to it as binary using <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>. Catches all exceptions.
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
        /// Creates a file at the given file location and writes the given object to it as binary using <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="obj">The object to write.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static async Task<bool> WriteAsync(string fileLocation, object obj)
        {
            CheckExtension(ref fileLocation);

            return await Task.Run(() => Write(fileLocation, obj));
        }

        /// <summary>
        /// Opens the file at the given location, reads all the text as binary using <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <returns>The object within the binary file or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static T Read<T>(string fileLocation)
        {
            CheckExtension(ref fileLocation);
            
            if (!File.Exists(fileLocation))
            {
                UnityEngine.Debug.Log($"The file '{fileLocation}' doesn't exist.");
                return default(T);
            }

            Stream stream = null;
            
            try
            {
                T obj;

                // Maybe a better way to do it.
                #if UNITY_WSA
                byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(fileLocation);
                stream = new System.IO.MemoryStream(bytes);
                #else
                stream = new FileStream(fileLocation, FileMode.Open);
                #endif
                
                var bf = new BinaryFormatter();
                obj = (T)bf.Deserialize(stream);
                
                return obj;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Error whiles reading from '{fileLocation}'. Details: {e}.");
                throw;
            }
            finally
            {
                stream?.Close();
            }
        }

        /// <summary>
        /// Attempts to open the file at the given location, reads all the text as binary using <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>, converts it to the given object type and returns it. Catches all exceptions.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <param name="obj">The object retrieved from the binary file.</param>
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
        /// Opens the file at the given location, reads all the text as binary using <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/>, converts it to the given object type and returns it.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <returns>The object within the binary file or if deserialisation was unsuccessful, a default object of the given type.</returns>
        public static async Task<T> ReadAsync<T>(string fileLocation)
        {
            CheckExtension(ref fileLocation);
            
            return await Task.Run(() => Read<T>(fileLocation));
        }
    }
}