using System;
using System.Collections.Generic;
using Futures;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game
{

    public delegate void ProgressCallback(float progress);

    public delegate void LoadAssetCallbacks(bool success, string message);


    /**
     * 资源管理器
     */
    public class AssetsManager
    {

        public ResourceComponent resource;

        private ProgressCallback progressCallback;

        private LoadAssetCallbacks loadAssetCallbacks;

        private List<IFuture> futures = new List<IFuture>();

        private int loadedCount = 0;

        private bool isLoading = false;

        private Dictionary<string, Texture2D> texture2ds = new Dictionary<string, Texture2D>();

        /**
         * 获取加载的Texture2D资源，如果资源不存在，则返回null。
         */
        public Texture2D GetTexture2D(string assetName)
        {
            if (texture2ds.ContainsKey(assetName))
            {
                return texture2ds[assetName];
            }
            else
            {
                Debug.LogError($"AssetsManager GetTexture2D: {assetName} not found");
                return null;
            }
        }

        public AssetsManager()
        {
            Debug.Log("AssetsManager constructor?");
            resource = GameEntry.GetComponent<ResourceComponent>();
        }

        /**
         * 加载文件
         */
        public void LoadFile(string filePath)
        {
            Debug.Log("resource LoadAsset:" + filePath + " resource:" + (resource != null ? "true" : "false"));
            futures.Add(new ImageFuture(filePath));
        }

        /**
         * 当加载成功新的资源时，会调用这个方法，传递资源的名称和资源对象。
         */
        protected void OnNewObject(string assetName, object asset)
        {
            loadedCount++;
            Debug.Log($"AssetsManager OnNewObject: {assetName}, asset: {asset}");
            if (asset is Texture2D texture2D)
            {
                Debug.Log($"AssetsManager OnNewObject: {assetName}, texture2D: {texture2D}");
                texture2ds[assetName] = texture2D;
            }
            if (progressCallback != null)
            {
                progressCallback((float)loadedCount / futures.Count);
            }
            if (loadedCount == futures.Count)
            {
                if (loadAssetCallbacks != null)
                {
                    loadAssetCallbacks(true, "All assets loaded successfully");
                }
            }
        }

        public void onProgress(ProgressCallback callback)
        {
            progressCallback = callback;
        }

        public void Start(LoadAssetCallbacks callback)
        {
            if (isLoading)
            {
                Debug.LogError("AssetsManager is already loading");
                return;
            }
            loadedCount = 0;
            isLoading = true;
            loadAssetCallbacks = callback;
            futures.ForEach(future =>
            {
                future.RemoveCallbacks();
                future.OnComplete((assetName, result) =>
                {
                    OnNewObject(assetName, result);
                }).OnError((assetName, error) =>
                {
                    Debug.LogError($"AssetsManager OnError: {assetName}, error: {error}");
                    Stop(true);
                });
                future.Post();
            });
        }



        public void Stop(bool isFail = false)
        {
            if (!isLoading)
            {
                Debug.LogError("AssetsManager is not loading");
                return;
            }
            isLoading = false;
            if (isFail && loadAssetCallbacks != null)
            {
                loadAssetCallbacks(isFail, "Assets loading stopped");
            }
        }


    }

}