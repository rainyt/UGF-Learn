using FairyGUI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Utils
{
    public class FGUI
    {
        public static void Init(int width, int height)
        {
            GRoot.inst.SetContentScaleFactor(width, height);
            Camera stageCamera = GameObject.Find("Stage Camera").GetComponent<Camera>();

            stageCamera.depth = 10;
            stageCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
            stageCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            stageCamera.clearFlags = CameraClearFlags.Depth;

            Debug.Log("FGUI.Init success.");

        }
    }
}