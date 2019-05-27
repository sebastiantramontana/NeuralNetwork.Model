using System;

namespace NeuralNetwork.Model.Exceptions
{
    public abstract class IdExceptionBase : Exception
    {
        internal IdExceptionBase(string id, string message) : base(message)
        {
            this.Id = id;
        }

        public string Id { get; private set; }
    }
}
