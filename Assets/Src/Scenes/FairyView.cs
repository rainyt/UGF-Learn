
using System;
using System.IO;
using Data;
using FairyGUI;
using Game;
using GameFramework.UI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Scenes
{
    /// <summary>
    /// FairyUI视图
    /// </summary>
    public class FairyView : UIFormLogic
    {

        public FairyLogicData logicData;

        public FairyViewData viewData;

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
                viewData = (FairyViewData)logicData.UserData;
                // 绑定组件
                var type = logicData.LogicInstance;
                component = this.gameObject.AddComponent(type);
                if (component is FairyUIFormLogic uiForm)
                {
                    uiForm.OnInitLogic(userData);
                    uiForm.OnOpenLogic(userData);
                }
            }
            else if (userData is FairyViewData)
            {
                viewData = (FairyViewData)userData;
            }
            Debug.Log("FairyView OnOpen");
            base.OnOpen(userData);
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