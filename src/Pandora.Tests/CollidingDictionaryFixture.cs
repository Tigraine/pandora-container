using System.Collections.Generic;
using Xunit;

namespace Pandora.Tests
{
    public class CollidingDictionaryFixture
    {
        [Fact]
        public void CanAddAndRetrieveValue()
        {
            var dictionary = new CollidingDictionary<int, string>();
            dictionary.Add(1, "test");
            var values = dictionary.Get(1);
            Assert.Equal(1, values.Count);
            Assert.Equal("test", values[0]);
        }

        [Fact]
        public void CanAddMultipleValuesPerKey()
        {
            var dictionary = new CollidingDictionary<int, string>();
            dictionary.Add(1, "test");
            dictionary.Add(1, "test2");

            var list = dictionary.Get(1);
            Assert.Equal(2, list.Count);
            Assert.Contains("test", list);
            Assert.Contains("test2", list);
        }

        [Fact]
        public void OrderOfInsertionGetsPreserved()
        {
            var dictionary = new CollidingDictionary<int, int>();
            dictionary.Add(1, 1);
            dictionary.Add(1, 2);
            dictionary.Add(1, 3);

            var list = dictionary.Get(1);
            Assert.Equal(1, list[0]);
            Assert.Equal(2, list[1]);
            Assert.Equal(3, list[2]);
        }

        [Fact]
        public void NotFoundKeyResultsInKeyNotFoundException()
        {
            var dictionary = new CollidingDictionary<int, int>();
            Assert.Throws<KeyNotFoundException>(() => dictionary.Get(1));
        }

        [Fact]
        public void ContainsKeyCanBeFalse()
        {
            var dictionary = new CollidingDictionary<int, int>();
            var result = dictionary.ContainsKey(1);
            Assert.False(result); 
        }

        [Fact]
        public void ContainsKeyCanBeTrue()
        {
            var dictionary = new CollidingDictionary<int, int>();
            dictionary.Add(1, 1);
            var result = dictionary.ContainsKey(1);
            Assert.True(result);
        }
    }
}