using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.PrefabLoader
{
    public class ResourcesPrefabLoadStrategy : IPrefabLoadStrategy
    {
        public async UniTask<T> AsyncLoadPrefab<T>(string patch) where T : Object
        {
            var prefab = await Resources.LoadAsync<T>(patch);

            return (prefab as T);
        }

        public T LoadPrefab<T>(string patch) where T : Object
        {
            return Resources.Load<T>(patch);
        }
    }
}
