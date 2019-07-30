using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to sort IFiles.
/// </summary>
public static class FileSorter
{
    /// <summary>
    /// The selection of fields we can sort IFiles by.
    /// </summary>
    public enum SortField { Name, Created, Accessed, Modified }

    /// <summary>
    /// The Directions we can sort in.
    /// </summary>
    public enum SortDirection { Ascending, Descending }

    /// <summary>
    /// Creates a copy of files, thats sorted by the field and direction passed in.
    /// </summary>
    /// <param name="files">The list of files we want to sort</param>
    /// <param name="sortBy"> the field we want to sort by.</param>
    /// <param name="sortDirection">the direction of the sort.</param>
    /// <returns></returns>
    public static List<IFile> SortFiles(List<IFile> files, SortField sortBy, SortDirection sortDirection)
    {
        List<IFile> sorted;

        switch (sortBy)
        {
            case SortField.Name:
                sorted = files.OrderBy(file => file.Name()).ToList();
                break;
            case SortField.Modified:
                sorted = files.OrderBy(file => file.Modified()).ToList();
                break;
            case SortField.Created:
                sorted = files.OrderBy(file => file.Created()).ToList();
                break;
            case SortField.Accessed:
                sorted = files.OrderBy(file => file.Accessed()).ToList();
                break;
            default:
                throw new System.NotImplementedException();
        }

        if(sortDirection == SortDirection.Descending)
        {
            sorted.Reverse();
        }

        return sorted;
    }

}
