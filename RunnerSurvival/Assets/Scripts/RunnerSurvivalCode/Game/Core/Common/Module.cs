using System;

namespace RunnerSurvivalCode.Game.Core.Common {
    public abstract class Module : IDisposable {
        public abstract void Initialize();

        public abstract void Dispose();
    }

    public abstract class Module<T> : Module {
        protected readonly T _view;

        protected Module(T view) {
            _view = view;
        }
    }
}
