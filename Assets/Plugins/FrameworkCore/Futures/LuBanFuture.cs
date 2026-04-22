using UnityGameFramework.Runtime;

namespace FrameworkCore.Futures
{
    /// <summary>
    /// Luban异步加载任务类
    /// </summary>
    public class LuBanFuture : Future<LuBanData, string>
    {
        public LuBanFuture(object requestData) : base(requestData)
        {

        }

        public override void Post()
        {
            base.Post();
            new BytesFuture(requestData).OnComplete((assetsName, bytes) =>
           {
               CompleteValue(new LuBanData((byte[])bytes));
           }).OnError((assetsName, error) =>
           {
               ErrorValue(error.Message);
           }).Post();
        }
    }
}
