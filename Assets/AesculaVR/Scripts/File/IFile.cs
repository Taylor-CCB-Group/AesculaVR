using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Then interface for every file
/// </summary>
public interface IFile
{
  
    /// <summary>
    /// The name of the file.
    /// </summary>
    /// <param name="withExtension">does the name include the file extension. e.g. ".txt"</param>
    /// <returns>The files name.</returns>
    string Name(bool withExtension = true);    

    /// <summary>
    /// The absolute path of the file.
    /// </summary>
    /// <param name="withName">Do we want the filename (with extension) included?</param>
    /// <returns>the absolute path of the file.</returns>
    string Path(bool withName = true);

    /// <summary>
    /// The file's extension, e.g. ".txt"
    /// </summary>
    /// <returns>The files extension</returns>
    string Extension();
    

    /// <summary>
    /// When was the file created?
    /// </summary>
    /// <returns>The DateTime for when the file was created </returns>
    DateTime Created ();
    /// <summary>
    ///  When was the file last modified?
    /// </summary>
    /// <returns>The DateTime for when the file was last modiefied</returns>
    DateTime Modified();
    /// <summary>
    /// When was the file last accessed?
    /// </summary>
    /// <returns>The DateTime for when the file was last accessed</returns>
    DateTime Accessed();


    void SetLastAccessTime(DateTime dateTime);
    void SetLastAccessTime();

}
