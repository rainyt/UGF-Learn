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
    }
}