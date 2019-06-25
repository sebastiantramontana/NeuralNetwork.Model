using System;

namespace NeuralNetwork.Model.Nodes
{
   public class Edge : Element
   {
      private double? _weight;

      private Edge(string id) : base(id)
      {
      }

      public NodeBase Source { get; private set; }
      public Neuron Destination { get; private set; }
      public double? Weight
      {
         get => _weight;
         set => ChangeProperty(ref _weight, value);
      }

      internal static Edge Create(NodeBase source, Neuron destination)
      {
         if (source == null || destination == null)
            throw new ArgumentNullException("source and/or destination arguments are null");

         var id = source.Layer.Id + "." + source.Id + " - " + destination.Layer.Id + "." + destination.Id;
         return new Edge(id) { Source = source, Destination = destination };
      }
   }
}
