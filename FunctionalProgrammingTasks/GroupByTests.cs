using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalProgrammingTasks.Tests
{
    [TestFixture]
    public class GroupByTests
    {
        [Test]
        public void GroupBy_WithSimpleIntegerList_GroupsCorrectly()
        {
            // Arrange
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };

            // Act
            var result = numbers.GroupBy(n => n % 2 == 0);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[true].Count, Is.EqualTo(3)); // Even numbers
            Assert.That(result[false].Count, Is.EqualTo(3)); // Odd numbers
        }

        [Test]
        public void GroupBy_WithEmptyList_ReturnsEmptyDictionary()
        {
            // Arrange
            var emptyList = new List<string>();

            // Act
            var result = emptyList.GroupBy(s => s.Length);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GroupBy_WithStrings_GroupsByLength()
        {
            // Arrange
            var words = new List<string> { "a", "ab", "abc", "abcd", "a" };

            // Act
            var result = words.GroupBy(s => s.Length);

            // Assert
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[1].Count, Is.EqualTo(2)); // Words of length 1
            Assert.That(result[2].Count, Is.EqualTo(1)); // Words of length 2
            Assert.That(result[3].Count, Is.EqualTo(1)); // Words of length 3
            Assert.That(result[4].Count, Is.EqualTo(1)); // Words of length 4
        }
    }
}