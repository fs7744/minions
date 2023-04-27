using Minions;

namespace ut
{
    public class RadixTreeUT
    {
        [Fact]
        public void SubPathMatch()
        {
            var t = new RadixTree<string>();
            t.AddNode("/ab/cd/ef", "/ab/cd/ef");
            t.AddNode("/ab/c", "/ab/c");
            Assert.Equal("/ab/c", t.Search("/ab/c", (k, o) => true, null)?.Value);
            Assert.Equal("/ab/cd/ef", t.Search("/ab/cd/ef", (k, o) => true, null)?.Value);
        }
    }
}