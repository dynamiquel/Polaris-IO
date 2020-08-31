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

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Polaris.IO.Compression;

namespace Polaris.IO
{
    internal static class Utility
    {
        private static readonly Dictionary<FileType, string> DefaultFileExtensions = new Dictionary<FileType, string>
        {
            [FileType.Text] = Settings.DefaultTextExtension,
            [FileType.Json] = Settings.DefaultJsonExtension,
            [FileType.Yaml] = Settings.DefaultYamlExtension,
            [FileType.Binary] = Settings.DefaultBinaryExtension
        };
        
        public static string GetTemporaryPath(string path, bool reverse = false)
        {
            // return fileLocation without incomplete at the end.
            if (reverse)
            {
                if (Path.GetExtension(path) == Settings.FileRecoveryExtension)
                    return System.IO.Path.ChangeExtension(path, null);
                else
                    return path;
            }

            return $"{path}{Settings.FileRecoveryExtension}";
        }

        public static void BackupDirectory(string directoryLocation, int maxBackups = 2)
        {
            // If a folder called 'Backups' doesn't exist in parent directory, create it.
            // If a folder with the directory name doesn't exist in 'Backups', create it.
            // Copy the selected directory to the 'Backups/directoryName' folder with '_DateTime' at the end.
            // While the 'Backup/directoryName' directory contains more than maxBackups,
            // delete the oldest directory within the 'Backup' directory.
        }

        public static DirectoryInfo[] GetBackups(string directoryLocation)
        {
            // Go to '../Backups/fileLocation'.
            // Return all directory infos.
            return new DirectoryInfo[0];
        }

        public static bool RecoverBackup(string directoryLocation, int backupIndex)
        {
            var backups = GetBackups(directoryLocation);
            // Copy backup[backupIndex] to fileLocation.
            return true;
        }

        /// <summary>
        /// If the Default Extension is enabled for the given FileType and the file location doesn't currently have
        /// an extension, give it the Default Extension for the given FileType.
        /// </summary>
        /// <param name="fileLocation">The file location to check and alter.</param>
        public static void UpdateExtension(ref string fileLocation, FileType fileType)
        {
            if (!Path.HasExtension(fileLocation) && (Settings.UseDefaultExtensions & fileType) == fileType)
                fileLocation = Path.ChangeExtension(fileLocation, DefaultFileExtensions[fileType]);
        }

        public static byte[] DecompressHelper(byte[] input, CompressionType compressionType)
        {
            using (var outputStream = new MemoryStream())
            using (var inputStream = new MemoryStream(input))
            {
                // Forces the code to run synchronously.
                Task.Run(() => Compressor.Decompress(inputStream, outputStream, compressionType)).GetAwaiter().GetResult();
                return outputStream.ToArray();
            }
        }
        
        public static async Task<byte[]> DecompressHelperAsync(byte[] input, CompressionType compressionType)
        {
            using (var outputStream = new MemoryStream())
            using (var inputStream = new MemoryStream(input))
            {
                await Compressor.Decompress(inputStream, outputStream, compressionType).ConfigureAwait(false);
                return outputStream.ToArray();
            }
        }
        
        public static byte[] CompressHelper(byte[] input, CompressionType compressionType)
        {
            using (var outputStream = new MemoryStream())
            using (var inputStream = new MemoryStream(input))
            {
                // Forces the code to run synchronously.
                Task.Run(() => Compressor.Compress(inputStream, outputStream, compressionType)).GetAwaiter().GetResult();
                return outputStream.ToArray();
            }
        }
        
        public static async Task<byte[]> CompressHelperAsync(byte[] input, CompressionType compressionType)
        {
            using (var outputStream = new MemoryStream())
            using (var inputStream = new MemoryStream(input))
            {
                await Compressor.Compress(inputStream, outputStream, compressionType).ConfigureAwait(false);
                return outputStream.ToArray();
            }
        }
    }
}