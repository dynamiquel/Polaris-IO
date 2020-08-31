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

using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Debug = UnityEngine.Debug;

namespace Polaris.IO
{
    /// <summary>
    /// Responsible for encrypting and decrypting data.
    /// </summary>
    public static class Encryption
    {
        private static readonly int KeySize = 256;
        private static readonly byte[] Salt = new byte[] {156, 41, 197, 67, 29, 159, 125, 140, 118, 115, 192, 255, 246, 92, 219, 118};
        private static readonly string DefaultKey = "Polaris";

        /// <summary>
        /// Reads the data from a specified stream, encrypts it using a predefined key,
        /// and writes the encrypted data to another specified stream.
        /// </summary>
        /// <param name="inputStream">The stream to read unencrypted data from.</param>
        /// <param name="outputStream">The stream to write encrypted data to.</param>
        public static void Encrypt(Stream inputStream, Stream outputStream) =>
            Encrypt(inputStream, outputStream, DefaultKey);

        /// <summary>
        /// Reads the data from a specified stream, encrypts it using a specified key,
        /// and writes the encrypted data to another specified stream.
        /// </summary>
        /// <param name="inputStream">The stream to read unencrypted data from.</param>
        /// <param name="outputStream">The stream to write encrypted data to.</param>
        /// <param name="key">The key (password) to use for encryption.</param>
        public static void Encrypt(Stream inputStream, Stream outputStream, string key)
        {
            var iv = GenerateRandomIV();

            // Writes the IV at the start of the output stream.
            foreach (var item in iv)
                outputStream.WriteByte(item);
            
            using (var aes = Aes.Create())
            {
                aes.Key = GetProcessedKey(key);
                aes.IV = iv;

                // Writes the encrypted data to the output stream.
                using (var cryptStream =
                    new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inputStream.Position = 0;
                    inputStream.CopyTo(cryptStream);
                }
            }
        }

        /// <summary>
        /// Reads the data from a specified stream, decrypts it using a predefined key,
        /// and writes the decrypted data to another specified stream.
        /// </summary>
        /// <param name="inputStream">The stream to read encrypted data from.</param>
        /// <param name="outputStream">The stream to write decrypted data to.</param>
        public static void Decrypt(Stream inputStream, Stream outputStream) =>
            Decrypt(inputStream, outputStream, DefaultKey);

        /// <summary>
        /// Reads the data from a specified stream, decrypts it using a specified key,
        /// and writes the decrypted data to another specified stream.
        /// </summary>
        /// <param name="inputStream">The stream to read encrypted data from.</param>
        /// <param name="outputStream">The stream to write decrypted data to.</param>
        /// <param name="key">The key (password) to use for decryption (must match the key used for encryption).</param>
        public static void Decrypt(Stream inputStream, Stream outputStream, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = GetProcessedKey(key);
                aes.IV = GetIVFromStream(inputStream);

                // Writes the decrypted data to the output stream.
                using (var cryptStream =
                    new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cryptStream.CopyTo(outputStream);
                }
            }
        }

        private static byte[] GetProcessedKey(string key)
        {
            var passwordBytes = new Rfc2898DeriveBytes(key, Salt);
            return passwordBytes.GetBytes(KeySize / 8);
        }
        
        /// <summary>
        /// Generates a random IV.
        /// <br></br>I don't understand the point of storing an IV in a file, but whatever.
        /// </summary>
        /// <returns></returns>
        private static byte[] GenerateRandomIV()
        {
            var iv = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }

            return iv;
        }
        
        /// <summary>
        /// Reads the IV from the input stream.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        private static byte[] GetIVFromStream(Stream inputStream)
        {
            inputStream.Position = 0;
            var iv = new byte[16];
            for (var i = 0; i < iv.Length; i++)
                iv[i] = (byte)inputStream.ReadByte();

            return iv;
        }
    }
}