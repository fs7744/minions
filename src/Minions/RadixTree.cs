using System.Collections;
using System.Xml.Linq;

namespace Minions
{
    public class RadixTree<V> : IEnumerable<Node<V>>
    {
        private readonly Node<V> root;

        public RadixTree()
        {
            root = new Node<V>();
        }

        public void AddNode(string term, V value)
        {
            ArgumentNullException.ThrowIfNull(term, nameof(term));
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            AddNode(root, term, value);
        }

        private void AddNode(Node<V> node, string term, V value)
        {
            // first
            if (node.Childrens == null)
            {
                node.Childrens = new List<Node<V>>() { new Node<V>() { Term = term, Value = value } };
            }
            else 
            {
                foreach (var child in node.Childrens) 
                {
                    var key = child.Term;
                    int common = 0;
                    for (int i = 0; i < Math.Min(term.Length, key.Length); i++) if (term[i] == key[i]) common = i + 1; else break;
                    if (common > 0)
                    {

                    }
                    else // no common prefix
                    {
                        node.Childrens.Add(new Node<V>() { Term = term, Value = value });
                    }
                }
            }
        }

        public IEnumerator<Node<V>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }
}