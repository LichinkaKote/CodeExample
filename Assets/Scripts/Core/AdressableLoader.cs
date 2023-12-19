using RSG;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Core
{
    public static class AdressableLoader
    {
        public static Promise<T> LoadAssetAsync<T>(string assetPath)
        {
            Debug.Log("AdressableLoader > " + assetPath);
            var result = new Promise<T>();
            var handle = Addressables.LoadAssetAsync<T>(assetPath);
            handle.Completed += (h) =>
            {
                Debug.Log("Done > " + assetPath);
                result.Resolve(h.Result);
            };

            return result;
        }

        public static Promise<T> LoadTextAssetAsync<T>(string assetPath)
        {
            var result = new Promise<T>();
            Debug.Log("AdressableLoader > " + assetPath);
            Addressables.LoadAssetAsync<TextAsset>(assetPath).Completed +=
                (asset) =>
                {
                    if (!(asset.Result == null))
                    {
                        Debug.Log("Done > " + assetPath);
                        result.Resolve(FileManager.LoadObject<T>(asset.Result.text));
                    }
                    else
                    {
                        Debug.Log("AdressableLoader failed > " + assetPath);
                        result.Reject(null);
                    }
                };

            return result;
        }
        public static Task<T> LoadAssetAsyncTask<T>(string path)
        {
            Debug.Log("AdressableLoader > " + path);
            var handle = Addressables.LoadAssetAsync<T>(path);
            handle.Completed += (h) =>
            {
                if (handle.Status.HasFlag(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed))
                {
                    Debug.Log("AdressableLoader failed > " + path);
                }
                else
                    Debug.Log("Done > " + path);
            };

            return handle.Task;
        }
        public async static Task<T> LoadTextAssetAsyncTask<T>(string path)
        {
            Debug.Log("AdressableLoader > " + path);
            var handle = Addressables.LoadAssetAsync<TextAsset>(path);
            handle.Completed += (h) =>
            {
                if (handle.Status.HasFlag(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed))
                {
                    Debug.Log("AdressableLoader failed > " + path);
                }
                else
                {
                    Debug.Log("Done > " + path);
                }
            };

            await handle.Task;
            return FileManager.LoadObject<T>(handle.Task.Result.text);
        }
    }
}