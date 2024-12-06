using System.Collections.Generic;
using System.Linq;
using RunnerSurvivalCode.Game.Core.Common;
using RunnerSurvivalCode.Game.States.Views;
using RunnerSurvivalCode.Services.Ticker;
using UnityEngine;
using Zenject;
using CharacterController = RunnerSurvivalCode.Game.Core.Modules.Character.CharacterController;

namespace RunnerSurvivalCode.Game.Core.Modules.Obstacles {
    public class ObstaclesController : Module {
        private const float CollisionDistance = 0.3f;

        private readonly GameplayManager _gameplayManager;
        private readonly System.Random _random;

        [Inject] private GameplayStateView _gameplayStateView;
        [Inject] private UnityTicker _unityTicker;

        private readonly List<ObstacleData> _liveObstacles = new List<ObstacleData>();

        //ToDo: remove this to server or configs that GD can change it for balance or different levels
        private readonly string[] _patterns = {
            "00x", "00x", "x0x", "0x0", "x00", "x00", "000",
        };

        public ObstaclesController(GameplayManager gameplayManager, System.Random random) {
            _gameplayManager = gameplayManager;
            _random = random;
        }

        public override void Initialize() {
            _unityTicker.Tick += Update;
            _gameplayManager.OnFullDistancePassed += SpawnPattern;

            _gameplayManager.SwipeLeftAction += CheckObstacles;
            _gameplayManager.SwipeRightAction += CheckObstacles;
        }

        public override void Dispose() {
            _unityTicker.Tick -= Update;
            _gameplayManager.OnFullDistancePassed -= SpawnPattern;

            _gameplayManager.SwipeLeftAction -= CheckObstacles;
            _gameplayManager.SwipeRightAction -= CheckObstacles;
            _gameplayStateView.ObstaclePoolFactory.ReleaseAllInstances();
        }

        private void Update(float deltaTime) {
            if (!_gameplayManager.IsStart) {
                return;
            }

            CheckObstacles();
        }

        private void SpawnPattern() {
            string selectedPattern = _patterns[_random.Next(0, _patterns.Length)];

            for (int i = 0; i < selectedPattern.Length; i++) {
                if (selectedPattern[i] == 'x') {
                    var obstacle = _gameplayStateView.ObstaclePoolFactory.Get<Transform>();

                    obstacle.position = new Vector3(CharacterController.LinePositions[i], 0f, GameplayManager.SpawnZPosition);
                    obstacle.gameObject.SetActive(true);

                    var data = new ObstacleData {
                        LineIndex = i,
                        Transform = obstacle
                    };

                    _liveObstacles.Add(data);
                }
            }
        }

        private void CheckObstacles() {
            foreach (var obstacle in _liveObstacles.ToList()) {
                if (_gameplayManager.PlayerPositionIndex == obstacle.LineIndex) {
                    var playerPosition = _gameplayStateView.Player.position;
                    var obstaclePosition = obstacle.Transform.position;

                    if (Mathf.Abs(playerPosition.z - obstaclePosition.z) < CollisionDistance) {
                        _gameplayManager.Lose();
                        return;
                    }
                }

                if (obstacle.Transform.position.z < _gameplayStateView.Player.position.z - 5f) {
                    obstacle.Transform.gameObject.SetActive(false);
                    _gameplayStateView.ObstaclePoolFactory.Release(obstacle.Transform);
                    _liveObstacles.Remove(obstacle);
                }
            }
        }
    }

    public class ObstacleData {
        public int LineIndex { get; set; }
        public Transform Transform { get; set; }
    }
}
