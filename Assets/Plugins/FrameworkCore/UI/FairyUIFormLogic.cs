using System;
using System.Reflection;
using FairyGUI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FrameworkCore.UI
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
            Type type = this.GetType();
            // 获取所有公开和非公开实例字段
            FieldInfo[] fields = type.GetFields(BindingFlags.Public
                                 | BindingFlags.NonPublic
                                 | BindingFlags.Instance);
            Debug.Log(this);
            foreach (FieldInfo field in fields)
            {
                if (field.Name == "viewObject")
                    continue;
                // 判断是否是变量
                if (!field.IsLiteral && !field.IsInitOnly && !field.IsStatic)
                {
                    // 判断是否继承自GObject
                    if (typeof(GObject).IsAssignableFrom(field.FieldType))
                    {
                        // 如果是属于变量
                        GObject obj = viewObject.asCom.GetChild(field.Name);
                        // 判断是否来自字段的类型
                        if (obj != null && obj.GetType().IsAssignableFrom(field.FieldType))
                        {
                            field.SetValue(this, obj);
                        }
                    }
                }
            }
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