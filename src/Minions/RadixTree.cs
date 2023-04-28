using System.Collections.Generic;

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
                            child.Value ??= value;
                        }
                        //new is subkey
                        //existing abcd
                        //new      ab
                        //if new is shorter (== common), then node(count) and only 1. children add (clause2)
                        else if (common == term.Length)
                        {
                            var commonNode = new Node<V>() { Term = key[..common], Childrens = new List<Node<V>>() };
                            commonNode.Childrens.Add(new Node<V>() { Term = child.Term[common..], Childrens = child.Childrens, Value = child.Value });
                            commonNode.Value = value;
                            commonNode.Childrens = commonNode.Childrens.OrderByDescending(i => i.Term).ToList();
                            //commonNode.Childrens = commonNode.Childrens.OrderBy(i => i.Term).ToList();
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
                            //commonNode.Childrens = commonNode.Childrens.OrderBy(i => i.Term).ToList();
                            node.Childrens[j] = commonNode;
                        }
                        return;
                    }
                }

                node.Childrens.Add(new Node<V>() { Term = term, Value = value });
                node.Childrens = node.Childrens.OrderByDescending(i => i.Term).ToList();
                //node.Childrens = node.Childrens.OrderBy(i => i.Term).ToList();
            }
        }

        public Node<V>? Search(string value, Func<V, object, bool> find, object context)
        {
            ArgumentNullException.ThrowIfNull(find, nameof(find));
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            return Search(root, new Node<V>() { Term = value }, find, context);
        }

        private static BinarySearchComparer binarySearchComparer = new BinarySearchComparer();
        private class BinarySearchComparer : IComparer<Node<V>>
        {
            public int Compare(Node<V>? x, Node<V>? y)
            {
                //return Comparer<string>.Default.Compare(x.Term[..y.Term.Length], y.Term);
                return Comparer<string>.Default.Compare(x.Term, y.Term[..x.Term.Length]);
            }
        }

        private Node<V>? Search(Node<V> curr, Node<V> value, Func<V, object, bool> find, object context)
        {
            Node<V>? n = curr;
            //bool hasFirst = false;
            //if (curr.Childrens[0].Term == "") 
            //{
            //    n = curr.Childrens[0];
            //    hasFirst = true;
            //}
            //var index = hasFirst ? curr.Childrens.BinarySearch(1 , curr.Childrens.Count - 1, value, binarySearchComparer) : curr.Childrens.BinarySearch(value, binarySearchComparer);
            //if (index >= 0) { n = curr.Childrens[index]; }
            var index = curr.Childrens.BinarySearch(value, binarySearchComparer);
            if (index >= 0) { n = curr.Childrens[index]; }

            //foreach (var item in curr.Childrens)
            //{
            //    if (item.Term.Length <= value.Length)
            //    {
            //        var commonKey = value[..item.Term.Length];
            //        if (commonKey == item.Term)
            //        {
            //            n = item;
            //            break;
            //        }
            //    }
            //}
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
                value.Term = value.Term[n.Term.Length..];
                if (value.Term.Length == 0)
                {
                    return n.Value != null && find(n.Value, context) ? n : null;
                }
                else
                {
                    return Search(n, value, find, context);
                }
            }
        }
    }
}