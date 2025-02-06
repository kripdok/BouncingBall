using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.Scripts.Utilities.PrefabLoad
{
    public interface IPrefabLoadStrategy
    {
        public T LoadPrefab<T>(string patch) where T : MonoBehaviour;

        public UniTask<T> AsyncLoadPrefab<T>(string patch) where T : MonoBehaviour;
    }
}
