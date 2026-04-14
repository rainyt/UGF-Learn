namespace UnityGameFramework.Runtime
{
    public class EntityIdGenerator
    {

        private static int _id = 0;

        /// <summary>
        /// 生成实体ID。会自动判断是否已存在该ID的实体。
        /// </summary>
        /// <returns></returns>
        public static int GenerateId()
        {
            while (true)
            {
                _id++;
                var isHasId = GameEntry.Entity.HasEntity(_id);
                var isLoading = GameEntry.Entity.IsLoadingEntity(_id);
                if (!isHasId && !isLoading)
                    break;
            }
            return _id;
        }
    }
}
