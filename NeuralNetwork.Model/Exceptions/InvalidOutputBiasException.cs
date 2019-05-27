using NeuralNetwork.Model.Layers;
using System;

namespace NeuralNetwork.Model.Exceptions
{
    public class InvalidOutputBiasException : Exception
    {
        internal InvalidOutputBiasException(LayerBase outputLayer) : base("Output layer cannot contain bias node")
        {
            this.OutputLayer = outputLayer;
        }

        public LayerBase OutputLayer { get; private set; }
    }
}
