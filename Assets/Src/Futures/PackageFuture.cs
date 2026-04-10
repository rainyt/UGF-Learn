using YooAsset;

namespace Futures
{
    /**
     * 可加载YooAssets的`AssetBundle`的Future类，表示一个异步加载`AssetBundle`的操作，可以包含成功的结果数据或者失败的错误信息。
     */
    public class PackageFuture : Future<ResourcePackage, string>
    {

        public static bool IsInitialized = false;

        public PackageFuture(string packageName) : base(packageName)
        {
        }

        public override void Post()
        {
            base.Post();
            if (!IsInitialized)
            {
                IsInitialized = true;
                YooAssets.Initialize();
            }
            var package = YooAssets.CreatePackage(this.requestData);
#if UNITY_EDITOR
            var initParameters = new EditorSimulateModeParameters();
            initParameters.EditorFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
#else
            var initParameters = new OfflinePlayModeParameters();
            initParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
#endif
            var handle = package.InitializeAsync(initParameters);
            handle.Completed += (result) =>
            {
                if (result.Status == EOperationStatus.Succeed)
                {
                    this.CompleteValue(package);
                }
                else
                {
                    this.ErrorValue($"加载资源包{this.requestData}失败，错误信息：{result.Error}");
                }
            };
        }


    }
}