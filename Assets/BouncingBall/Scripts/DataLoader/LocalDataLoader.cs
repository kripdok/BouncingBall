using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace BouncingBall.DataLoader
{
    public class LocalDataLoader : IDataLoader
    {
        public async UniTask<T> LoadDataAsync<T>(string path) where T : IDownloadable, new()
        {
            TaskCompletionSource<T> task = new TaskCompletionSource<T>();

            T data = new T();

            if (File.Exists(path) == false)
            {
                throw new FileNotFoundException($"{path} does not exist!");
            }

            try
            {
                string jsonData = await File.ReadAllTextAsync(path);
                data.Load(jsonData);
                task.SetResult(data);
                return task.Task.Result;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load data due to: {ex.Message} {ex.StackTrace}");
                throw ex;
            }
        }
    }
}
