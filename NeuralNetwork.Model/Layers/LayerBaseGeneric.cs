using NeuralNetwork.Model.Nodes;
using System.Collections.Generic;

namespace NeuralNetwork.Model.Layers
{
    public abstract class LayerBase<TNode> : LayerBase where TNode : NodeBase
    {
        private readonly IDictionary<string, TNode> _nodes;
        internal LayerBase(string id) : base(id)
        {
            _nodes = new Dictionary<string, TNode>();
        }
        public IEnumerable<TNode> Nodes { get => _nodes.Values; }

        public void AddNode(TNode node)
        {
            SetNodeToLayer(node);
            _nodes.Add(node.Id, node);

            ConnectNodeToNextLayer(node);
            AddNodeChild(node);
        }

        public void RemoveNode(TNode node)
        {
            _nodes.Remove(node.Id);

            DisconnectNodeFromNextLayer(node);
            RemoveNodeChild(node);
        }

        public override void RemoveNode(string nodeId)
        {
            if (!_nodes.TryGetValue(nodeId, out TNode node))
                return;

            RemoveNode(node);
        }

        public override IEnumerable<NodeBase> GetAllNodes()
        {
            var nodes = new List<NodeBase>(_nodes.Values);

            if (this.Bias != null)
                nodes.Add(this.Bias);

            return nodes;
        }

        private protected override void ConnectChild()
        {
            for (NeuronLayer layer = this.Next; layer != null; layer = layer.Next)
            {
                layer.RemoveEdgesLayer();

                foreach (var node in layer.Previous.GetAllNodes())
                {
                    ConnectNodeToNextLayer(node, layer);
                }
            }
        }

        private protected virtual void AddNodeChild(TNode node) { }
        private protected virtual void RemoveNodeChild(TNode node) { }
    }
}
