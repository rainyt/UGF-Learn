using FairyGUI;
using UnityGameFramework.Runtime;

namespace Scenes
{
    /// <summary>
    /// FairyGUI逻辑组件
    /// </summary>
    public class FairyUIFormLogic : UIFormLogic
    {

        /// <summary>
        /// FairyGUI视图对象
        /// </summary>
        public GObject viewObject;


        public void OnInitLogic(object userData, GObject viewObject)
        {
            this.viewObject = viewObject;
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