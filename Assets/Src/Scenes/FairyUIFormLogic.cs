using UnityGameFramework.Runtime;

namespace Scenes
{
    /// <summary>
    /// FairyGUI逻辑组件
    /// </summary>
    public class FairyUIFormLogic : UIFormLogic
    {
        /// <summary>
        /// FairyGUI逻辑实例
        /// </summary>
        public object LogicInstance;

        public void OnInitLogic(object userData)
        {
            this.OnInit(userData);
        }

        public void OnOpenLogic(object userData)
        {
            this.OnOpen(userData);
        }

        public void OnCloseLogic(bool isShutdown, object userData)
        {
            this.OnClose(isShutdown, userData);
        }
    }
}