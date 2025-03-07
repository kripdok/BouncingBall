using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.PrefabLoader
{
    public class ResourcesPrefabLoadStrategy : IPrefabLoadStrategy
    {
        public async UniTask<T> LoadPrefabAsync<T>(string path) where T : Object
        {
            var prefab = await Resources.LoadAsync<T>(path);
            return prefab as T;
        }

        public T LoadPrefabSync<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}
