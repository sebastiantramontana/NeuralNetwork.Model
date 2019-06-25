using System.Collections.Generic;

namespace NeuralNetwork.Model.Nodes
{
   public class Neuron : NodeBase
   {
      private double? _sumValue;
      private ActivationFunction _activationFunction;

      public Neuron(string id) : base(id)
      {
         this.EdgesInternal = new List<Edge>();
      }

      public double? SumValue
      {
         get => _sumValue;
         set => ChangeProperty(ref _sumValue, value);
      }

      public ActivationFunction ActivationFunction
      {
         get => _activationFunction;
         set => ChangeProperty(ref _activationFunction, value);
      }

      public IEnumerable<Edge> Edges { get => this.EdgesInternal; } //get read-only access to externals
      internal ICollection<Edge> EdgesInternal { get; private set; } //for internal use. Hide to externals.

      private protected override void ValidateDuplicatedIChild(IDictionary<string, Element> acumulatedIds)
      {
         foreach (var edge in this.Edges)
            edge.ValidateId(acumulatedIds);
      }
   }
}
