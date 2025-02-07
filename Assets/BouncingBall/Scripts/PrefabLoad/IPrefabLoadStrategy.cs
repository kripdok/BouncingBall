using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Utilities.PrefabLoad
{
    public interface IPrefabLoadStrategy
    {
        public T LoadPrefab<T>(string patch) where T : Object;

        public UniTask<T> AsyncLoadPrefab<T>(string patch) where T : Object;
    }
}
