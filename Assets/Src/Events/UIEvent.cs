using GameFramework.Event;

namespace Events
{
    public class UIEvent : GameEventArgs
    {
        /// <summary>
        /// 开始游戏事件。
        /// </summary>
        public const int START_GAME = 1001;

        private int __eventId = 0;

        // 2. 实现抽象属性 Id
        public override int Id => __eventId;

        // 3. 自定义数据（如果你想传关卡 ID 或模式，写在这里）
        public string GameMode { get; private set; }

        // 4. 清理逻辑（当对象被回收到引用池时执行）
        public override void Clear()
        {
            GameMode = null;
        }

        // 5. 静态创建方法（从引用池中获取对象，代替 new）
        public static UIEvent Create(int eventId)
        {
            UIEvent args = new UIEvent();
            args.__eventId = eventId;
            return args;
        }
    }
}