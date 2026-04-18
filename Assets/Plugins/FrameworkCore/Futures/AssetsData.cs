using UnityEngine;
using UnityGameFramework.Runtime;

namespace FrameworkCore.Futures
{
    /// <summary>
    /// 资源数据类，在使用不同的资源管理加载器去进行加载时，在使用`Dispose`方法释放对应方式加载的资源。
    /// </summary>
    public class AssetsData
    {
        public AssetsData() { }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name;

        /// <summary>
        /// Texture2D资源
        /// </summary>
        public object Asset;

        /// <summary>
        /// 释放资源
        /// </summary>
        virtual public void Dispose()
        {
            GameEntry.Resource.UnloadAsset(Asset);
            Asset = null;
        }
    }
}