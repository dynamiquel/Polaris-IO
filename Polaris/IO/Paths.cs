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

using System.IO;
using UnityEngine;

namespace Polaris.IO
{
    public struct Paths
    {
        // Local/GameID/UserData
        public static string UserData => Path.Combine(Application.persistentDataPath, "UserData");

        // Local/GameID/UserData/UserID
        public static string UserHome => Path.Combine(UserData, "Player0");

        // Local/GameID/UserData/UserID/UserOptions
        public static string UserOptions => Path.Combine(UserHome, "UserOptions");

        // Local/GameID/UserData/UserID/UserOptions/Keybinds
        public static string KeyBindings => Path.Combine(UserOptions, "KeyBindings");

        // Local/GameID/UserData/UserID/UserOptions/VideoAudioOptions
        public static string VideoAudioOptions => Path.Combine(UserOptions, "VideoAudioOptions");

        // Local/GameID/UserData/UserID/UserOptions/Preferences
        public static string Preferences => Path.Combine(UserOptions, "Preferences");

        // Local/GameID/UserData/UserID/UserBinaries
        public static string UserBinaries => Path.Combine(UserHome, "UserBinaries");

        // Local/GameID/UserData/UserID/UserBinaries/OnlineData
        public static string OnlineData => Path.Combine(UserBinaries, "OnlineData");

        // Local/GameID/UserData/UserID/UserBinaries/LocalData
        public static string LocalData => Path.Combine(UserBinaries, "LocalData");

        // Local/GameID/UserData/UserID/UserLogs
        public static string UserLogs => Path.Combine(UserHome, "UserLogs");

        // Local/GameID/System
        public static string System => Path.Combine(Application.persistentDataPath, "System");

        // Local/GameID/System/Benchmarks
        public static string Benchmarks => Path.Combine(System, "Benchmarks");

        // Local/GameID/System/Screenshots
        public static string Screenshots => Path.Combine(System, "Screenshots");

        // Local/GameID/System/SystemInfo
        public static string SystemInfo => Path.Combine(System, "SystemInfo");
        
        // Feel free to add your own static paths here.
    }
}