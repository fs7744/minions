namespace Minions
{
    public class RadixTree<V>
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
                for (int j = 0; j < node.Childrens.Count; j++)
                {
                    var child = node.Childrens[j];
                    var key = child.Term;
                    int common = 0;
                    for (int i = 0; i < Math.Min(term.Length, key.Length); i++) if (term[i] == key[i]) common = i + 1; else break;
                    if (common > 0)
                    {
                        if (common == term.Length && common == key.Length) // same
                        {
                        }
                        //new is subkey
                        //existing abcd
                        //new      ab
                        //if new is shorter (== common), then node(count) and only 1. children add (clause2)
                        else if (common == term.Length)
                        {
                            var commonNode = new Node<V>() { Term = key[..common], Childrens = new List<Node<V>>() };
                            commonNode.Childrens.Add(new Node<V>() { Term = child.Term[common..], Childrens = child.Childrens, Value = child.Value });
                            commonNode.Childrens.Add(new Node<V>() { Term = term[common..], Value = value });
                            commonNode.Childrens = commonNode.Childrens.OrderByDescending(i => i.Term).ToList();
                            node.Childrens[j] = commonNode;
                        }
                        //if oldkey shorter (==common), then recursive addTerm (clause1)
                        //existing: te
                        //new:      test
                        else if (common == key.Length)
                        {
                            AddNode(child, term[common..], value);
                        }
                        //old and new have common substrings
                        //existing: test
                        //new:      team
                        else
                        {
                            var commonNode = new Node<V>() { Term = key[..common], Childrens = new List<Node<V>>() };
                            commonNode.Childrens.Add(new Node<V>() { Term = child.Term[common..], Childrens = child.Childrens, Value = child.Value });
                            commonNode.Childrens.Add(new Node<V>() { Term = term[common..], Value = value });
                            commonNode.Childrens = commonNode.Childrens.OrderByDescending(i => i.Term).ToList();
                            node.Childrens[j] = commonNode;
                        }
                        return;
                    }
                }

                node.Childrens.Add(new Node<V>() { Term = term, Value = value });
                node.Childrens = node.Childrens.OrderByDescending(i => i.Term).ToList();
            }
        }

        public Node<V>? Search(string value, Func<V, object, bool> find, object context)
        {
            ArgumentNullException.ThrowIfNull(find, nameof(find));
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            return Search(root, value, find, context);
        }

        private Node<V>? Search(Node<V> curr, string value, Func<V, object, bool> find, object context)
        {
            Node<V>? n = null;
            foreach (var item in curr.Childrens)
            {
                if (item.Term.Length <= value.Length)
                {
                    var commonKey = value[..item.Term.Length];
                    if (commonKey == item.Term)
                    {
                        n = item;
                        break;
                    }
                }
            }
            if (n == null)
            {
                return null;
            }
            else if (n.Childrens == null || n.Childrens.Count == 0)
            {
                return n.Value != null && find(n.Value, context) ? n : null;
            }
            else
            {
                return Search(n, value[n.Term.Length..], find, context);
            }
        }
    }
}