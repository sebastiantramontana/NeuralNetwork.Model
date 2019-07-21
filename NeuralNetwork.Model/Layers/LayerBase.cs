using NeuralNetwork.Model.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NeuralNetwork.Model.Layers
{
   public abstract class LayerBase : Element
   {
      internal LayerBase(string id) : base(id)
      {

      }

      public NeuronLayer Next { get; internal set; }

      private Bias _bias = null;
      public Bias Bias
      {
         get { return _bias; }
         set
         {
            if (_bias != null)
               _bias.PropertyChanged -= Bias_PropertyChanged;

            SetNewBias(value);
            ChangeProperty(ref _bias, value);

            if (_bias != null)
               _bias.PropertyChanged += Bias_PropertyChanged;
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

            FireChanges("Next");
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

         FireChanges("Next");
         nextLayer.PropertyChanged += NextLayer_PropertyChanged;
      }

      internal void Reconnect()
      {
         ConnectChild();
      }

      internal void UnsubscribeChangesNextLayer()
      {
         if (this.Next != null)
         {
            this.Next.PropertyChanged -= NextLayer_PropertyChanged;
         }
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
            var edge = Edge.Create(previousNode, nextNeuron);
            edge.PropertyChanged += Edge_PropertyChanged;

            nextNeuron.EdgesInternal.Add(edge);
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
            var edge = nextNeuron.Edges.Single(e => e.Source == node);
            edge.PropertyChanged -= Edge_PropertyChanged;

            nextNeuron.EdgesInternal.Remove(edge);
         }
      }

      protected void SetNodeToLayer(NodeBase node)
      {
         if (node.Layer != null)
            throw new InvalidOperationException($"Node '{node.Id}' already belongs to another layer. Remove the node from the other layer, then add to this one.");

         node.Layer = this;
      }

      protected void SetNewBias(Bias newBias)
      {
         if (newBias == null)
         {
            DisconnectNodeFromNextLayer(this.Bias);
         }
         else
         {
            SetNodeToLayer(newBias);
            ConnectNodeToNextLayer(newBias);
         }
      }

      private void Edge_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         FireChanges(sender, e.PropertyName);
      }

      private void NextLayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         FireChanges(sender, e.PropertyName);
      }

      private void Bias_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         FireChanges(sender, e.PropertyName);
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
