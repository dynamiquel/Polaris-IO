# Polaris IO
Polaris is a Game SDK for Unity that provides almost essential features. These features make sure your development time stays in what makes your game special.

## Introduction
IO is a subset of Polaris that makes it **simpler** to use common IO operations in Unity.

IO doesn't really add anything new to your program. It is simply a wrapper that gives you a **platform-independent**, **higher-level** and **consistent** approach to **common IO functions**. The simplification may seem minor but it can really reduce redundancy.

Of course this higher-level approach may not be ideal for all circumstances, but you can always use this side by side with the more native and lower-level methods.

I initially designed IO to reduce redundancy in my code and to easily switch between System.IO and UnityEngine.Windows when creating UWP games. This small system has made doing IO operations so much easier for me that I thought why not share it.

## Simplification
- **Why remember how to write and read with every file format?** Just use Json.Write(), Binary.Write(), Yaml.Write(), etc.
- **Why have redundant lines of code?** Just use a one liner.
- **Why write your own async operations?** Just use Json.ReadAsync(), Binary.ReadAsync(), etc.
- **Why try to remember the location of the save folder?** Just use strongly-typed references like Paths.SaveFolder.
- **Why replace a working older save file with a newer corrupt one?** When enabled, every file you create will only overwrite when all data has been written.
- **Why create your own backup system?** Do Utilities.BackupFolder() to backup any folder with a desired backup limit, and then use Utilities.RecoverBackup() to recover a desired backup.

## Features
- Write, TryWrite, WriteAsync, Read, TryRead, ReadAsync
  - Text
  - JSON
  - YAML
  - Binary
- File and directory recovery
- Default file extensions
- Strongly-typed static references for paths

View the **[wiki](https://github.com/dynamiquel/Polaris-IO/wiki)** to learn more.

## Prerequisites
- Unity 2019.3 or later
- YamlDotNet
- Json.NET

## Adding to your Unity project
<ol>
  <li>Go to the <a href="https://github.com/dynamiquel/Polaris-IO/releases/latest">latest release</a> and download the desired version.</li>
  <li>Extract the <i><b>Plugins</b></i> folder to your project's <i><b>Assets</b></i> folder.</li>
