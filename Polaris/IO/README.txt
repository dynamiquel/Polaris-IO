Polaris-IO Beta 2004.1
https://github.com/dynamiquel/Polaris-IO

ADDING TO YOUR UNITY PROJECT:
    - Extract the 'Plugins' folder to your project's 'Assets' folder.

LICENSE:
    Copyright (c) 2020 dynamiquel and contributors
    
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
    - File Recovery doesn't work with 'Binary.Write()' or 'Binary.TryWrite()'.
      Workarounds: Use 'Binary.WriteAsync()' or set 'Settings.EnableFileRecovery' to false.
    
    
NOT YET TESTED:
    - Any platform other than 'Windows Standalone'.
    - File Recovery on large files (> 2 MBs).
    - Folder Recovery.
    - 'TryRead()' and 'TryWrite()'.


NOT YET IMPLEMENTED:
    - Backup system.
    - Any platform support other than 'Windows Standalone' and 'UWP'.