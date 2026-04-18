
using System.IO;
using Data;
using FairyGUI;
using Game;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Scenes
{
    /// <summary>
    /// FairyUI视图
    /// </summary>
    public class FairyView : UIFormLogic
    {

        public FairyViewData viewData;

        public GObject viewObject;

        public AssetsManager assets;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Debug.Log("FairyView OnInit");
        }

        protected override void OnOpen(object userData)
        {
            Debug.Log("FairyView OnOpen");
            base.OnOpen(userData);
            viewData = (FairyViewData)userData;
            assets = new AssetsManager();
            assets.LoadFairyUI(viewData.packageName);
            assets.Start((success, message) =>
            {
                if (success)
                {
                    viewObject = UIPackage.CreateObject(Path.GetFileName(viewData.packageName), viewData.componentName);
                    GRoot.inst.AddChild(viewObject);
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