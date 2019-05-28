using NeuralNetwork.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Model.Layers
{
    public abstract class LayerBase : Element
    {
        internal LayerBase(string id) : base(id)
        {

        }

        public NeuronLayer Next { get; internal set; }

        private Bias _Bias = null;
        public Bias Bias
        {
            get { return _Bias; }
            set
            {
                SetNewBias(value);
                _Bias = value;
            }
        }

        public void Connect(NeuronLayer nextLayer)
        {
            if (nextLayer == null) //Cut off the layer link
            {
                if (this.Next != null)
                {
                    this.Next.Previous = null; //cancel the Next's Previous layer (this) 
                }

                this.Next = null;
                return;
            }

            if (this.Next != null)
            {
                this.Next.Previous = nextLayer;
                nextLayer.Next = this.Next;
            }

            this.Next = nextLayer;
            nextLayer.Previous = this;

            ConnectChild();
        }

        internal void Reconnect()
        {
            ConnectChild();
        }

        protected void ConnectNodeToNextLayer(NodeBase node)
        {
            //Connect the edges... they are neurons
            ConnectNodeToNextLayer(node, this.Next);
        }

        protected void ConnectNodeToNextLayer(NodeBase previousNode, NeuronLayer nextLayer)
        {
            //Connect the edges...
            if (nextLayer == null)
                return;

            var nextNeuronss = nextLayer.Nodes; //they are neurons

            foreach (var nextNeuron in nextNeuronss)
            {
                nextNeuron.EdgesInternal.Add(Edge.Create(previousNode, nextNeuron));
            }
        }

        protected void DisconnectNodeFromNextLayer(NodeBase node)
        {
            //Disconnect the edges...
            if (this.Next == null || node == null)
                return;

            var nextNeurons = this.Next.Nodes;

            foreach (var nextNeuron in nextNeurons)
            {
                nextNeuron.EdgesInternal.Remove(nextNeuron.Edges.Single(e => e.Source == node));
            }
        }

        protected void SetNodeToLayer(NodeBase node)
        {
            if (node.Layer != null)
                throw new InvalidOperationException($"Node '{node.Id}' already belongs to another layer. Remove the node from the other layer, then add to this one.");

            node.Layer = this;
        }

        protected void SetNewBias(Bias bias)
        {
            if (bias == null)
            {

                DisconnectNodeFromNextLayer(this.Bias);
            }
            else
            {
                SetNodeToLayer(bias);
                ConnectNodeToNextLayer(bias);
            }
        }

        private protected override void ValidateDuplicatedIChild(IDictionary<string, Element> acumulatedIds)
        {
            foreach (var node in this.GetAllNodes())
                node.ValidateId(acumulatedIds);

            if (this.Next != null)
                this.Next.ValidateId(acumulatedIds);
        }

        public abstract IEnumerable<NodeBase> GetAllNodes();
        public abstract void RemoveNode(string nodeId);

        private protected abstract void ConnectChild();

    }
}
