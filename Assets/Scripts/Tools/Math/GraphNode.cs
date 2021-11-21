using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Tools.Math {
    class GraphNode<T> {
        private T _Value;
        private List<GraphNode<T>> _Neighbors;

        public GraphNode(T value) {
            this._Value = value;
            _Neighbors = new List<GraphNode<T>>();
        }

        public T Value { get => this._Value; }
        public IList<GraphNode<T>> Neighbors { get => this._Neighbors.AsReadOnly(); }

        public bool AddNeighbor(GraphNode<T> neighbor) {
            if (_Neighbors.Contains(neighbor)) {
                return false;
            }
            else {
                _Neighbors.Add(neighbor);
                return true;
            }
        }

        public bool RemoveNeighbor(GraphNode<T> neighbor) {
            return _Neighbors.Remove(neighbor);
        }

        public bool RemoveAllNeighbors() {
            _Neighbors.RemoveAll(_ => true);
            return true;
        }

        public override string ToString() {
            return $"Node Value: {_Value}, Neighbors: {_Neighbors.ToString()}";
        }
    }
}
