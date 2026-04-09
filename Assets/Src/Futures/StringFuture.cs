namespace Futures
{
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityGameFramework.Runtime;
    public class StringFuture : Future<string, string>
    {
        public StringFuture(object requestData) : base(requestData)
        {

        }

        public override void Post()
        {
            base.Post();
            new BytesFuture(this.requestData).OnComplete((assetName, result) =>
            {
                byte[] bytes = (byte[])result;
                Debug.Log($"Load bytes asset success: {assetName}, bytes length: {bytes.Length}");
                string str = System.Text.Encoding.UTF8.GetString(bytes);
                this.CompleteValue(str);
            }).OnError((assetName, errorMessage) =>
            {
                Debug.LogError($"Load bytes asset failure: {assetName}, error message: {errorMessage.Message}");
                this.ErrorValue(errorMessage.Message);
            }).Post();
        }
    }
}