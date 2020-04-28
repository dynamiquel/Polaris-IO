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

namespace Polaris.IO
{
    public static class File
    {
        /// <summary>
        /// Checks if a file exists at the given file location. [Platform-independent]
        /// </summary>
        /// <param name="fileLocation">The location of the file to check.</param>
        /// <returns>True if the file exists at the given file location.</returns>
        public static bool Exists(string fileLocation)
        {
            #if UNITY_WSA
            return UnityEngine.Windows.File.Exists(fileLocation);
            #else
            return System.IO.File.Exists(fileLocation);
            #endif
        }

        /// <summary>
        /// Deletes the file at the given file location. [Platform-independent]
        /// </summary>
        /// <param name="fileLocation">The location of the file to delete.</param>
        /// <returns>True if the file exists.</returns>
        public static bool Delete(string fileLocation)
        {
            if (Exists(fileLocation))
            {
                #if UNITY_WSA
                UnityEngine.Windows.File.Delete(fileLocation);
                #else
                System.IO.File.Delete(fileLocation);
                #endif

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// If a temporary file exists for the given file location (Utility.GetTemporaryPath(fileLocation), renames it to the given file location; overwriting the previous file.
        /// </summary>
        /// <param name="fileLocation">The original file location (not the temporary one).</param>
        /// <returns>True if the file has been renamed.</returns>
        public static bool ReplaceOldWithNew(string fileLocation)
        {
            string tempPath = Utility.GetTemporaryPath(fileLocation);
            
            if (Exists(tempPath))
            {
                if (Exists(fileLocation))
                    Delete(fileLocation);
                
                System.IO.File.Move(tempPath, fileLocation);

                return true;
            }
                
            return false;
        }
    }
}