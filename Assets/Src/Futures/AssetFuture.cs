using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Futures
{
    /**
     * 资源(图片、音频、预制体等)异步操作的结果，可以包含成功的结果数据或者失败的错误信息。
     */
    public class AssetFuture : Future<AssetsData, string>
    {
        public AssetFuture(object requestData) : base(requestData)
        {

        }

        override public void Post()
        {
            base.Post();
            this.resource.LoadAsset(this.requestData.ToString(), new GameFramework.Resource.LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Debug.Log($"Load image asset success: {assetName}, duration: {duration}, asset: {asset}");
                    var assetsData = new AssetsData();
                    assetsData.Name = assetName;
                    assetsData.Asset = asset;
                    this.CompleteValue(assetsData);
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