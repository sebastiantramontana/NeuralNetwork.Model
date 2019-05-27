using NeuralNetwork.Model.Layers;

namespace NeuralNetwork.Model.Nodes
{
    public abstract class NodeBase : Element
    {
        public NodeBase(string id) : base(id)
        {

        }
        public double? OutputValue { get; set; }
        public LayerBase Layer { get; internal set; }
    }
}
