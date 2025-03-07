using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.PrefabLoader
{
    public interface IPrefabLoadStrategy
    {
        public T LoadPrefabSync<T>(string path) where T : Object;

        public UniTask<T> LoadPrefabAsync<T>(string path) where T : Object;
    }
}
