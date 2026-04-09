namespace Futures
{
   public class FutureError
    {
        public string Message { get; private set; }

        public FutureError(string message)
        {
            Message = message;
        }
    }
}