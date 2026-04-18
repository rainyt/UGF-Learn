using System.IO;
using FairyGUI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Futures
{
    /// <summary>
    /// FairyUI加载异步任务类。
    /// </summary>
    public class FairyUIFuture : Future<object, string>
    {
        public FairyUIFuture(string requestData) : base(requestData)
        {
        }

        public override void Post()
        {
            base.Post();
            var bytesFuture = new BytesFuture(requestData + "_fui.bytes");
            bytesFuture.OnComplete((assetName, bytes) =>
            {

                var fuiBytes = (byte[])bytes;

                Debug.Log($"FairyUI.bytes 加载完成：{assetName}");

                if (fuiBytes != null)
                {
                    Debug.Log($"FairyUI.bytes 加载完成，字节数：{fuiBytes.Length}");
                }
                else
                {
                    Debug.LogError($"FairyUI.bytes 加载失败");
                }

                int count = 0;
                int loadedCount = 0;
                var fileName = Path.GetFileName(requestData);
                UIPackage package = null;
                package = UIPackage.AddPackage(fuiBytes, fileName, (string name, string extension, System.Type type, PackageItem item) =>
                {
                    count++;
                    Debug.Log($"FairyUI.bytes 所需资源加载处理：{name}");
                    var dictionary = Path.GetDirectoryName(requestData);
                    var loadFile = Path.Join(dictionary, name + extension);
                    Debug.Log($"FairyUI.bytes 所需资源加载处理：{loadFile}");
                    var future = new AssetFuture(loadFile);
                    future.OnComplete((assetName, assets) =>
                    {
                        var asset = (AssetsData)assets;
                        Debug.Log($"FairyUI.bytes 所需资源加载完成：{assetName} assets: {asset.Asset}");
                        item.owner.SetItemAsset(item, asset.Asset, DestroyMethod.None);
                        loadedCount++;
                        if (loadedCount == count)
                        {
                            CompleteValue(new FairyUIData(package));
                        }
                    });
                    future.OnError((assetName, error) =>
                    {
                        ErrorValue(error.Message);
                    });
                    future.Post();
                });
                package.LoadAllAssets();
            });
            bytesFuture.OnError((assetName, error) =>
            {
                ErrorValue(error.Message);
            });
            bytesFuture.Post();
        }
    }
}
