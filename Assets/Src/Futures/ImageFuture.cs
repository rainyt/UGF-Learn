using UnityEngine;
using UnityGameFramework.Runtime;

namespace Futures
{
    /**
     * 图片异步操作的结果，可以包含成功的结果数据或者失败的错误信息。
     */
    public class ImageFuture : Future
    {
        public ImageFuture(object requestData) : base(requestData)
        {

        }

        override public void Post()
        {
            base.Post();
            // 操作图片位图加载
            this.resource.LoadAsset(this.requestData.ToString(), new GameFramework.Resource.LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Debug.Log($"Load image asset success: {assetName}, duration: {duration}, asset: {asset}");
                    this.CompleteValue(asset);
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