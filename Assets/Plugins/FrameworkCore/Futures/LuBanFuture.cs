using UnityGameFramework.Runtime;

namespace FrameworkCore.Futures
{
    /// <summary>
    /// Luban异步加载任务类
    /// </summary>
    public class LuBanFuture : Future<object, string>
    {
        public LuBanFuture(object requestData) : base(requestData)
        {
            // GameEntry.Resource.
        }
    }
}
