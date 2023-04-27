namespace Minions
{
    public class Node<V>
    {
        public string Term { get; set; }
        public List<Node<V>> Childrens { get; set; }

        public V? Value { get; set; }
    }
}