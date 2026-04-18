
using System.IO;
using FairyGUI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FrameworkCore.UI
{
    /// <summary>
    /// FairyUI视图
    /// </summary>
    public class FairyComponent : UIFormLogic
    {

        public FairyLogicData logicData;

        public object userData;

        public GObject viewObject;

        public AssetsManager assets;

        public Component component;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Debug.Log("FairyView OnInit");
        }

        protected override void OnOpen(object userData)
        {
            if (userData is FairyLogicData)
            {
                logicData = userData as FairyLogicData;
                this.userData = logicData.UserData;
                // 绑定组件
                var type = logicData.LogicInstance;
                component = this.gameObject.AddComponent(type);
            }
            else
            {
                this.userData = userData;
            }
            Debug.Log("FairyView OnOpen");
            base.OnOpen(userData);
            assets = new AssetsManager();
            assets.LoadFairyUI(logicData.PackageName);
            assets.Start((success, message) =>
            {
                if (success)
                {
                    viewObject = UIPackage.CreateObject(Path.GetFileName(logicData.PackageName), logicData.ComponentName);
                    GRoot.inst.AddChild(viewObject);
                    if (component is FairyUIFormLogic uiForm)
                    {
                        uiForm.OnInitLogic(userData, viewObject);
                        uiForm.OnOpenLogic(userData);
                    }
                }
                else
                {
                    // 展示失败
                    Debug.LogError($"FairyView OnOpen {message}");
                }
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            if (component is FairyUIFormLogic uiForm)
            {
                uiForm.OnCloseLogic(isShutdown, userData);
            }
            if (viewObject != null)
                viewObject.Dispose();
            viewObject = null;
            if (assets != null)
                assets.Dispose();
            assets = null;
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            if (viewObject != null)
            {
                viewObject.sortingOrder = depthInUIGroup;
            }
        }
    }
}