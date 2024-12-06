using RunnerSurvivalCode.Game.Core.Common;
using RunnerSurvivalCode.Game.States;
using RunnerSurvivalCode.Game.States.Views;
using RunnerSurvivalCode.Services.Ticker;
using UnityEngine;
using Zenject;

namespace RunnerSurvivalCode.Game.Core.ObjectsMover {
    public class ObjectsMoverModule : Module {
        private const float SpawnDistance = 10f;
        private const float MoveSpeed = 10f;
        private const float StartOffset = 20.4f;

        private readonly GameplayManager _gameplayManager;

        [Inject] private GameplayStateView _gameplayStateView;
        [Inject] private UnityTicker _unityTicker;

        private float _lastParentZPosition;
        private float _startParentZPosition;
        private bool _isHalfPassed;

        public ObjectsMoverModule(GameplayManager gameplayManager) {
            _gameplayManager = gameplayManager;
        }

        public override void Initialize() {
            _unityTicker.Tick += Update;
            _gameplayStateView.ObtaclesSpawnParent.position = new Vector3(0, 0, StartOffset);
            _startParentZPosition = _gameplayStateView.ObtaclesSpawnParent.position.z;
        }

        public override void Dispose() {
            _unityTicker.Tick -= Update;
        }

        private void Update(float deltaTime) {
            if (!_gameplayManager.IsStart) {
                return;
            }

            MoveObstacles(deltaTime);
            CheckAndSpawn();
        }

        private void MoveObstacles(float deltaTime) {
            if (_gameplayStateView.ObtaclesSpawnParent != null) {
                _gameplayStateView.ObtaclesSpawnParent.Translate(Vector3.back * MoveSpeed * deltaTime);
            }

            _lastParentZPosition = _gameplayStateView.ObtaclesSpawnParent.position.z;
        }

        private void CheckAndSpawn() {
            if (Mathf.Abs(_lastParentZPosition - _startParentZPosition) >= SpawnDistance / 2) {
                if (!_isHalfPassed) {
                    _gameplayManager.HalfDistancePassed();
                    _isHalfPassed = true; 
                }
            }

            if (Mathf.Abs(_lastParentZPosition - _startParentZPosition) >= SpawnDistance) {
                _gameplayManager.FullDistancePassed();
                _startParentZPosition = _gameplayStateView.ObtaclesSpawnParent.position.z;
                _isHalfPassed = false;
            }
        }
    }
}
