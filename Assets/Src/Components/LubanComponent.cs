using cfg;
using FrameworkCore;
using UnityGameFramework.Runtime;

namespace Components
{
    /// <summary>
    /// LuBan组件
    /// </summary>
    public class LubanComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 数据表格
        /// </summary>
        public Tables Tables = null;

        public void Init()
        {
            Tables = new Tables(AssetsManager.Instance.GetLuBanData("AllConfigs").LoadData);
        }
    }
}