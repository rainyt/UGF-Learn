using FrameworkCore;

namespace FrameworkCore.Futures
{
    public interface IFuture
    {
        public void RemoveCallbacks();

        public IFuture OnComplete(FutureCompleteCallback onComplete);

        public IFuture OnError(FutureErrorCallback onError);

        public void Post();

        public AssetsManager Manager { get; set; }
    }
}