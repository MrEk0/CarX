using UnityEngine;

namespace Factories
{
    public abstract class ObjectPoolFactory : MonoBehaviour
    {
        public static bool CanRelease(AObjectPoolItem poolAObject) => poolAObject.gameObject.activeSelf;

        protected static void OnReleaseToPool(AObjectPoolItem pooledAObject)
        {
            pooledAObject.gameObject.SetActive(false);
        }

        protected static void OnGetFromPool(AObjectPoolItem pooledAObject)
        {
            pooledAObject.gameObject.SetActive(true);
        }
    }
}