.. _doc_tutorials_json:

Json
====
Introduction
------------
From the `JSON <https://www.json.org/>`__ website:
    **JSON** (JavaScript Object Notation) is a lightweight data-interchange format. It is easy for humans to read and write. It is easy for machines to parse and generate. It is based on a subset of the `JavaScript Programming Language Standard ECMA-262 3rd Edition - December 1999 <http://www.ecma-international.org/publications/files/ecma-st/ECMA-262.pdf>`__. JSON is a text format that is completely language independent but uses conventions that are familiar to programmers of the C-family of languages, including C, C++, C#, Java, JavaScript, Perl, Python, and many others. These properties make JSON an ideal data-interchange language.

JSON allows users to store their C# objects into an easily-readable text file, which can be later loaded.

Since JSON can be used for all programming languages, you can create a JSON file from a C# object in Unity and easily convert it to a Python object in another program, and vice versa.

JSON files are serialised and deserialised using `Newtonsoft Json.NET <https://github.com/JamesNK/Newtonsoft.Json>`__.

Saving objects to JSON files
----------------------------
**Synchronous Example:**

To save an object to a JSON file use ``Json.Write(string fileLocation, object obj)``. This converts the given ``object`` to a JSON-formatted string and then writes that string to a file at the given ``fileLocation``.

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

            // Creates a JSON file at 'path', containing the fields of 'playerData'.
            Polaris.IO.Json.Write(path, playerData);
  
            // Contents of the save.data file.
            // {
            //     "PlayerName": "James",
            //     "Kills": 125,
            //     "Exp": 777
            // }
        }
    }

**Asynchronous Example:**

Alternatively, you can asynchronously save an object to a JSON file by using ``Json.WriteAsync(string fileLocation, object obj)``. This is more suitable for larger files or files on a network as it doesn't block the main thread.

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

            // Executes the entire method asynchronously (converting to JSON and writing the text file).
            await Json.WriteAsync(path, playerData);

            // Only writing the text file is executed asynchronously.
            Json.WriteAsync(path, playerData);
        }
    }

Creating objects from JSON files
--------------------------------
**Synchronous Example:**

To create an object from a JSON file use ``Json.Read<T>(string fileLocation)``. This reads the text from the file at the given ``fileLocation`` as JSON, creates an object out of it, casts the object as the given type ``T`` and returns it.

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
            // Creates a PlayerData object from the JSON file at 'path'.
            var playerData = Polaris.IO.Json.Read<PlayerData>(path);
        }
    }


If the file at the given ``fileLocation`` can not create an object of the given type ``T``, an exception may be thrown or a ``default(T)`` object will be returned.

**Synchronous Try Example:**

A safer way of creating an object from a JSON file is to use ``Json.TryRead<T>(string fileLocation, out T obj)``. This will catch and swallow all exceptions, as well as always providing you with a ``default(T)`` if an exception is caught. 

Unlike ``Json.Read<T>``, which returns the object, ``Json.TryRead<T>`` returns a boolean and uses an ``out`` parameter to provide you with the object. The return value indicates whether an object has been successfully created from the JSON file or not.

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
            // Attempts to create a PlayerData object from the JSON file at 'path'.
            bool success = Json.TryRead<PlayerData>(path, out var playerData);

            // If the conversion was unsuccessful, create a new 'PlayerData'.
            if (success == false)
                playerData = new PlayerData();
        }
    }


**Asynchronous Example:**

Alternatively, you can asynchronously create an object from a JSON file by using ``Json.ReadAsync<T>(string fileLocation)``. This is more suitable for larger files or files on a network as it doesn't block the main thread.

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
            // Asynchronously creates a PlayerData object from the JSON file at 'path'.
            var playerData = await Json.ReadAsync<PlayerData>(path);
        }
    }


Creating objects from JSON-formatted strings
--------------------------------------------
You don't need to read text directly from a file to create an object from JSON. You can also create an object directly from a JSON-formatted string using ``Json.ReadString<T>(string jsonString)``. This is useful if you have an alternate method to retrieve the text file.

**Example:**

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

        void Start()
        {
            // An example of an alternate method of reading text from a file.
            string jsonString = System.IO.File.ReadAllText(path);

            // Creates a PlayerData object from the given JSON-formatted string.
            var playerData = Json.ReadString<PlayerData>(jsonString);
        }
    }


Creating JSON-formatted strings from objects
--------------------------------------------
Like, creating objects from JSON-formatted strings, you can also create JSON-formatted strings from objects by using ``Json.ToString(object obj, INamingConvention namingConvention)``. This is useful if you have an alternate method to save the string to a text file.

**Example:**

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

        void Start()
        {
            var playerData = new PlayerData();

            playerData.Username = "James";
            playerData.Kills = 125;
            playerData.Exp = 777;

            // Creates a JSON-formatted string from the given object.
            // The string is formatted with a Pascal Case naming convention.
            var jsonString = Json.ToString(playerData, new PascalCaseNamingConvention());

            // An example of an alternate method of saving the JSON-formatted string to a file.
            System.IO.File.WriteAllText(path, jsonString);
        }
    }

