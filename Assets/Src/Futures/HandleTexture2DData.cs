using YooAsset;

namespace Futures
{
    /// <summary>
    /// 依赖YooAsset加载的Texture2D数据类，释放时会通过`AssetHandle.Dispose`方法释放资源。
    /// </summary>
    class HandleTexture2DData : Texture2DData
    {

        public HandleTexture2DData() { }

        public AssetHandle Handle;

        public override void Dispose()
        {
            if (Handle != null)
                Handle.Dispose();
            Handle = null;
            Texture = null;
        }
    }
}