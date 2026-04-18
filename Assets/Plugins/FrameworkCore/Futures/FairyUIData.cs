using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace FrameworkCore.Futures
{
    /// <summary>
    /// FairyGUI资源包数据类
    /// </summary>
    public class FairyUIData
    {

        /// <summary>
        /// 资源包引用计数 
        /// 用于记录资源包的引用次数，避免重复释放。
        /// </summary>
        private static Dictionary<string, int> packages_ref_count = new Dictionary<string, int>();

        /// <summary>
        /// FairyGUI资源包名称
        /// </summary>
        public string name => package == null ? "" : package.name;

        /// <summary>
        /// FairyGUI资源包
        /// </summary>
        public UIPackage package;

        public FairyUIData(UIPackage package)
        {
            this.package = package;
            if (packages_ref_count.ContainsKey(this.name))
                packages_ref_count[this.name]++;
            else
                packages_ref_count[this.name] = 1;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        virtual public void Dispose()
        {
            if (packages_ref_count.ContainsKey(this.name))
                packages_ref_count[this.name]--;
            if (packages_ref_count[this.name] <= 0)
            {
                package.GetItems().ForEach(item =>
                {
                    // TODO 需要进行释放处理
                    Debug.Log($"释放资源包：{item.name}");
                });
                UIPackage.RemovePackage(name);
            }
            package = null;
        }
    }
}
