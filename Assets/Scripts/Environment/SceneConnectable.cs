using UnityEngine;

namespace Environment
{
    public interface ISceneConnectable
    {
        GameObject gameObject { get; }

        public string GetSceneConnectionId();
    }
}
