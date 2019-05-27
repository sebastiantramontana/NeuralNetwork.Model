namespace NeuralNetwork.Model.Exceptions
{
    public class DuplicatedIdException : IdExceptionBase
    {
        internal DuplicatedIdException(string id) : base(id, $"Id '{id}' already exists. The Ids cannot be duplicated")
        {
        }
    }
}
