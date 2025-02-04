using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Utilities.PrefabLoad
{
    public class ResourcesPrefabLoadStrategy : IPrefabLoadStrategy
    {
        public async UniTask<T> AsyncLoadPrefab<T>(string patch) where T : MonoBehaviour
        {
            var prefab = await Resources.LoadAsync<T>(patch);

            return (prefab as T);
        }

        public T LoadPrefab<T>(string patch) where T : MonoBehaviour
        {
            return Resources.Load<T>(patch);
        }
    }
}
