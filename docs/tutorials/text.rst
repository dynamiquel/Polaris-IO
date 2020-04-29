.. _doc_tutorials_text:

Text
====
Introduction
------------
Not yet implemented.

Saving strings to text files
----------------------------
**Synchronous Example:**

To save a string to a text file use ``Text.Write(string fileLocation, string content)``. This writes the string to a file at the given ``fileLocation``.

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
            string playerName = "James";

            // Writes the 'playerName' string to a text file at 'path'.
            Polaris.IO.Text.Write(path, playerName);
  
            // Contents of the save.data file.
            // James
        }
    }

**Asynchronous Example:**

Alternatively, you can asynchronously save a string to a text file by using ``Text.WriteAsync(string fileLocation, string content)``. This is more suitable for larger files or files on a network as it doesn't block the main thread.

::

    using Polaris.IO;

    public class GameSaver : MonoBehaviour
    {
        string path = Path.Combine(Application.persistentDataPath, "save.data");

        async void Start()
        {
            string playerName = "James";

            // Asynchronously writes the 'playerName' string to a text file at 'path'.
            await Text.WriteAsync(path, playerName);
        }
    }

Creating strings from text files
--------------------------------
**Synchronous Example:**

To create a string from a text file use ``Text.Read(string fileLocation)``. This reads the text from the file at the given ``fileLocation`` as a string and returns it.

::

    public class GameSaver : MonoBehaviour
    {
        string path = Path.Combine(Application.persistentDataPath, "save.data");

        void Start()
        {
            // Creates a string from the text file at 'path'.
            string playerName = Polaris.IO.Text.Read(path);
        }
    }


If a string cannot be loaded from the file at the given ``fileLocation``, an exception may be thrown or a ``string.Empty`` will be returned.

**Synchronous Try Example:**

A safer way of creating a string from a text file is to use ``Text.TryRead(string fileLocation, out string content)``. This will catch and swallow all exceptions, as well as always providing you with a ``string.Empty`` if an exception is caught. 

Unlike ``Text.Read``, which returns the string, ``Text.TryRead`` returns a boolean and uses an ``out`` parameter to provide you with the string. The return value indicates whether a string has been successfully created from the text file or not.

::

    public class GameSaver : MonoBehaviour
    {
        string path = Path.Combine(Application.persistentDataPath, "save.data");

        void Start()
        {
            // Attempts to create a string from the text file at 'path'.
            bool success = Text.TryRead(path, out var playerName);

            // If the conversion was unsuccessful, set 'playerName' to "New Player".
            if (success == false)
                playerName = "New Player";
        }
    }


**Asynchronous Example:**

Alternatively, you can asynchronously create a string from a text file by using ``Text.ReadAsync(string fileLocation)``. This is more suitable for larger files or files on a network as it doesn't block the main thread.

::

    using Polaris.IO;

    public class GameSaver : MonoBehaviour
    {
        string path = Path.Combine(Application.persistentDataPath, "save.data");

        async void Start()
        {
            // Asynchronously creates a string from the text file at 'path'.
            var playerName = await Text.ReadAsync(path);
        }
    }
