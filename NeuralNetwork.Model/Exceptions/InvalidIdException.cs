namespace NeuralNetwork.Model.Exceptions
{
    public class InvalidIdException : IdExceptionBase
    {
        internal InvalidIdException(string id) : base(id, "Id canot be null or empty")
        {

        }
    }
}
