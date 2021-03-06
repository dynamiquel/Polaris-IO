﻿//  This file is part of Polaris-IO - An IO wrapper for Unity.
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

using System.Threading.Tasks;
using Polaris.IO.Compression;

namespace Polaris.IO
{
    /// <summary>
    /// An <see cref="Polaris.IO.ISerialiser"/> for human-readable serialisers.
    /// Includes a <see cref="Polaris.IO.NamingConvention"/> parameter.
    /// </summary>
    /*
    internal interface IHumanSerialiser : ISerialiser
    {
        void Write(string fileLocation, object value, NamingConvention namingConvention);
        void Write(string fileLocation, object value, NamingConvention namingConvention, CompressionType compressionType);
        Task WriteAsync(string fileLocation, object value, NamingConvention namingConvention);
        Task WriteAsync(string fileLocation, object value, NamingConvention namingConvention, CompressionType compressionType);
        
        byte[] GetBytes(object value, NamingConvention namingConvention);
        byte[] GetBytes(object value, NamingConvention namingConvention, CompressionType compressionType);
        Task<byte[]> GetBytesAsync(object value, NamingConvention namingConvention);
        Task<byte[]> GetBytesAsync(object value, NamingConvention namingConvention, CompressionType compressionType);
    }
    */
}