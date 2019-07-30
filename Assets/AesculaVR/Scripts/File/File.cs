using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File : IFile
{

    private DateTime accessed;
    private DateTime created;
    private DateTime modified;
    
    private string path;

    public File(string path)
    {
        if (!System.IO.File.Exists(path))
            throw new System.IO.FileNotFoundException("Unable to create file object for a file that does not exist", path);

        this.path = path;


        this.accessed = System.IO.File.GetLastAccessTime(path);
        this.created  = System.IO.File.GetCreationTime  (path);
        this.modified = System.IO.File.GetLastWriteTime (path);
    }

    public DateTime Accessed() => accessed;

    public DateTime Created() => created;

    public DateTime Modified() => modified;


    public string Name(bool withExtension = true)
    {
        if (withExtension)
            return System.IO.Path.GetFileName(path);
        else
            return System.IO.Path.GetFileNameWithoutExtension(path);
    }

    public string Path(bool withName = true)
    {
        if (withName)
            return System.IO.Path.GetFullPath(path);
        else
            return System.IO.Path.GetDirectoryName(this.Path(true));
    }
}
