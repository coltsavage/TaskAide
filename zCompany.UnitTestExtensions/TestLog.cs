using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace zCompany.UnitTestExtensions
{
    public static class TestLog
    {
        // Class Properties
        public static ITestOutputHelper Output { get; set; }

        // Class Methods
        public static void OutputCollection(string header, IReadOnlyCollection<string> collection)
        {
            TestLog.WriteLine($"{header}:");
            foreach (var i in collection)
            {
                TestLog.WriteLine($"    {i}");
            }
            if (collection.Count == 0)
            {
                TestLog.WriteLine($"    <none>");
            }
        }

        public static void OutputCollection<T>(IReadOnlyCollection<T> collection)
        {
            TestLog.OutputCollection<T>($"{typeof(T).Name}s", collection);
        }

        public static void OutputCollection<T>(string header, IReadOnlyCollection<T> collection)
        {
            TestLog.WriteLine($"{header}:");
            foreach (T i in collection)
            {
                TestLog.WriteLine($"    {i}");
            }
            if (collection.Count == 0)
            {
                TestLog.WriteLine($"    <none>");
            }
        }

        public static void WriteLine(string message)
        {
            TestLog.Output.WriteLine(message);
        }
    }
}
