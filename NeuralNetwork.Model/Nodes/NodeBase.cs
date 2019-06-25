using NeuralNetwork.Model.Layers;

namespace NeuralNetwork.Model.Nodes
{
   public abstract class NodeBase : Element
   {
      private double? _outputValue;

      public NodeBase(string id) : base(id)
      {

      }
      public double? OutputValue
      {
         get => _outputValue;
         set => ChangeProperty(ref _outputValue, value);
      }

      public LayerBase Layer { get; internal set; }
   }
}
