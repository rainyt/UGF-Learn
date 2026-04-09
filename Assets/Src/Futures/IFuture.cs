namespace Futures
{
    public interface IFuture
    {
        public void RemoveCallbacks();

        public IFuture OnComplete(FutureCompleteCallback onComplete);

        public IFuture OnError(FutureErrorCallback onError);

        public void Post();
    }
}