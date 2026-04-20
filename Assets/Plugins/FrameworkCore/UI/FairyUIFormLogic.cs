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
            FieldInfo[] fields = type.GetFields(BindingFlags.Public
                                 | BindingFlags.NonPublic
                                 | BindingFlags.Instance);
            Debug.Log("OnInitLogic:" + type.Name + " fields.length=" + fields.Length);
            Debug.Log(this);
            foreach (FieldInfo field in fields)
            {
                if (field.Name == "viewObject")
                    continue;
                Debug.Log("Field " + field.Name + ":" + field.FieldType.Namespace + "." + field.FieldType.Name);
                if (!field.IsLiteral && !field.IsInitOnly && !field.IsStatic)
                {
                    Debug.Log("Auto Field Bind:" + field.Name);
                    if (typeof(GObject).IsAssignableFrom(field.FieldType))
                    {
                        // 如果是属于变量
                        GObject obj = viewObject.asCom.GetChild(field.Name);
                        Debug.Log("Auto Field Bind Obj:" + obj);
                        if (obj != null)
                        {
                            Debug.Log("Auto Field Bind:s" + field.Name + ":" + field.FieldType.Namespace + "." + field.FieldType.Name);
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