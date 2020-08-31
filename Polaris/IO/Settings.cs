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

namespace Polaris.IO
{
    public struct Settings
    {
        /// <summary>
        /// Uses flags to enable default extensions for each file type.
        /// When a default extension is enabled and the corresponding file type doesn't already have an extension, the default extension is given.
        /// </summary>
        public static FileType UseDefaultExtensions => FileType.Text | FileType.Binary | FileType.Json | FileType.Yaml;
        public static string DefaultTextExtension => ".txt";
        public static string DefaultBinaryExtension => ".bin";
        public static string DefaultJsonExtension => ".json";
        public static string DefaultYamlExtension => ".yaml";
        /// <summary>
        /// If enabled, it can help prevent a corrupt ot incomplete file from overwriting a complete file.
        /// More info: When creating a new file, it has the <see cref="Polaris.IO.Settings.FileRecoveryExtension"/> appended to the end of it.
        /// When the file has been fully written, the <see cref="Polaris.IO.Settings.FileRecoveryExtension"/> is removed.
        /// </summary>
        public static bool EnableFileRecovery => false;
        public static string FileRecoveryExtension => ".part";
    }

    [Flags]
    public enum FileType
    {
        Text = 1 << 0,
        Binary = 1 << 1,
        Json = 1 << 2,
        Yaml = 1 << 3
    }
}