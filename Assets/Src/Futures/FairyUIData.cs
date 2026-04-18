using FairyGUI;
using UnityEngine;

namespace Futures
{
    /// <summary>
    /// FairyGUI资源包数据类
    /// </summary>
    public class FairyUIData
    {
        /// <summary>
        /// FairyGUI资源包名称
        /// </summary>
        public string name => package.name;

        /// <summary>
        /// FairyGUI资源包
        /// </summary>
        public UIPackage package;

        public FairyUIData(UIPackage package)
        {
            this.package = package;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        virtual public void Dispose()
        {
            package.GetItems().ForEach(item =>
            {
                // TODO 需要进行释放处理
                Debug.Log($"释放资源包：{item.name}");
            });
            UIPackage.RemovePackage(name);
        }
    }
}
