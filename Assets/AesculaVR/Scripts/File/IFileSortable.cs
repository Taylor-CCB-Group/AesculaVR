using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface for objects that contains a list of IFiles that can be sorted.
/// </summary>
public interface IFileSortable
{
    /// <summary>
    /// Set what field that the Ifiles will be sorted by.
    /// </summary>
    /// <param name="value">What do we want to the files to be sorted by?</param>
    void SetSortBy       (FileSorter.SortBy        value);
    /// <summary>
    /// Set the direction of the Sort
    /// </summary>
    /// <param name="value">The direction of the sort</param>
    void SetSortDirection(FileSorter.SortDirection value);

    /// <summary>
    /// Get the field The IFiles are being sorted by
    /// </summary>
    /// <returns>The field the IFiles are being sorted by. </returns>
    FileSorter.SortBy        GetSortBy();
    /// <summary>
    /// Get the direction of the sort.
    /// </summary>
    /// <returns>The direction of the sort.</returns>
    FileSorter.SortDirection GetSortDirection();
    
}
