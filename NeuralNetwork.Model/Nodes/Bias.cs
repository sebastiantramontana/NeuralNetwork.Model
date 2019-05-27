namespace NeuralNetwork.Model.Nodes
{
    public class Bias : NodeBase
    {
        public Bias(string id) : base(id)
        {
            this.OutputValue = 1.0;
        }
    }
}
