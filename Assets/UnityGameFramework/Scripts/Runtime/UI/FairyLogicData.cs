using System;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// FairyGUI逻辑数据类
    /// </summary>
    public class FairyLogicData
    {
        /// <summary>
        /// FairyGUI逻辑实例
        /// </summary>
        public Type LogicInstance;

        /// <summary>
        /// 界面资源包名称。
        /// </summary>
        public string PackageName;

        /// <summary>
        /// 组件名称。
        /// </summary>
        public string ComponentName;

        /// <summary>
        /// 用户数据
        /// </summary>
        public object UserData;
    }
}