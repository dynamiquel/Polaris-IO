.. _doc_tutorials_binary:

Binary
======
Introduction
------------
Unlike `Json <json.rst>`__ and `Yaml <yaml.rst>`__, Binary is not easily-readable for humans and is difficult to use with any programming language other than C#. This makes Binary recommended for save files as it's pretty difficult to edit.

.. note:: When using Binary to store save data, it's recommended to use `Json <json.rst>`__ or `Yaml <yaml.rst>`__ for the meta/header data, with a reference to the version of the save data format. This is because versioning is difficult with binary files.

Binary files are serialised and deserialised using `.NET's BinaryFormatter <https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.formatters.binary.binaryformatter?view=netcore-3.1>`__.

Saving objects to binary files
----------------------------
**Synchronous Example:**

To save an object to a binary file use ``Binary.Write(string fileLocation, object obj)``. This converts the given ``object`` to a series of bytes, which is then written a file at the given ``fileLocation``.

::

    public class GameSaver : MonoBehaviour
    {
        class PlayerData
        {
            public string Username { get; set; }
            public int Kills { get; set; }
            public int Exp { get; set; }
        }

        string path = Path.Combine(Application.persistentDataPath, "save.data");

        void Start()
        {
            var playerData = new PlayerData();

            playerData.Username = "James";
            playerData.Kills = 125;
            playerData.Exp = 777;

            // Creates a binary file at 'path', containing the fields of 'playerData'.
            Polaris.IO.Binary.Write(path, playerData);
  
            // Contents of the save.data file.
            // "PlayerName": "James"
            // "Kills": 125
            // "Exp": 777
        }
    }

**Asynchronous Example:**

Alternatively, you can asynchronously save an object to a binary file by using ``Binary.WriteAsync(string fileLocation, object obj)``. This is more suitable for larger files or files on a network as it doesn't block the main thread.

::

    using Polaris.IO;

    public class GameSaver : MonoBehaviour
    {
        class PlayerData
        {
            public string Username { get; set; }
            public int Kills { get; set; }
            public int Exp { get; set; }
        }

        string path = Path.Combine(Application.persistentDataPath, "save.data");

        async void Start()
        {
            var playerData = new PlayerData();

            playerData.Username = "James";
            playerData.Kills = 125;
            playerData.Exp = 777;

            // Executes the entire method asynchronously (converting to binary and writing the text file).
            await Binary.WriteAsync(path, playerData);

            // Only writing the text file is executed asynchronously.
            Binary.WriteAsync(path, playerData);
        }
    }

Creating objects from binary files
--------------------------------
**Synchronous Example:**

To create an object from a binary file use ``Binary.Read<T>(string fileLocation)``. This reads the binary data from the file at the given ``fileLocation``, creates an object out of it, casts the object as the given type ``T`` and returns it.

::

    public class GameSaver : MonoBehaviour
    {
        class PlayerData
        {
            public string Username { get; set; }
            public int Kills { get; set; }
            public int Exp { get; set; }
        }

        string path = Path.Combine(Application.persistentDataPath, "save.data");

        void Start()
        {
            // Creates a PlayerData object from the binary file at 'path'.
            var playerData = Polaris.IO.Binary.Read<PlayerData>(path);
        }
    }


If the file at the given ``fileLocation`` can not create an object of the given type ``T``, an exception may be thrown or a ``default(T)`` object will be returned.

**Synchronous Try Example:**

A safer way of creating an object from a binary file is to use ``Binary.TryRead<T>(string fileLocation, out T obj)``. This will catch and swallow all exceptions, as well as always providing you with a ``default(T)`` if an exception is caught. 

Unlike ``Binary.Read<T>``, which returns the object, ``Binary.TryRead<T>`` returns a boolean and uses an ``out`` parameter to provide you with the object. The return value indicates whether an object has been successfully created from the binary file or not.

::

    public class GameSaver : MonoBehaviour
    {
        class PlayerData
        {
            public string Username { get; set; } = "New Player";
            public int Kills { get; set; } = 0;
            public int Exp { get; set; } = 0;
        }

        string path = Path.Combine(Application.persistentDataPath, "save.data");

        void Start()
        {
            // Attempts to create a PlayerData object from the binary file at 'path'.
            bool success = Binary.TryRead<PlayerData>(path, out var playerData);

            // If the conversion was unsuccessful, create a new 'PlayerData'.
            if (success == false)
                playerData = new PlayerData();
        }
    }


**Asynchronous Example:**

Alternatively, you can asynchronously create an object from a binary file by using ``Binary.ReadAsync<T>(string fileLocation)``. This is more suitable for larger files or files on a network as it doesn't block the main thread.

::

    using Polaris.IO;

    public class GameSaver : MonoBehaviour
    {
        class PlayerData
        {
            public string Username { get; set; }
            public int Kills { get; set; }
            public int Exp { get; set; }
        }

        string path = Path.Combine(Application.persistentDataPath, "save.data");

        async void Start()
        {
            // Asynchronously creates a PlayerData object from the binary file at 'path'.
            var playerData = await Binary.ReadAsync<PlayerData>(path);
        }
    }
