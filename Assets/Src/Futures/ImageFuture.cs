using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Futures
{
    /**
     * 图片异步操作的结果，可以包含成功的结果数据或者失败的错误信息。
     */
    public class ImageFuture : Future<Texture2DData, string>
    {
        public ImageFuture(object requestData) : base(requestData)
        {

        }

        override public void Post()
        {
            base.Post();
            // 操作图片位图加载
            // 需要检查一次YooPackage是否存在
            foreach (var package in this.Manager.Packages)
            {
                try
                {
                    var isExist = package.Value.CheckLocationValid(this.requestData);
                    if (isExist)
                    {
                        Debug.Log($"[YooAsset] Load image asset from package: {package.Key}, asset name: {this.requestData}");
                        var handle = package.Value.LoadAssetAsync<Texture2D>(this.requestData);
                        handle.Completed += (result) =>
                        {
                            if (result.Status == YooAsset.EOperationStatus.Succeed)
                            {
                                var texture2DData = new HandleTexture2DData();
                                texture2DData.Name = this.GetAssetName();
                                texture2DData.Texture = result.AssetObject as Texture2D;
                                texture2DData.Handle = handle;
                                this.CompleteValue(texture2DData);
                            }
                            else
                            {
                                this.ErrorValue($"加载图片资源{this.requestData}失败，错误信息：{result.ToString()}");
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
            Debug.Log($"Ready load image asset: {this.requestData}");
            this.resource.LoadAsset(this.requestData.ToString(), new GameFramework.Resource.LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Debug.Log($"Load image asset success: {assetName}, duration: {duration}, asset: {asset}");
                    var texture2DData = new Texture2DData();
                    texture2DData.Name = this.GetAssetName();
                    texture2DData.Texture = asset as Texture2D;
                    this.CompleteValue(texture2DData);
                },
                (assetName, status, errorMessage, userData) =>
                {
                    Debug.LogError($"Load image asset failure: {assetName}, status: {status}, error message: {errorMessage}");
                    this.ErrorValue(errorMessage);
                }
            ));
        }

    }
}