using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out var component))
                return component;

            return component ?? gameObject.AddComponent<T>();
        }
    }
}