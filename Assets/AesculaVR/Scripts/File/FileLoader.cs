using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A static class that that can be used to load in IFiles.
/// </summary>
public static class FileLoader
{
    /// <summary>
    /// Does the paths extension match the extension we want?
    /// </summary>
    /// <param name="path">The path we want to check.</param>
    /// <param name="extension">the extension we want</param>
    /// <returns>true if the paths extension matches the extension we give, otherwise false.</returns>
    private static bool DoesPathExtensionMatch(string path, string extension) => string.Equals(System.IO.Path.GetExtension(path), extension, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Get all of the files in a directory.
    /// </summary>
    /// <param name="directory">The directory we want to get the files from.</param>
    /// <returns>A new list filled with File objects.</returns>
    public static List<IFile> GetFiles(string directory) => GetFiles(directory, string.Empty);

    /// <summary>
    /// Get all of the files in a directory with an extension.
    /// </summary>
    /// <param name="directory">The directory we want to get the files from.</param>
    /// <param name="extension">The extension we want the files to have.</param>
    /// <returns>A new list filled with File objects.</returns>
    public static List<IFile> GetFiles(string directory, string extension)
    {
        if (directory == string.Empty)
            throw new System.IO.FileLoadException("Unable to load files from an empty directory");

        if(!System.IO.Directory.Exists(directory))
            throw new System.IO.FileLoadException("Unable to load files from a directory ["+directory+"] that does not exist.");

        string[] paths = System.IO.Directory.GetFiles(directory,"*"+ extension);
        List<IFile> files = new List<IFile>(paths.Length + 1);

        for (int i = 0; i < paths.Length; i++)
        {
            if((extension != string.Empty) && DoesPathExtensionMatch(paths[i], extension))
                files.Add(new File(paths[i]));
        }

        return files;
    }

}
