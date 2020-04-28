﻿//  This file is part of Polaris-IO - An IO wrapper for Unity.
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
    public static class Directory
    {
        /// <summary>
        /// Checks if a directory exists at the given directory location. [Platform-independent]
        /// </summary>
        /// <param name="directoryLocation">The location of the directory to check.</param>
        /// <returns>True if the directory exists at the given file location.</returns>
        public static bool Exists(string directoryLocation)
        {
            #if UNITY_WSA
            return UnityEngine.Windows.Directory.Exists(directoryLocation);
            #else
            return System.IO.Directory.Exists(directoryLocation);
            #endif
        }

        /// <summary>
        /// Deletes the directory at the given directory location. [Platform-independent]
        /// </summary>
        /// <param name="directoryLocation">The location of the directory to delete.</param>
        /// <returns>True if the directory exists.</returns>
        public static bool Delete(string directoryLocation)
        {
            if (Exists(directoryLocation))
            {
                #if UNITY_WSA
                UnityEngine.Windows.Directory.Delete(directoryLocation);
                #else
                System.IO.Directory.Delete(directoryLocation);
                #endif

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// If a temporary directory exists for the given directory location (Utility.GetTemporaryPath(directoryLocation), renames it to the given directory location; overwriting the previous directory.
        /// </summary>
        /// <param name="directoryLocation">The original directory location (not the temporary one).</param>
        /// <returns>True if the directory has been renamed.</returns>
        public static bool ReplaceOldDirectoryWithNew(string directoryLocation)
        {
            string tempPath = Utility.GetTemporaryPath(directoryLocation);
            
            if (Exists(tempPath))
            {
                if (Exists(directoryLocation))
                    Delete(directoryLocation);
                
                System.IO.Directory.Move(tempPath, directoryLocation);
                return true;
            }

            return false;
        }
    }
}