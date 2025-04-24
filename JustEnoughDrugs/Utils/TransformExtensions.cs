using UnityEngine;

namespace JustEnoughDrugs.Utils
{
    public static class TransformExtensions
    {
        public static Transform FindChildByPath(Transform parent, string path)
        {
            var current = parent;
            foreach (var part in path.Split('/'))
            {
                current = current.Find(part);
                if (current == null) return null;
            }
            return current;
        }
    }
}