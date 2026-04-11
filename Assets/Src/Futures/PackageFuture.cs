using UnityEngine;
using YooAsset;

namespace Futures
{

    /**
     * 可加载YooAssets的`AssetBundle`的Future类，表示一个异步加载`AssetBundle`的操作，可以包含成功的结果数据或者失败的错误信息。
     */
    public class PackageFuture : Future<ResourcePackage, string>
    {

        public static bool IsInitialized = false;

        private bool isDefault = false;

        public PackageFuture(string packageName, bool isDefault = false) : base(packageName)
        {
            this.isDefault = isDefault;
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
            Debug.Log($"PackageFuture EditorFileSystemParameters: {this.requestData}");
            var initParameters = new EditorSimulateModeParameters();
            initParameters.EditorFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
#else
            Debug.Log($"PackageFuture BuildinFileSystemParameters: {this.requestData}");
            var initParameters = new OfflinePlayModeParameters();
            initParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
#endif
            var handle = package.InitializeAsync(initParameters);
            handle.Completed += (result) =>
            {
                if (result.Status == EOperationStatus.Succeed)
                {
                    var handle = package.RequestPackageVersionAsync();
                    handle.Completed += (result) =>
                    {
                        if (result.Status == EOperationStatus.Succeed)
                        {
                            var op = result as RequestPackageVersionOperation;
                            Debug.Log($"[YooAsset] RequestPackageVersion: {this.requestData}, version: {result}");
                            var handle = package.UpdatePackageManifestAsync(op.PackageVersion);
                            handle.Completed += (result) =>
                            {
                                if (result.Status == EOperationStatus.Succeed)
                                {
                                    Debug.Log($"[YooAsset] UpdatePackageManifest: {this.requestData}, version: {result}");
                                    if (isDefault)
                                    {
                                        YooAssets.SetDefaultPackage(package);
                                    }
                                    this.CompleteValue(package);
                                }
                                else
                                {
                                    Debug.LogError($"[YooAsset] UpdatePackageManifest: {this.requestData}, error: {result.Error}");
                                    this.ErrorValue($"加载资源包{this.requestData}失败，错误信息：{result.Error}");
                                }
                            };
                        }
                        else
                        {
                            Debug.LogError($"[YooAsset] RequestPackageVersion: {this.requestData}, error: {result.Error}");
                            this.ErrorValue($"加载资源包{this.requestData}失败，错误信息：{result.Error}");
                        }
                    };
                }
                else
                {
                    this.ErrorValue($"加载资源包{this.requestData}失败，错误信息：{result.Error}");
                }
            };
        }


    }
}