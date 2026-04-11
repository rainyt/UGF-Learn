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