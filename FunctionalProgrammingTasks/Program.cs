using System;
using System.Collections.Generic;
using System.Linq;

/*
Functional Programming Tasks Implementation:

FP1: GroupBy Implementation
- Custom implementation of GroupBy extension method
- Groups elements of a collection by a specified key selector
- Returns Dictionary with keys and corresponding lists of elements


FP2: Server Load Analysis
- Analyzes server load by day of week using GroupBy
- Calculates average request count for each day
- Demonstrates practical usage of GroupBy functionality


FP3: Sequence Generation and Control
- Generate method: Creates infinite sequence using provided generator function
- TakeUntil method: Takes elements from sequence until condition is met
- Practical examples: Reading strings until empty line and double empty line

FP4: Error Handling Utilities
- WithRetry implementation: Executes function with retry logic
- Demonstrates functional approach to error handling
- Features exponential backoff between retries
*/

namespace FunctionalProgrammingTasks
{
    public static class Extensions
    {
        // FP1: GroupBy implementation
        public static Dictionary<TKey, List<TSource>> GroupBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var result = new Dictionary<TKey, List<TSource>>();
            foreach (var item in source)
            {
                var key = keySelector(item);
                if (!result.ContainsKey(key))
                    result[key] = new List<TSource>();
                result[key].Add(item);
            }
            return result;
        }

        // FP2: Server load analysis
        public static Dictionary<DayOfWeek, double> AnalyzeServerLoad(
            IEnumerable<(DateTime Date, int RequestCount)> yearLog)
        {
            var groupedByDayOfWeek = yearLog.GroupBy(day => day.Date.DayOfWeek);
            
            var result = new Dictionary<DayOfWeek, double>();
            foreach (var group in groupedByDayOfWeek)
            {
                var averageLoad = group.Value.Average(day => day.RequestCount);
                result[group.Key] = averageLoad;
            }
            return result;
        }

        // FP3: Generate and TakeUntil implementations
        public static IEnumerable<T> Generate<T>(Func<T> makeNext)
        {
            while (true)
                yield return makeNext();
        }

        public static IEnumerable<T> TakeUntil<T>(
            this IEnumerable<T> source,
            Func<T, bool> shouldStop)
        {
            foreach (var item in source)
            {
                yield return item;
                if (shouldStop(item))
                    yield break;
            }
        }

        // Extension method for calculating average
        public static double Average<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            int sum = 0;
            int count = 0;
            foreach (var item in source)
            {
                sum += selector(item);
                count++;
            }
            return count > 0 ? (double)sum / count : 0;
        }

        // FP5: Циклический сдвиг массива влево
        public static IEnumerable<T> ShiftLeft<T>(this IEnumerable<T> source, int k)
        {
            var array = source.ToArray();
            int n = array.Length;
            k = k % n; // Нормализация k
            return array.Skip(k).Concat(array.Take(k));
        }
    }

    class Program
    {
        static void Main()
        {
            // Example of reading strings until empty line
            Console.WriteLine("Enter strings (empty line to stop):");
            var lines = Extensions.Generate(Console.ReadLine)
                .TakeUntil(line => line == "");

            foreach (var line in lines)
                Console.WriteLine($"You entered: {line}");

            // Example of reading strings until double empty line
            Console.WriteLine("\nEnter strings (double empty line to stop):");
            bool wasEmpty = false;
            var lines2 = Extensions.Generate(Console.ReadLine)
                .TakeUntil(line =>
                {
                    bool shouldStop = line == "" && wasEmpty;
                    wasEmpty = line == "";
                    return shouldStop;
                });

            foreach (var line in lines2)
                Console.WriteLine($"You entered: {line}");

            // FP4: Example of utility methods
            // Error handling example
            static T WithRetry<T>(Func<T> action, int maxAttempts = 3)
            {
                Exception lastException = null;
                for (int i = 0; i < maxAttempts; i++)
                {
                    try
                    {
                        return action();
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        System.Threading.Thread.Sleep(1000 * i); // Exponential backoff
                    }
                }
                throw new Exception($"Failed after {maxAttempts} attempts", lastException);
            }

            // Example usage of WithRetry
            try
            {
                var result = WithRetry(() =>
                {
                    // Simulate some operation that might fail
                    if (Random.Shared.Next(2) == 0)
                        throw new Exception("Random failure");
                    return "Success";
                });
                Console.WriteLine($"\nRetry result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFinal error: {ex.Message}");
            }
        }
    }
}
