namespace Futures
{
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityGameFramework.Runtime;
    public class StringFuture : Future<string, string>
    {
        public StringFuture(object requestData) : base(requestData)
        {
            
        }
    }
}