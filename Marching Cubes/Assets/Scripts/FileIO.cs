using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// FileIO Class by Joseph Staria
/// Credit to use
/// </summary>

public static class FileIO
{
    // Not particularly useful for this project
    /// <summary>
    /// Initializes file path for writing, useful for created save files
    /// </summary>
    /// <param name="name">Name of document</param>
    public static void CreateTxtFile(string name)
    {
        string stream = "Assets/TileData/" + name + ".txt";

        StreamWriter fileWriter = new StreamWriter(stream);

        fileWriter.Close();
    }

    /// <summary>
    /// Overwrites document (string data)
    /// </summary>
    /// <param name="name">Name of document</param>
    /// <param name="data">List of strings</param>
    public static void WriteTo(string name, List<string> data)
    {
        string stream = "Assets/TileData/" + name + ".txt";

        try
        {
            StreamWriter fileWriter = new StreamWriter(stream, false);

            foreach (string item in data)
            {
                fileWriter.WriteLine(item);
            }

            fileWriter.Close();
        }
        catch
        {
            // oh well
            // This catch is specifically for the problem that I kept having with it not closing a document from what I assume is
            // too-fast of reading and writing and then opening before it can close, I haven't experienced any loss of save data since
            // implementing this, so I see it as a win
        }
    }

    /// <summary>
    /// Writes to the end of a document
    /// </summary>
    /// <param name="name">Name of document</param>
    /// <param name="data">Data being written to document</param>
    public static void AppendTo(string name, List<string> data)
    {
        string stream = "Assets/TileData/" + name + ".txt";

        StreamWriter fileWriter = new StreamWriter(stream, true);

        foreach (string item in data)
        {
            fileWriter.WriteLine(item);
        }

        fileWriter.Close();
    }

    /// <summary>
    /// Overwrites a document entirely (floats only)
    /// </summary>
    /// <param name="name">Name of document</param>
    /// <param name="data">Float Data being written to document</param>
    public static void WriteTo(string name, List<float> data)
    {
        string stream = "Assets/TileData/" + name + ".txt";

        StreamWriter fileWriter = new StreamWriter(stream);

        foreach (float item in data)
        {
            fileWriter.WriteLine(item);
        }

        fileWriter.Close();
    }

    /// <summary>
    /// Returns a list of string information from document 
    /// </summary>
    /// <param name="name">Name of document</param>
    /// <returns></returns>
    public static List<String> ReadFrom(string name)
    {
        List<String> data = new List<String>();

        string stream = "Assets/TileData/" + name + ".txt";

        StreamReader fileReader = new StreamReader(stream);

        string line = null;

        while ((line = fileReader.ReadLine()) != null)
        {
            data.Add(line);
        }

        if (fileReader != null)
        {
            fileReader.Close();
        }

        return data;
    }

    /// <summary>
    /// Returns a documents information that is trusted to be all floats
    /// </summary>
    /// <param name="name">name of document</param>
    /// <returns></returns>
    public static List<float> NumReadFrom(string name)
    {
        List<float> data = new List<float>();

        string stream = "Assets/TileData/" + name + ".txt";

        StreamReader fileReader = new StreamReader(stream);

        string line = null;

        while ((line = fileReader.ReadLine()) != null)
        {
            data.Add(float.Parse(line));
        }

        if (fileReader != null)
        {
            fileReader.Close();
        }

        return data;
    }
}
