using System;
using System.Collections.Generic;
using Futures;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;

namespace Game
{

    public delegate void ProgressCallback(float progress);

    public delegate void LoadAssetCallbacks(bool success, string message);


    /**
     * 资源管理器
     */
    public class AssetsManager
    {

        private static AssetsManager __instance;

        /// <summary>
        /// 获取资源管理器实例。
        /// </summary>
        public static AssetsManager Instance
        {
            get
            {
                if (__instance == null)
                {
                    __instance = new AssetsManager();
                }
                return __instance;
            }
        }


        public ResourceComponent resource;

        private ProgressCallback progressCallback;

        private LoadAssetCallbacks loadAssetCallbacks;

        private List<IFuture> futures = new List<IFuture>();

        private int loadedCount = 0;

        private bool isLoading = false;

        public Dictionary<string, AssetsData> Assets = new Dictionary<string, AssetsData>();
        public Dictionary<string, Texture2D> Texture2ds = new Dictionary<string, Texture2D>();
        public Dictionary<string, string> Strings = new Dictionary<string, string>();
        public Dictionary<string, byte[]> Binaries = new Dictionary<string, byte[]>();
        public Dictionary<string, ResourcePackage> Packages = new Dictionary<string, ResourcePackage>();

        public ResourcePackage GetPackage(string packageName)
        {
            if (Packages.ContainsKey(packageName))
            {
                return Packages[packageName];
            }
            else
            {
                Debug.LogError($"AssetsManager GetPackage: {packageName} not found");
                return null;
            }
        }

        /// <summary>
        /// 获取加载成功的Texture2D资源，传递资源的名称，如果资源存在，则返回Texture2D对象，如果资源不存在，则返回null，并在控制台输出错误日志。
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public Texture2D GetTexture2D(string assetName)
        {
            if (Texture2ds.ContainsKey(assetName))
            {
                return Texture2ds[assetName];
            }
            else
            {
                Debug.LogError($"AssetsManager GetTexture2D: {assetName} not found");
                return null;
            }
        }

        /// <summary>
        /// 获取加载成功的文本资源，传递资源的名称，如果资源存在，则返回字符串，如果资源不存在，则返回null，并在控制台输出错误日志。
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public string GetString(string assetName)
        {
            if (Strings.ContainsKey(assetName))
            {
                return Strings[assetName];
            }
            else
            {
                Debug.LogError($"AssetsManager GetString: {assetName} not found");
                return null;
            }
        }

        /// <summary>
        /// 获取加载成功的二进制资源，传递资源的名称，如果资源存在，则返回二进制数组，如果资源不存在，则返回null，并在控制台输出错误日志。
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public byte[] GetBinary(string assetName)
        {
            if (Binaries.ContainsKey(assetName))
            {
                return Binaries[assetName];
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

        /// <summary>
        /// 加载图片文件，如果存在YooAsset资源包，优先从资源包中加载，如果资源包中不存在，则从文件系统中加载。
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadFile(string filePath)
        {
            Debug.Log("resource LoadAsset:" + filePath + " resource:" + (resource != null ? "true" : "false"));
            futures.Add(new AssetFuture(filePath));
        }


        /// <summary>
        /// 加载文本文件，如果存在YooAsset资源包，优先从资源包中加载，如果资源包中不存在，则从文件系统中加载。
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadString(string filePath)
        {
            futures.Add(new StringFuture(filePath));
        }

        /// <summary>
        /// 加载二进制文件，如果存在YooAsset资源包，优先从资源包中加载，如果资源包中不存在，则从文件系统中加载。
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadBinary(string filePath)
        {
            futures.Add(new BytesFuture(filePath));
        }

        /// <summary>
        /// 加载YooAsset资源包，传递资源包的名称。资源包会优先加载，当资源包加载完成后，才会继续加载其他资源包。
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="isDefault"></param>
        public void LoadPackage(string packageName, bool isDefault = false)
        {
            futures.Add(new PackageFuture(packageName, isDefault));
        }

        /**
         * 当加载成功新的资源时，会调用这个方法，传递资源的名称和资源对象。
         */
        protected void OnNewObject(string assetName, object asset)
        {
            loadedCount++;
            Debug.Log($"AssetsManager OnNewObject: {assetName}, asset: {asset}");
            if (asset is AssetsData assetsData)
            {
                if (assetsData.Asset is Texture2D texture2D)
                {
                    Debug.Log($"AssetsManager OnNewObject: {assetName}, texture2D: {texture2D}");
                    Texture2ds[assetName] = texture2D;
                }
            }
            else if (asset is ResourcePackage package)
            {
                Packages[package.PackageName] = package;
            }
            else if (asset is string str)
            {
                Strings[assetName] = str;
            }
            else if (asset is byte[] bytes)
            {
                Binaries[assetName] = bytes;
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
            else if (packageFutures.Count > 0 && loadedCount == packageFutures.Count)
            {
                // 当资源包加载完成后，继续加载其他资源包
                loadNext();
            }
        }

        /// <summary>
        /// 侦听加载进度回调函数，当每个资源加载成功时，会调用这个函数，传递当前的加载进度（0到1之间的浮点数）。
        /// </summary>
        /// <param name="callback"></param>
        public void OnProgress(ProgressCallback callback)
        {
            progressCallback = callback;
        }

        private List<IFuture> packageFutures = new List<IFuture>();
        private List<IFuture> otherFutures = new List<IFuture>();

        /// <summary>
        /// 开始加载资源，传递一个回调函数，当所有资源加载完成或者发生错误时，会调用这个函数，传递一个布尔值表示是否成功和一个字符串消息。
        /// </summary>
        /// <param name="callback"></param>
        public void Start(LoadAssetCallbacks callback)
        {
            if (isLoading)
            {
                Debug.LogError("AssetsManager is already loading");
                return;
            }
            packageFutures.Clear();
            otherFutures.Clear();
            loadedCount = 0;
            isLoading = true;
            loadAssetCallbacks = callback;
            // 对资源进行排序，优先先加载PackageFuture
            futures.Sort((a, b) => a is PackageFuture ? -1 : 1);
            // 先优先加载所有PackageFuture
            futures.ForEach(future =>
            {
                if (future is PackageFuture)
                {
                    packageFutures.Add(future);
                }
                else
                {
                    otherFutures.Add(future);
                }
            });
            this.loadNext();
        }

        /// <summary>
        /// 统一处理加载资源的Future，设置完成和错误回调函数，并发送请求开始加载资源。
        /// </summary>
        /// <param name="future"></param>
        private void startLoadFuture(IFuture future)
        {
            future.RemoveCallbacks();
            future.OnComplete((assetName, result) =>
            {
                Debug.Log($"AssetsManager OnComplete: {assetName}, result: {result}");
                OnNewObject(assetName, result);
            }).OnError((assetName, error) =>
            {
                Debug.LogError($"AssetsManager OnError: {assetName}, error: {error}");
                Stop(true);
            });
            Debug.Log($"AssetsManager Start load: {future}");
            future.Manager = this;
            future.Post();
            future.Manager = null;
        }

        /// <summary>
        /// 由内部调用，加载下一个资源包或其他资源。
        /// </summary>
        private void loadNext()
        {
            Debug.Log($"AssetsManager loadNext: {loadedCount}, {packageFutures.Count}, {otherFutures.Count}");
            if (packageFutures.Count > 0 && loadedCount < packageFutures.Count)
            {
                packageFutures.ForEach(future =>
                {
                    startLoadFuture(future);
                });
            }
            else if (otherFutures.Count > 0)
            {
                otherFutures.ForEach(future =>
                {
                    startLoadFuture(future);
                });
            }
        }

        /// <summary>
        /// 停止加载资源，传递一个布尔值表示是否失败。
        /// </summary>
        /// <param name="isFail"></param>
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
                loadAssetCallbacks(false, "Assets loading stopped");
            }
        }

        public void Dispose()
        {
            Stop();
            loadAssetCallbacks = null;
            progressCallback = null;
            foreach (var asset in Assets.Values)
            {
                asset.Dispose();
            }
            Assets.Clear();
            Texture2ds.Clear();
            Strings.Clear();
            foreach (var package in Packages.Values)
            {
                YooAssets.RemovePackage(package.PackageName);
            }
            packageFutures.Clear();
        }

    }

}