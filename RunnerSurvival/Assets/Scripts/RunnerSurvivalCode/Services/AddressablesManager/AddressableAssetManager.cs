using System;
using System.Collections.Generic;
using RunnerSurvivalCode.Services.AddressablesManager;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.AssetManager {
    public class AddressablesAssetManager : IAssetManager {
        private const string Label = "Objects";

        private Dictionary<string, GameObject> _loadedAssetsMap = new();
        public Action OnAssetsLoadSuccess { get; set; }
        public Action OnAssetsLoadFail { get; set; }

        public void LoadAllAssets() {
            LoadAssetsByLabel(Label);
        }

        public GameObject GetAsset(string key) {
            return _loadedAssetsMap[key];
        }

        public void LoadAssetsByLabel(string label) {
            Addressables.LoadAssetsAsync<GameObject>(label, OnAssetLoaded).Completed += OnLoadCompleted;
        }

        public List<string> GetAllCharacters() {
            return new List<string>(_loadedAssetsMap.Keys);
        }

        //ToDo: connect when needed
        public void ReleaseAssets() {
            foreach (GameObject asset in _loadedAssetsMap.Values) {
                Addressables.Release(asset);
            }

            _loadedAssetsMap.Clear();
        }

        private void OnAssetLoaded(GameObject loadedAsset) {
            _loadedAssetsMap.Add(loadedAsset.name, loadedAsset);
        }

        private void OnLoadCompleted(AsyncOperationHandle<IList<GameObject>> handle) {
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                OnAssetsLoadSuccess?.Invoke();
            }
            else {
                OnAssetsLoadFail?.Invoke();
            }
        }
    }

}
