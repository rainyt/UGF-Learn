using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityGameFramework.Editor.ResourceTools;
using UnityGameFramework.Runtime;

namespace Futures
{

    public delegate void FutureCompleteCallback(string assetName, object result);

    public delegate void FutureErrorCallback(string assetName, FutureError errorMessage);

    /**
     * Future类表示一个异步操作的结果，可以包含成功的结果数据或者失败的错误信息。
     */
    public class Future
    {
        /**
         * 资源组件，可以通过`resource`属性访问，用于加载资源等操作。
         */
        protected ResourceComponent resource => GameEntry.GetComponent<ResourceComponent>();

        /**
         * 错误信息，当`isError`为true时，表示请求失败，错误信息包含在`Error`中。
         */
        public FutureError Error { get; set; }

        /**
         * 结果数据，当`isError`为false时，且`IsSuccess`为true时，表示表示请求成功，结果数据包含在`Result`中。
         */
        public object Result { get; set; }

        /**
         * 是否成功，当`isError`为false时，表示请求成功，结果数据包含在`Result`中。
         */
        public bool IsSuccess { get; set; }

        /**
         * 是否失败，当`isError`为true时，表示请求失败，错误信息包含在`Error`中。
         */
        public bool IsError => Error != null;

        /**
         * 请求数据，请求时传递的参数。
         */
        protected object requestData;

        /**
         * 完成回调列表，当请求完成时，会依次调用这些回调函数，传递结果数据。
         */
        private List<FutureCompleteCallback> onCompleteCallbacks = new List<FutureCompleteCallback>();

        /**
         * 错误回调列表，当请求发生错误时，会依次调用这些回调函数，传递错误信息。
         */
        private List<FutureErrorCallback> onErrorCallbacks = new List<FutureErrorCallback>();

        public Future(object requestData)
        {
            this.requestData = requestData;
        }

        protected bool isPosting = false;

        /**
         * 发送请求，开始执行异步操作。
         */
        virtual public void Post()
        {
        }

        public string GetAssetName()
        {
            string path = requestData.ToString();
            // 去掉路径
            path = path.Substring(path.LastIndexOf('/') + 1, path.Length - path.LastIndexOf('/') - 1);
            // 去掉后缀
            path = path.Substring(0, path.LastIndexOf('.'));
            return path;
        }

        /**
         * 完成请求，设置结果数据为`result`，并设置`IsSuccess`为true。
         */
        protected void CompleteValue(object result)
        {
            this.Result = result;
            this.IsSuccess = true;
            onCompleteCallbacks.ForEach(callback =>
            {
                callback(GetAssetName(), result);
            });
        }

        /**
         * 发生错误，设置错误信息为`errorMessage`，并设置`IsSuccess`为false。
         */
        protected void ErrorValue(string errorMessage)
        {
            this.Error = new FutureError(errorMessage);
            onErrorCallbacks.ForEach(callback =>
            {
                callback(GetAssetName(), this.Error);
            });
        }

        /**
         * 添加完成回调函数，当请求完成时，会调用这些回调函数，传递结果数据。
         */
        public Future OnComplete(FutureCompleteCallback onComplete)
        {
            this.onCompleteCallbacks.Add(onComplete);
            return this;
        }

        /**
         * 添加错误回调函数，当请求发生错误时，会调用这些回调函数，传递错误信息。
         */
        public Future OnError(FutureErrorCallback onError)
        {
            this.onErrorCallbacks.Add(onError);
            return this;
        }

        /**
         * 移除所有回调函数，清空完成回调列表和错误回调列表。
         */
        public void RemoveCallbacks()
        {
            this.onCompleteCallbacks.Clear();
            this.onErrorCallbacks.Clear();
        }

    }
}