Polaris-IO Beta 2009
https://github.com/dynamiquel/Polaris-IO

ADDING TO YOUR UNITY PROJECT:
    - Extract the 'Plugins' folder to your project's 'Assets' folder.

LICENSE:
    Copyright (c) 2020 dynamiquel
    
    MIT License
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.


KNOWN ISSUES:
    - Probably not an issue but LZ4 doesn't perform much faster than GZIP.
    - Yaml can't deserialise the 'textExample' string. Cannot find the exact reason.
      Seems to be when the '>+' symbol is used.
    
    
NOT YET TESTED:
    - Any platform other than 'Windows Standalone'.
    - Yaml has only been lightly tested.
    - Binary has only been lighty tested.
    - Directory.Create.


NOT YET IMPLEMENTED:
    - Backup system.
    - Any platform support other than 'Standalone' and 'UWP'.
    - Binary.
    - Attributes.
    - Json Naming Conventions: Snake-case and Kebab-case.
    - Binary documentation.