using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace BouncingBall.Scripts.Game
{
    public class SceneLoader
    {
        public async UniTask LoadScene(string sceneName, Action onLoader = null)
        {
            await UniTask.WaitForSeconds(1f);

            if(SceneManager.GetActiveScene().name == sceneName)
            {
                onLoader?.Invoke();
                return;
            }

            var operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
                await Task.Yield();

            onLoader?.Invoke();
        }

    }
}
