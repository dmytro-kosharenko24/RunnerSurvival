using System.Collections.Generic;
using System.Linq;
using Game.UI.Pool;
using UnityEngine;

namespace RunnerSurvivalCode.Services.PoolFactory {
    public class ComponentPoolFactory : MonoBehaviour, IComponentPoolFactory {
        public GameObject prefab;
        public int count;
        public Transform content;
        public Transform poolStorage;

        private readonly HashSet<GameObject> _instances;
        private Queue<GameObject> _pool;

        public Transform Content { get { return content; } }

        public ComponentPoolFactory() {
            _instances = new HashSet<GameObject>();
            _pool = new Queue<GameObject>();
        }

        public int CountInstances {
            get { return _instances.Count; }
        }

        private void Awake() {
            if (_instances.Count > 0)
                return;

            for (int i = 0; i < count; i++) {
                Get<Transform>();
            }
            ReleaseAllInstances();
        }

        public T Get<T>() where T : Component {
            return Get<T>(_instances.Count);
        }

        public T Get<T>(int sublingIndex) where T : Component {
            bool isNewInstance = false;
            if (_pool.Count == 0) {
                GameObject result = Instantiate(prefab);

                if (null == result)
                    return null;

                _pool.Enqueue(result);
                isNewInstance = true;
            }

            T resultComponent = _pool.Dequeue().GetComponent<T>();
            if (null == resultComponent) {
                return resultComponent;
            }

            var go = resultComponent.gameObject;
            var t = resultComponent.transform;
            if (isNewInstance || (poolStorage != null && poolStorage != content)) {
                t.SetParent(content, false);
            }

            _instances.Add(go);

            if (!go.activeSelf) {
                go.SetActive(true);
            }

            if (t.GetSiblingIndex() != sublingIndex) {
                t.SetSiblingIndex(sublingIndex);
            }

            return resultComponent;
        }

        public void Release<T>(T component) where T : Component {
            var go = component.gameObject;
            if (_instances.Contains(go)) {
                go.SetActive(false);
                if (poolStorage) {
                    go.transform.SetParent(poolStorage, false);
                }
                _pool.Enqueue(go);
                _instances.Remove(go);
            }
        }

        public void ReleaseAllInstances() {
            foreach (GameObject instance in _instances) {
                instance.SetActive(false);
                if (poolStorage) {
                    instance.transform.SetParent(poolStorage, false);
                }
                _pool.Enqueue(instance);
            }
            _instances.Clear();
        }

        public void PutInstancesToPool() {
            _pool = new Queue<GameObject>(_instances.Union(_pool));
            _instances.Clear();
        }

        public void HideUnusedInstances() {
            foreach (GameObject instance in _pool) {
                instance.SetActive(false);
            }
        }

        public void Dispose() {
            ReleaseAllInstances();

            foreach (GameObject gameObject in _pool) {
                Destroy(gameObject);
            }
            _pool.Clear();
        }
    }
}
