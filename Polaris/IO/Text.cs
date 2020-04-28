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
using System.Text;
using System.Threading.Tasks;

namespace Polaris.IO
{
    public static class Text
    {
        /// <summary>
        /// If the Text Default Extension is enabled and the file location doesn't currently have an extension, give it the Default Text Extension.
        /// </summary>
        /// <param name="fileLocation">The file location to check and alter.</param>
        private static void CheckExtension(ref string fileLocation)
        {
            if (!Path.HasExtension(fileLocation) && (Settings.UseDefaultExtensions & FileType.Text) == FileType.Text)
                fileLocation = Path.ChangeExtension(fileLocation, Settings.DefaultTextExtension);
        }
        
        /// <summary>
        /// Creates a file at the given file location and writes the given string to it.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="content">The string to write to the file.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static bool Write(string fileLocation, string content)
        {
            CheckExtension(ref fileLocation);
            
            // If recovery is enabled, store the file as fileLocation.incomplete.
            if (Settings.EnableFileRecovery)
                fileLocation = Utility.GetTemporaryPath(fileLocation);

            #if UNITY_WSA
            UnityEngine.Windows.File.WriteAllBytes(fileLocation, Encoding.UTF8.GetBytes(content));
            #else
            System.IO.File.WriteAllText(fileLocation, content);
            #endif
            
            // If recovery is enabled, move the fileLocation.incomplete file to fileLocation.
            if (Settings.EnableFileRecovery)
                return File.ReplaceOldWithNew(Utility.GetTemporaryPath(fileLocation, true));
            
            return true;
        }

        /// <summary>
        /// Attempts to create a file at the given file location and writes the given string to it. Catches all exceptions.
        /// </summary>
        /// <param name="fileLocation">The location of the file to write.</param>
        /// <param name="content">The string to write to the file.</param>
        /// <returns>True if the file was written successfully.</returns>
        public static bool TryWrite(string fileLocation, string content)
        {
            try
            {
                Write(fileLocation, content);
                return true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Error whiles writing to '{fileLocation}'. Details: {e}.");
                return false;
            }
        }

        /// <summary>
        /// Creates a file at the given file location and writes the given string to it.
        /// </summary>
        /// <param name="fileLocation">The location where to create the file.</param>
        /// <param name="content">The string to write to the file.</param>
        /// <returns>True if the file was successfully written.</returns>
        public static async Task<bool> WriteAsync(string fileLocation, string content)
        {
            CheckExtension(ref fileLocation);
                
            try
            {
                if (Settings.EnableFileRecovery)
                    fileLocation = Utility.GetTemporaryPath(fileLocation);

                // Try to create folders if don't exist.
                #if UNITY_WSA
                var folder = await GetFolderFromPathAsync(fileLocation);
                var file = await folder.CreateFileAsync(fileLocation, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(file, content);
                #else
                using (var sw = new StreamWriter(fileLocation))
                {
                    await sw.WriteAsync(content);
                }
                #endif

                if (Settings.EnableFileRecovery)
                    return File.ReplaceOldWithNew(Utility.GetTemporaryPath(fileLocation, true));

                return true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Error whiles writing to '{fileLocation}'. Details: {e}.");
                throw;
            }

            return false;
        }

        /// <summary>
        /// Opens a text file at the given location, reads all the text and returns it as a string.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <returns>The contents of the file as a string.</returns>
        public static string Read(string fileLocation)
        {
            CheckExtension(ref fileLocation);
            
            var text = string.Empty;

            if (!File.Exists(fileLocation))
            {
                UnityEngine.Debug.Log($"The file '{fileLocation} doesn't exist.");
                return text;
            }
            
            #if UNITY_WSA
            var bytes = UnityEngine.Windows.File.ReadAllBytes(fileLocation);
            text = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            #else
            text = System.IO.File.ReadAllText(fileLocation);
            #endif

            return text;
        }

        /// <summary>
        /// Attempts to open a text file at the given location, reads all the text and returns it as a string. Catches all exceptions.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <param name="content">The string retrieved from the text file.</param>
        /// <returns>True if the file was read successfully.</returns>
        public static bool TryRead(string fileLocation, out string content)
        {
            content = string.Empty;
            
            try
            {
                content = Read(fileLocation);
                return true;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log($"Error whiles reading from '{fileLocation}'. Details: {e}.");
                return false;
            }
        }

        /// <summary>
        /// Opens a text file at the given location, reads all the text and returns it as a string.
        /// </summary>
        /// <param name="fileLocation">The location of the file to read.</param>
        /// <returns>The contents of the file as a string.</returns>
        public static async Task<string> ReadAsync(string fileLocation)
        {
            CheckExtension(ref fileLocation);
            
            string text = string.Empty;

            // Not tested.
            #if UNITY_WSA
            var file = await GetFileFromPathAsync(fileLocation);

            if (file.Exists)
                text = await Windows.Storage.FileIO.ReadTextAsync(file);
            else
                UnityEngine.Debug.Log($"The file '{fileLocation} doesn't exist.");
            #else
            if (File.Exists(fileLocation))
                using (var sr = new StreamReader(fileLocation))
                {
                    text = await sr.ReadToEndAsync();
                }
            else
                UnityEngine.Debug.Log($"The file '{fileLocation} doesn't exist.");
            #endif

            return text;
        }

        public static bool Append(string fileLocation, string content)
        {
            return false;
        }
    }
}