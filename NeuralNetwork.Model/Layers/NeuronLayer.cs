using NeuralNetwork.Model.Nodes;

namespace NeuralNetwork.Model.Layers
{
    public class NeuronLayer : LayerBase<Neuron>
    {
        public NeuronLayer(string id) : base(id)
        {

        }

        public LayerBase Previous { get; internal set; }

        public void Disconnect()
        {
            if (this.Previous != null)
            {
                if (this.Next != null)
                {
                    this.Previous.Next = this.Next;
                    this.Next.Previous = this.Previous;
                    this.Previous.Reconnect();
                }
                else
                {
                    this.Previous.Next = null;
                }
            }
            else
            {
                if (this.Next != null)
                {
                    this.Next.Previous = null;
                    this.Next.RemoveEdgesLayer();
                }
            }

            RemoveEdgesLayer();
            this.Previous = null;
            this.Next = null;
        }

        internal void RemoveEdgesLayer()
        {
            foreach (var node in this.Nodes)
            {
                node.EdgesInternal.Clear();
            }
        }

        private protected override void AddNodeChild(Neuron neuron)
        {
            ConnectNeuronToPreviousLayer(neuron);
        }

        private protected override void RemoveNodeChild(Neuron neuron)
        {
            neuron.EdgesInternal.Clear();
        }

        private void ConnectNeuronToPreviousLayer(Neuron neuron)
        {
            neuron.EdgesInternal.Clear();

            if (this.Previous == null)
                return;

            foreach (var previousNode in this.Previous.GetAllNodes())
            {
                neuron.EdgesInternal.Add(Edge.Create(previousNode, neuron));
            }
        }
    }
}
