using UnityEngine;

namespace Utils
{
        public class System
        {
                /// <summary>
                /// 是否为IL2CPP模式
                /// </summary>
                public static bool IsIL2CPP =>
#if ENABLE_IL2CPP
        true;
#else
                false;
#endif

                /// <summary>
                /// 屏幕的左边界。
                /// </summary>
                public static float LEFT_BOUNDARY = 0;

                /// <summary>
                /// 屏幕的右边界。
                /// </summary>
                public static float RIGHT_BOUNDARY = 0;

                /// <summary>
                /// 屏幕的上边界。
                /// </summary>
                public static float TOP_BOUNDARY = 0;

                /// <summary>
                /// 屏幕的下边界。
                /// </summary>
                public static float BOTTOM_BOUNDARY = 0;

                /// <summary>
                /// 更新屏幕的上下左右边界。
                /// </summary>
                public static void UpdateScreen()
                {
                        Vector3 vector3 = new Vector3(0, 0, 0);
                        vector3 = Camera.main.ScreenToWorldPoint(vector3);
                        LEFT_BOUNDARY = vector3.x;
                        TOP_BOUNDARY = vector3.y;

                        vector3.x = Screen.width;
                        vector3.y = Screen.height;
                        vector3.z = 0;
                        vector3 = Camera.main.ScreenToWorldPoint(vector3);
                        RIGHT_BOUNDARY = vector3.x;
                        BOTTOM_BOUNDARY = vector3.y;
                }
        }
}