using cfg;
using Luban;

namespace UnityGameFramework.Runtime
{
    public class LubanComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 数据表格
        /// </summary>
        public Tables Tables = null;

        public void Init(System.Func<string, ByteBuf> loader)
        {
            Tables = new Tables(loader);
        }
    }
}