.. _doc_yaml:
Yaml
====
Introduction
------------
From `The Official YAML Web Site <https://yaml.org/>`__:
    YAML is a human friendly data serialization standard for all programming languages.

YAML allows users to store their C# objects into an easily-readable text file, which can be later loaded. Due to its readability, it is often used for config files and modding. It is very similar to JSON, but even more readable.

Since YAML can be used for all programming languages, you can create a YAML file from a C# object in Unity and easily convert it to a Python object in another program, and vice versa.

YAML files are serialised and deserialised using `YamlDotNet <https://github.com/aaubry/YamlDotNet>`__.

.. note:: Currently, only public properties can be saved to and loaded from YAML files.

Saving objects to YAML files
----------------------------
**Synchronous Example:**

To save an object to a YAML file use ``Yaml.Write(string fileLocation, object obj)``. This converts the given ``object`` to a YAML-formatted string and then writes that string to a file at the given ``fileLocation``.

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

            // Creates a YAML file at 'path', containing the fields of 'playerData'.
            Polaris.IO.Yaml.Write(path, playerData);
  
            // Contents of the save.data file.
            // PlayerName: James
            // Kills: 125
            // Exp: 777
        }
    }

**Asynchronous Example:**

Alternatively, you can asynchronously save an object to a YAML file by using ``Yaml.WriteAsync(string fileLocation, object obj)``. This is more suitable for larger files or files on a network as it doesn't block the main thread.

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

            // Executes the entire method asynchronously (converting to YAML and writing the text file).
            await Yaml.WriteAsync(path, playerData);

            // Only writing the text file is executed asynchronously.
            Yaml.WriteAsync(path, playerData);
        }
    }

Creating objects from YAML files
--------------------------------
**Synchronous Example:**

To create an object from a YAML file use ``Yaml.Read<T>(string fileLocation)``. This reads the text from the file at the given ``fileLocation`` as YAML, creates an object out of it, casts the object as the given type ``T`` and returns it.

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
            // Creates a PlayerData object from the YAML file at 'path'.
            var playerData = Polaris.IO.Yaml.Read<PlayerData>(path);
        }
    }


If the file at the given ``fileLocation`` can not create an object of the given type ``T``, an exception may be thrown or a ``default(T)`` object will be returned.

**Synchronous Try Example:**

A safer way of creating an object from a YAML file is to use ``Yaml.TryRead<T>(string fileLocation, out T obj)``. This will catch and swallow all exceptions, as well as always providing you with a ``default(T)`` if an exception is caught. 

Unlike ``Yaml.Read<T>``, which returns the object, ``Yaml.TryRead<T>`` returns a boolean and uses an ``out`` parameter to provide you with the object. The return value indicates whether an object has been successfully created from the YAML file or not.

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
            // Attempts to create a PlayerData object from the YAML file at 'path'.
            bool success = Yaml.TryRead<PlayerData>(path, out var playerData);

            // If the conversion was unsuccessful, create a new 'PlayerData'.
            if (success == false)
                playerData = new PlayerData();
        }
    }


**Asynchronous Example:**

Alternatively, you can asynchronously create an object from a YAML file by using ``Yaml.ReadAsync<T>(string fileLocation)``. This is more suitable for larger files or files on a network as it doesn't block the main thread.

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
            // Asynchronously creates a PlayerData object from the YAML file at 'path'.
            var playerData = await Yaml.ReadAsync<PlayerData>(path);
        }
    }


Creating objects from YAML-formatted strings
--------------------------------------------
You don't need to read text directly from a file to create an object from YAML. You can also create an object directly from a YAML-formatted string using ``Yaml.ReadString<T>(string yamlString)``. This is useful if you have an alternate method to retrieve the text file.

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
            string yamlString = System.IO.File.ReadAllText(path);

            // Creates a PlayerData object from the given YAML-formatted string.
            var playerData = Yaml.ReadString<PlayerData>(yamlString);
        }
    }


Choosing a naming convention
----------------------------
By default, all fields are written and read using the Pascal Case naming convention. You can overload certain methods with an ``INamingConvention`` if you wish to use a different naming convention.

The methods that can be overloaded with an ``INamingConvention`` include:

- ``Yaml.Write(string fileLocation, object obj, INamingConvention namingConvention)``
- ``Yaml.TryWrite(string fileLocation, object obj, INamingConvention namingConvention)``
- ``Yaml.WriteAsync(string fileLocation, object obj, INamingConvention namingConvention)``
- ``Yaml.Read<T>(string fileLocation, INamingConvention namingConvention)``
- ``Yaml.TryRead<T>(string fileLocation, INamingConvention namingConvention, out T obj)``
- ``Yaml.ReadAsync<T>(string fileLocation, INamingConvention namingConvention)``
- ``Yaml.ReadString<T>(string yamlString, INamingConvention namingConvention)``

The naming conventions you can use include:

- ``PascalCaseNamingConvention`` (PlayerName)
- ``CamelCaseNamingConvention`` (playerName)
- ``UnderscoredNamingConvention`` (player_name)
- ``HyphenatedNamingConvention`` (player-name)
- ``LowerCaseNamingConvention`` (playername)

.. note:: The naming convention used to read the file must match the naming convention used to write the file.

**Camel Case Example:**

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
            playerData.Username = "James";
            playerData.Kills = 125;
            playerData.Exp = 777;

            // Creates a YAML file at 'path', containing the fields of 'playerData', with the Camel Case naming convention.
            Polaris.IO.Yaml.Write(path, playerData, new CamelCaseNamingConvention());

            // Contents of the save.data file.
            // playerName: James
            // kills: 125
            // exp: 777
        }
    }