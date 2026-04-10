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
        private Dictionary<string, string> strings = new Dictionary<string, string>();
        private Dictionary<string, byte[]> binaries = new Dictionary<string, byte[]>();

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

        public string GetString(string assetName)
        {
            if (strings.ContainsKey(assetName))
            {
                return strings[assetName];
            }
            else
            {
                Debug.LogError($"AssetsManager GetString: {assetName} not found");
                return null;
            }
        }

        public byte[] GetBinary(string assetName)
        {
            if (binaries.ContainsKey(assetName))
            {
                return binaries[assetName];
            }
            else
            {
                Debug.LogError($"AssetsManager GetBinary: {assetName} not found");
                return null;
            }
        }

        public AssetsManager()
        {
            Debug.Log("AssetsManager constructor?");
            resource = GameEntry.GetComponent<ResourceComponent>();
        }

        /**
         * 加载图片文件
         */
        public void LoadFile(string filePath)
        {
            Debug.Log("resource LoadAsset:" + filePath + " resource:" + (resource != null ? "true" : "false"));
            futures.Add(new ImageFuture(filePath));
        }


        /**
         * 加载文本文件
         */
        public void LoadString(string filePath)
        {
            futures.Add(new StringFuture(filePath));
        }

        /**
         * 加载二进制文件
         */
        public void LoadBinary(string filePath)
        {
            futures.Add(new BytesFuture(filePath));
        }

        /**
         * 加载资源包
         */
        public void LoadPackage(string packageName)
        {
            futures.Add(new PackageFuture(packageName));
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
            else if (asset is string str)
            {
                strings[assetName] = str;
            }
            else if (asset is byte[] bytes)
            {
                binaries[assetName] = bytes;
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

        public void OnProgress(ProgressCallback callback)
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
            // 对资源进行排序，优先先加载PackageFuture
            futures.Sort((a, b) => a is PackageFuture ? -1 : 1);
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