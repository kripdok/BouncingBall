using Cysharp.Threading.Tasks;
using System;
using System.IO;
using UnityEngine;

namespace BouncingBall.DataLoader
{
    public class LocalDataLoader : IDataLoader
    {
        public async UniTask<T> LoadDataFromPathAsync<T>(string path) where T : IDownloadable, new()
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found at path: {path}");
            }

            T data = new T();

            try
            {
                string jsonContent = await File.ReadAllTextAsync(path);
                data.Load(jsonContent);
                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load data due to: {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}