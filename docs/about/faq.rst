.. _doc_about_faq:

Frequently asked questions
==========================
1. Are there any dependencies?
------------------------------
Yes. Since Polaris IO is simply a wrapper, it does not come with built-in support for serialisation and deserialisation of certain file types.
So far, our dependencies include:

- `Newtonsoft Json.NET <https://github.com/JamesNK/Newtonsoft.Json>`__ for JSON serialisation and deserialisation.
- `YamlDotNet <https://github.com/aaubry/YamlDotNet>`__ for YAML serialisation and deserialisation.

2. Which platforms does Polaris IO support?
----------------------------------------------------------
We hope to support all major platforms (Windows, macOS, Linux, UWP, PS4, iOS and Android). However, so far we can only confirm that **Windows Standalone** is supported as testing has not been done for other platforms.

3. What's even the point of Polaris IO?
---------------------------------------
If you've done I/O in Unity across multiple platforms, you've probably realised that there isn't an all-in-one solution to write or read files. Some platforms require special ways to read or write files due to security limitations or incompatible APIs. This forces you to use preprocessor directives to make your game support multiple platforms.

There's a good chance that you've already thought of making your own wrapper to prevent using preprocessor directives everytime you want to do I/O in Unity. Polaris IO just has it done for you.

Polaris IO also gives users a **higher-level** and **consistent** approach to writing and reading files. We don't want users to constantly lookup how to write or read with a particular file API. It should be an easy-to-remember syntax that's the same for every file API.

4. Which file types does Polaris IO support?
--------------------------------------------
Currently, Polaris IO support the following file types:

- **Plain text** through ``Text``.
- **JSON** through ``Json``.
- **YAML** through ``Yaml``.
- **Binary** through ``Binary``.

Feel free to contribute support for XML. I will not as I don't use it.

5. Does Polaris IO support asynchronous writing and reading?
------------------------------------------------------------
Of course, instead of using the ``Write()`` or ``Read()`` method, use ``WriteAsync()`` or ``ReadAsync()``.

There is no asynchronous equivalent for ``TryWrite()`` or ``TryRead()``. Instead, you can implement ``WriteAsync()`` or ``ReadAsync()`` inside a try-catch statement to get a similar effect.

In case you didn't know, ``Json.TryRead<T>(string fileLocation, out T obj)`` is just:
::

    public static bool TryRead<T>(string fileLocation, out T obj)
    {
        obj = default;
        
        try
        {
            obj = Read<T>(fileLocation);
            return true;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log($"Error whiles reading from '{fileLocation}'. Details: {e}.");
            return false;
        }
    }

6. What is File Recovery?
-------------------------
**File Recovery** is a tool in Polaris IO that helps ensure that a corrupt or incomplete file **never** overwrites an uncorrupt and complete file.

Wouldn't it suck if your hard-earned save data was gone because the game tried to save your data but something prevented it from completing?

File Recovery is a pretty simple concept. When a new file is created, ``FileRecoveryExtension`` is **appended** to the end of the path. When the file has **finished** writing, ``FileRecoveryExtension`` is **removed**. This ensures that the previous file is only overwritten by an uncorrupt and complete file.

By default, ``FileRecoveryExtension`` is ``".partial"``.

There is also **Directory Recovery**, which is a more manual process but follows the same concept.

7. How do I know when to Polaris.IO.File or Polaris.IO.Directory?
-----------------------------------------------------------------
Due to how platform-dependent I/O in Unity can be, if ``Polaris.IO.File`` or ``Polaris.IO.Directory`` has a method with the same name as one in ``System.IO.File`` or ``System.IO.Directory``, always try to use it.

There is a reason why ``Polaris.IO.File`` or ``Polaris.IO.Directory`` has those methods. The reason will usually be that the method within ``System.IO.File`` or ``System.IO.Directory`` doesn't work on all platforms.

8. What are the static paths?
-----------------------------
Polaris IO comes bundled with a number of strongly-typed static paths (both files and directories).
They are just paths you can easily reference anywhere in your code that work on all platforms.
These paths do not have to be used and can be refactored or removed entirely.

.. note:: Other Polaris systems may depend on some of these paths. We do not recommend renaming or removing any of the static paths if you are using other Polaris systems.

9. How do the default file extensions work?
-------------------------------------------
Within the ``Settings`` struct, you will find the following properties:

- ``FileType UseDefaultExtensions``
- ``string DefaultTextExtension``
- ``string DefaultJsonExtension``
- ``string DefaultYamlExtension``
- ``string DefaultBinaryExtension``

The enum ``UseDefaultExtensions`` uses flags to enable the default extension for each file type.
When a default extension is enabled and the corresponding file type doesn't already have an extension, the default extension is given.

10. Can I make modifications or give contributions?
---------------------------------------------------
Polaris IO is under the MIT License so you're free to make any modifications, as long as **copyright and license notices are preserved**.

Contributions are **always welcome**.