using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RunnerSurvivalCode.Game.Core.Common {

    public abstract class BaseGameplayManager {
        private readonly List<Module> _levelModules;

        private DiContainer _container;

        protected BaseGameplayManager(DiContainer container) {
            _levelModules = new List<Module>();
            _container = container;
        }

        public virtual void Initialize() {
        }

        public virtual void Dispose() {
        }

        protected void AddModule<T>(params object[] args) where T : Module {
            var result = (T)Activator.CreateInstance(typeof(T), args);
            AddModule(result);
        }

        protected void AddModule(Module result) {
            _levelModules.Add(result);
            _container.Inject(result);
            result.Initialize();
        }

        protected T AddModule<T, T1>(MonoBehaviour moduleView) where T : Module {
            var view = moduleView.GetComponent<T1>();

            if (null == view) {
                return null;
            }

            var result = (T)Activator.CreateInstance(typeof(T), new object[] {
                view
            });

            _levelModules.Add(result);
            _container.Inject(result);
            result.Initialize();
            return result;
        }

        protected void DisposeModules() {
            foreach (var levelModule in _levelModules) {
                levelModule.Dispose();
            }

            _levelModules.Clear();
        }

        protected void DisposeModule<T>() {
            for (int i = _levelModules.Count - 1; i >= 0; i--) {
                if (_levelModules[i] is T) {
                    _levelModules[i].Dispose();
                    _levelModules.RemoveAt(i);
                }
            }
        }
    }
}
