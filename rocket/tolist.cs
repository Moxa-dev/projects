using System;
using System.Collections.Generic;

// Define a Student class with properties LastName and Group
public class Student
{
    public string LastName { get; set; }
    public string Group { get; set; }
}

// Define extension methods for IEnumerable
public static class IEnumerableExtensions
{
    // Extension method for filtering elements based on a predicate
    public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        foreach (var e in enumerable)
            if (predicate(e))
                yield return e;
    }

    // Extension method for projecting elements into a new form
    public static IEnumerable<TOut> Select<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> selector)
    {
        foreach (var e in enumerable)
            yield return selector(e);
    }

    // Extension method for converting IEnumerable to List
    public static List<T> ToList<T>(this IEnumerable<T> enumerable)
    {
        var list = new List<T>();
        foreach (var e in enumerable)
            list.Add(e);
        return list;
    }

    // Extension method for skipping a specified number of elements
    public static IEnumerable<T> Skip<T>(this IEnumerable<T> enumerable, int count)
    {
        int skipped = 0;
        foreach (var e in enumerable)
        {
            if (skipped++ >= count)
                yield return e;
        }
    }

    // Extension method for taking a specified number of elements
    public static IEnumerable<T> Take<T>(this IEnumerable<T> enumerable, int count)
    {
        int taken = 0;
        foreach (var e in enumerable)
        {
            if (taken++ < count)
                yield return e;
            else
                yield break;
        }
    }
}

public class Program
{
    public static void Main()
    {
        // Create a list of Student objects
        var students = new List<Student>
        {
            new Student { LastName="Jones", Group="FT-1" },
            new Student { LastName="Adams", Group="FT-1" },
            new Student { LastName="Williams", Group="KN-1"},
            new Student { LastName="Brown", Group="KN-1"}
        };

        // Use extension methods to filter, select, skip, take, and convert to list
        var result = students.Where(z => z.Group == "KN-1").Select(z => z.LastName).Skip(1).Take(1).ToList();

        // Print the last names of students in group KN-1
        foreach (var lastName in result)
        {
            Console.WriteLine(lastName);
        }
    }
}