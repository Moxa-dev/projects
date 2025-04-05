using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalProgrammingTasks.Tests
{
    [TestFixture]
    public class ShiftLeftTests
    {
        [Test]
        public void ShiftLeft_WithPositiveK_ShiftsCorrectly()
        {
            // Arrange
            var numbers = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = numbers.ShiftLeft(2).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(new[] { 3, 4, 5, 1, 2 }));
        }

        [Test]
        public void ShiftLeft_WithKEqualToLength_ReturnsSameArray()
        {
            // Arrange
            var numbers = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = numbers.ShiftLeft(5).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(numbers));
        }

        [Test]
        public void ShiftLeft_WithKGreaterThanLength_ShiftsCorrectly()
        {
            // Arrange
            var numbers = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = numbers.ShiftLeft(7).ToArray(); // Equivalent to k=2

            // Assert
            Assert.That(result, Is.EqualTo(new[] { 3, 4, 5, 1, 2 }));
        }

        [Test]
        public void ShiftLeft_WithEmptyArray_ReturnsEmptyArray()
        {
            // Arrange
            var empty = Array.Empty<int>();

            // Act
            var result = empty.ShiftLeft(3).ToArray();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ShiftLeft_WithStrings_ShiftsCorrectly()
        {
            // Arrange
            var strings = new[] { "a", "b", "c", "d" };

            // Act
            var result = strings.ShiftLeft(1).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(new[] { "b", "c", "d", "a" }));
        }
    }
}