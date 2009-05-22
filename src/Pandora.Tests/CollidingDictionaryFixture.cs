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
    }
}