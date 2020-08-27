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

namespace Polaris.IO
{
    public static class Text
    {
        #region Write

        /// <summary>
        /// Creates a new file, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The string to write to the file.</param>
        public static void Write(string fileLocation, string content)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Text);
            
            File.WriteAllText(fileLocation, content);
        }

        /// <summary>
        /// Creates a new file, compresses the contents, writes the compressed contents to the file,
        /// and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The string to write to the file.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static void Write(string fileLocation, string content, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                Write(fileLocation, content);
            else
                Write(fileLocation, Encoding.UTF8.GetBytes(content), compressionType);
        }

        /// <summary>
        /// Creates a new file, writes the contents to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The string to write to the file.</param>
        public static async Task WriteAsync(string fileLocation, string content)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Text);
            
#if UNITY_WSA
                var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(fileLocation);
                var file = await folder.CreateFileAsync(fileLocation, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(file, content);
#else
            using (var sw = new StreamWriter(fileLocation, false))
            {
                await sw.WriteAsync(content).ConfigureAwait(false);
            }
#endif
        }

        /// <summary>
        /// Creates a new file, compresses the contents, writes the compressed contents to the file,
        /// and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The string to write to the file.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static async Task WriteAsync(string fileLocation, string content, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                await WriteAsync(fileLocation, content).ConfigureAwait(false);
            else
                await WriteAsync(fileLocation, Encoding.UTF8.GetBytes(content), compressionType).ConfigureAwait(false);
        }

        #endregion


        #region Read

        /// <summary>
        /// Opens a text file, reads all the text in the file into a string, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns>The contents of the file in a string.</returns>
        public static string Read(string fileLocation)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Text);

            return File.ReadAllText(fileLocation);
        }

        /// <summary>
        /// Opens a text file, reads all the text in the file, decompresses it into a string, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.</param>
        /// <returns>The contents of the file in a string.</returns>
        public static string Read(string fileLocation, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return Read(fileLocation);
            
            return Encoding.UTF8.GetString(ReadAsBytes(fileLocation, compressionType));
        }

        /// <summary>
        /// Opens a text file, reads all the text in the file into a string, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns>The contents of the file in a string.</returns>
        public static async Task<string> ReadAsync(string fileLocation)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Text);
            
#if UNITY_WSA
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(fileLocation);
            return await Windows.Storage.FileIO.ReadTextAsync(file);
#else
            using (var sr = new StreamReader(fileLocation))
            {
                return await sr.ReadToEndAsync().ConfigureAwait(false);
            }
#endif
        }

        /// <summary>
        /// Opens a text file, reads all the text in the file, decompresses it into a string, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.</param>
        /// <returns>The contents of the file in a string.</returns>
        public static async Task<string> ReadAsync(string fileLocation, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return await ReadAsync(fileLocation).ConfigureAwait(false);
                    
            return Encoding.UTF8.GetString(await ReadAsBytesAsync(fileLocation, compressionType).ConfigureAwait(false));
        }

        #endregion
        
        
        #region Write As Bytes

        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The bytes to write to the file.</param>
        public static void Write(string fileLocation, byte[] content)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Text);
            
            File.WriteAllBytes(fileLocation, content);
        }

        /// <summary>
        /// Creates a new file, compresses the specified byte array and writes it to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The bytes to write to the file.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        public static void Write(string fileLocation, byte[] content, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
            {
                Write(fileLocation, content);
                return;
            }
            
#if UNITY_STANDALONE || UNITY_EDITOR
            // I would assume this is more efficient due to less byte <-> Stream conversions.
            Utility.UpdateExtension(ref fileLocation, FileType.Text);

            using (var outputStream = new FileStream(fileLocation, FileMode.Create))
            using (var inputStream = new MemoryStream(content))
            {
                // Forces the code to run synchronously.
                Task.Run(() => Compressor.Compress(inputStream, outputStream, compressionType)).GetAwaiter().GetResult();
            }
#else
            // Fallback.
            Write(fileLocation, Utility.CompressHelper(content, compressionType));
#endif
        }

        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The bytes to write to the file.</param>
        public static async Task WriteAsync(string fileLocation, byte[] content)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Text);

#if UNITY_WSA
                var folder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(fileLocation);
                var file = await folder.CreateFileAsync(fileLocation, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteBytesAsync(file, content);
#else
            using (FileStream outputStream = System.IO.File.Create(fileLocation))
            {
                //outputStream.Seek(0, SeekOrigin.End);
                await outputStream.WriteAsync(content, 0, content.Length).ConfigureAwait(false);
            }
#endif
        }

        /// <summary>
        /// Creates a new file, compresses the specified byte array and writes it to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The bytes to write to the file.</param>
        public static async Task WriteAsync(string fileLocation, byte[] content, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
            {
                await WriteAsync(fileLocation, content).ConfigureAwait(false);
                return;
            }
            
#if UNITY_STANDALONE || UNITY_EDITOR
            // I would assume this is more efficient due to less byte <-> Stream conversions.
            Utility.UpdateExtension(ref fileLocation, FileType.Text);

            using (var outputStream = new FileStream(fileLocation, FileMode.Create))
            using (var inputStream = new MemoryStream(content))
            {
                await Compressor.Compress(inputStream, outputStream, compressionType).ConfigureAwait(false);
            }
#else
            // Fallback.
            var bytes = await Utility.CompressHelperAsync(content, compressionType).ConfigureAwait(false);
            await WriteAsync(fileLocation, bytes).ConfigureAwait(false);
#endif
        }

        #endregion
        
        
        #region Read As Bytes

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns>The contents of the file in a byte array.</returns>
        public static byte[] ReadAsBytes(string fileLocation)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Text);

            var bytes = File.ReadAllBytes(fileLocation);
            return bytes;
        }

        /// <summary>
        /// Opens a binary file, reads the contents of the file and decompresses it into a byte array,
        /// and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.</param>
        /// <returns>The contents of the file in a byte array.</returns>
        public static byte[] ReadAsBytes(string fileLocation, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return ReadAsBytes(fileLocation);
            
#if UNITY_STANDALONE || UNITY_EDITOR
            // I would assume this is more efficient due to less byte <-> Stream conversions.
            Utility.UpdateExtension(ref fileLocation, FileType.Text);

            using (var outputStream = new MemoryStream())
            using (var inputStream = new FileStream(fileLocation, FileMode.Open))
            {
                // Forces the code to run synchronously.
                Task.Run(() => Compressor.Decompress(inputStream, outputStream, compressionType)).GetAwaiter().GetResult();
                return outputStream.ToArray();
            }
#else
            // Fallback.
            return Utility.DecompressHelper(ReadAsBytes(fileLocation), compressionType);
#endif
        }

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns>The contents of the file in a byte array.</returns>
        public static async Task<byte[]> ReadAsBytesAsync(string fileLocation)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Text);

#if UNITY_WSA
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(fileLocation);
            return Encoding.UTF8.GetBytes(await Windows.Storage.FileIO.ReadTextAsync(file));
#else
            byte[] result;

            using (FileStream inputStream = System.IO.File.Open(fileLocation, FileMode.Open))
            {
                result = new byte[inputStream.Length];
                await inputStream.ReadAsync(result, 0, (int)inputStream.Length).ConfigureAwait(false);
            }

            return result;
#endif
        }

        /// <summary>
        /// Opens a binary file, reads the contents of the file and decompresses it into a byte array,
        /// and then closes the file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="compressionType">The type of decompression to use.</param>
        /// <returns>The contents of the file in a byte array.</returns>
        public static async Task<byte[]> ReadAsBytesAsync(string fileLocation, CompressionType compressionType)
        {
            if (compressionType == CompressionType.None)
                return await ReadAsBytesAsync(fileLocation).ConfigureAwait(false);
            
#if UNITY_STANDALONE || UNITY_EDITOR
            // I would assume this is more efficient due to less byte <-> Stream conversions.
            Utility.UpdateExtension(ref fileLocation, FileType.Text);

            using (var outputStream = new MemoryStream())
            using (var inputStream = new FileStream(fileLocation, FileMode.Open))
            {
                await Compressor.Decompress(inputStream, outputStream, compressionType).ConfigureAwait(false);
                return outputStream.ToArray();
            }
#else
            // Fallback.
            var bytes = await ReadAsBytesAsync(fileLocation).ConfigureAwait(false);
            return await Utility.DecompressHelperAsync(bytes, compressionType).ConfigureAwait(false);
#endif
        }

        #endregion


        #region Try

        /// <summary>
        /// Attempts to create a new file, write the contents to the file, and then close the file.
        /// If the target file already exists, it is overwritten. Catches all exceptions.
        /// Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The string to write to the file.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool TryWrite(string fileLocation, string content) =>
            TryWrite(fileLocation, content, CompressionType.None);

        /// <summary>
        /// Attempts to create a new file, compress the contents, write the compressed contents to the file,
        /// and then close the file. If the target file already exists, it is overwritten. Catches all exceptions.
        /// Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="content">The string to write to the file.</param>
        /// <param name="compressionType">The type of compression to use.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool TryWrite(string fileLocation, string content, CompressionType compressionType)
        {
            try
            {
                Write(fileLocation, content, compressionType);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to open a text file, read all the text in the file into a string, and then close the file.
        /// Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="result">The contents of the file in a string.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool TryRead(string fileLocation, out string result) =>
            TryRead(fileLocation, CompressionType.None, out result);

        /// <summary>
        /// Attempts to open a text file, read all the text in the file and decompresses it into a string,
        /// and then close the file. Catches all exceptions. Returns true if successful.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="result">The contents of the file in a string.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool TryRead(string fileLocation, CompressionType compressionType, out string result)
        {
            try
            {
                result = Read(fileLocation, compressionType);
                return true;
            }
            catch (Exception e)
            {
                result = string.Empty;
                return false;
            }
        }

        #endregion
        
        
        #region Replace

        /// <summary>
        /// Opens up a file and replaces all occurrences of a specified string with another specified string.
        /// The changes are then saved.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="from">The contents to be replaced.</param>
        /// <param name="to">The contents to replace with.</param>
        public static void Replace(string fileLocation, string from, string to) =>
            Replace(fileLocation, from, to, CompressionType.None);
        
        /// <summary>
        /// Opens up a file and replaces all occurrences of a specified string with another specified string.
        /// The changes are then saved.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="from">The contents to be replaced.</param>
        /// <param name="to">The contents to replace with.</param>
        /// <param name="compressionType">The type of compression and decompression to use.</param>
        public static void Replace(string fileLocation, string from, string to, CompressionType compressionType)
        {
            var contents = Read(fileLocation, compressionType);
            contents = contents.Replace(from, to);
            Write(fileLocation, contents, compressionType);
        }

        /// <summary>
        /// Opens up a file and replaces all occurrences of a specified string with another specified string.
        /// The changes are then saved.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="from">The contents to be replaced.</param>
        /// <param name="to">The contents to replace with.</param>
        public static Task ReplaceAsync(string fileLocation, string from, string to) =>
            ReplaceAsync(fileLocation, from, to, CompressionType.None);
        
        /// <summary>
        /// Opens up a file and replaces all occurrences of a specified string with another specified string.
        /// The changes are then saved.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="from">The contents to be replaced.</param>
        /// <param name="to">The contents to replace with.</param>
        /// <param name="compressionType">The type of compression and decompression to use.</param>
        public static async Task ReplaceAsync(string fileLocation, string from, string to, CompressionType compressionType)
        {
            var contents = await ReadAsync(fileLocation, compressionType).ConfigureAwait(false);
            contents = contents.Replace(from, to);
            await WriteAsync(fileLocation, contents, compressionType).ConfigureAwait(false);
        }
        
        #endregion


        /// <summary>
        /// A direct way of getting the Stream for the given file. Can be used to write or read files.
        /// Not yet fully implemented.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="fileMode">Use either FileMode.Open or FileMode.Create.</param>
        /// <returns></returns>
        public static Stream GetStream(string fileLocation, FileMode fileMode)
        {
            Utility.UpdateExtension(ref fileLocation, FileType.Text);

#if UNITY_STANDALONE || UNITY_EDITOR
            // I would assume this is more efficient due to less byte <-> Stream conversions.

            return new FileStream(fileLocation, fileMode);
#else
            // Fallback.
            if (fileMode == FileMode.Open)
            {
                var bytes = ReadAsBytes(fileLocation);
                return new MemoryStream(bytes);
            }
            else if (fileMode == FileMode.Create || fileMode == FileMode.CreateNew)
            {
                // dunno
                throw new NotImplementedException();
            }
#endif
        }
    }
}