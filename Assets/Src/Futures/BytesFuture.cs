namespace Futures
{
    using System;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityGameFramework.Runtime;

    /**
     * BytesFuture类表示一个异步加载二进制数据的Future，可以包含成功的结果数据或者失败的错误信息。
     */
    public class BytesFuture : Future<byte[], string>
    {
        public BytesFuture(object requestData) : base(requestData)
        {

        }

        public override void Post()
        {
            base.Post();
            // 需要检查一次YooPackage是否存在
            foreach (var package in this.Manager.Packages)
            {
                try
                {
                    var isExist = package.Value.CheckLocationValid(this.requestData);
                    if (isExist)
                    {
                        Debug.Log($"[YooAsset] Load image asset from package: {package.Key}, asset name: {this.requestData}");
                        var handle = package.Value.LoadAssetAsync<TextAsset>(this.requestData);
                        handle.Completed += (result) =>
                        {
                            if (result.Status == YooAsset.EOperationStatus.Succeed)
                            {
                                TextAsset asset = result.AssetObject as TextAsset;
                                this.CompleteValue(asset.bytes);
                            }
                            else
                            {
                                this.ErrorValue($"加载字节资源{this.requestData}失败，错误信息：{result.ToString()}");
                            }
                        };
                        return;
                    }
                    else
                    {
                        Debug.Log($"[YooAsset] CheckLocationValid: {package.Key}, asset name: {this.requestData}, isExist: {isExist}");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[YooAsset] CheckLocationValid: {package.Key}, asset name: {this.requestData}, error: {e.Message}");
                }
            }
            this.resource.LoadBinary(this.requestData, new GameFramework.Resource.LoadBinaryCallbacks(
                (assetName, bytes, duration, userData) =>
                {
                    Debug.Log($"Load bytes asset success: {assetName}, duration: {duration}, bytes length: {bytes.Length}");
                    this.CompleteValue(bytes);
                },
                (assetName, status, errorMessage, userData) =>
                {
                    Debug.LogError($"Load bytes asset failure: {assetName}, status: {status}, error message: {errorMessage}");
                    this.ErrorValue(errorMessage);
                }
            ));
        }
    }
}