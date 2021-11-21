using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Tools.Math {
    class Graph<T> {
        List<GraphNode<T>> _Nodes = new List<GraphNode<T>>();

        public int Count => _Nodes.Count;

        public IList<GraphNode<T>> Nodes {
            get { return _Nodes.AsReadOnly(); }
        }

        public void Clear() {
            foreach(var node in _Nodes) {
                node.RemoveAllNeighbors();
            }
            _Nodes.RemoveAll(_ => true);
        }

        public GraphNode<T> Find(T value) {
            return _Nodes.FirstOrDefault(_ => _.Value.Equals(value));
        }

        public bool AddNode(T value) {
            if(Find(value) != null) {
                return false;
            }
            else {
                _Nodes.Add(new GraphNode<T>(value));
                return true;
            }
        }

        public bool AddEdge(T value1, T value2) {
            var node1 = Find(value1);
            var node2 = Find(value2);
            if (node1 == null || node2 == null)
                return false;
            else if (node1.Neighbors.Contains(node2)) {
                return false;
            }
            else {
                node1.AddNeighbor(node2);
                node2.AddNeighbor(node1);
                return true;
            }
        }

        public bool RemoveNode(T value) {
            var removeNode = Find(value);
            if (removeNode == null)
                return false;
            else {
                _Nodes.Remove(removeNode);
                _Nodes.ForEach(_ => _.RemoveNeighbor(removeNode));
                return true;
            }
        }

        public bool RemoveEdge(T value1, T value2) {
            var node1 = Find(value1);
            var node2 = Find(value2);
            if (node1 == null || node2 == null)
                return false;
            else if (!node1.Neighbors.Contains(node2)) {
                return false;
            }
            else {
                node1.RemoveNeighbor(node2);
                node2.RemoveNeighbor(node1);
                return true;
            }
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < Count; i++) {
                builder.Append(_Nodes[i].ToString());
                if(i < Count - 1) {
                    builder.Append(",");
                }
            }
            return builder.ToString();
        }
    }
}
