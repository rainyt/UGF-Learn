using UnityEngine;

namespace Futures
{
    /// <summary>
    /// Texture2D数据类，在使用不同的资源管理加载器去进行加载时，在使用`Dispose`方法释放对应方式加载的Texture2D资源。
    /// </summary>
    public class Texture2DData
    {
        public Texture2DData() { }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name;

        /// <summary>
        /// Texture2D资源
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// 释放Texture2D资源
        /// </summary>
        virtual public void Dispose()
        {
            Object.Destroy(Texture);
            Texture = null;
        }
    }
}