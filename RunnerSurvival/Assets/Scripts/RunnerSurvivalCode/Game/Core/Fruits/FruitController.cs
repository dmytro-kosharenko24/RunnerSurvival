using System;
using System.Collections.Generic;
using RunnerSurvivalCode.Game.Core.Common;
using RunnerSurvivalCode.Game.States.Views;
using RunnerSurvivalCode.Services.Ticker;
using UnityEngine;
using Zenject;
using CharacterController = RunnerSurvivalCode.Game.Core.Modules.Character.CharacterController;
using Random = System.Random;

namespace RunnerSurvivalCode.Game.Core.Fruits {
    public class FruitController : Module {
        private const float CollisionDistance = 0.3f;

        private readonly GameplayManager _gameplayManager;
        private readonly Random _random;

        [Inject] private GameplayStateView _gameplayStateView;
        [Inject] private UnityTicker _unityTicker;

        private readonly List<FruitData> _liveFruits = new List<FruitData>();
        private readonly Dictionary<FruitType, int> _collectedFruits = new Dictionary<FruitType, int>() {
            {
                FruitType.Apple, 0
            }, {
                FruitType.Corn, 0
            }, {
                FruitType.Banana, 0
            }
        };

        //ToDo: remove this to server or configs that GD can change it for balance or different levels
        private readonly FruitType[] _fruitBalance = {
            FruitType.Apple, FruitType.Apple, FruitType.Apple, FruitType.Corn, FruitType.Corn, FruitType.Banana
        };

        public FruitController(GameplayManager gameplayManager, Random random) {
            _gameplayManager = gameplayManager;
            _random = random;
        }

        public override void Initialize() {
            _gameplayManager.OnHalfDistancePassed += SpawnFruit;
            _unityTicker.Tick += OnTick;

            _gameplayManager.SwipeLeftAction += CheckFruits;
            _gameplayManager.SwipeRightAction += CheckFruits;

            _gameplayStateView.AppleScoreText.text = "0";
            _gameplayStateView.BananaScoreText.text = "0";
            _gameplayStateView.CornScoreText.text = "0";
        }

        public override void Dispose() {
            _gameplayManager.OnHalfDistancePassed -= SpawnFruit;
            _unityTicker.Tick -= OnTick;

            _gameplayManager.SwipeLeftAction -= CheckFruits;
            _gameplayManager.SwipeRightAction -= CheckFruits;

            _gameplayStateView.ApplePoolFactory.ReleaseAllInstances();
            _gameplayStateView.BananaPoolFactory.ReleaseAllInstances();
            _gameplayStateView.CornPoolFactory.ReleaseAllInstances();
        }

        private void OnTick(float deltaTime) {
            if (!_gameplayManager.IsStart) {
                return;
            }

            CheckFruits();
        }

        private void SpawnFruit() {
            var selectedFruit = _fruitBalance[_random.Next(0, _fruitBalance.Length)];
            var randomLine = _random.Next(0, CharacterController.LinePositions.Length);

            Transform fruit = selectedFruit switch {
                FruitType.Apple => _gameplayStateView.ApplePoolFactory.Get<Transform>(),
                FruitType.Banana => _gameplayStateView.BananaPoolFactory.Get<Transform>(),
                FruitType.Corn => _gameplayStateView.CornPoolFactory.Get<Transform>(),
                _ => throw new ArgumentOutOfRangeException()
            };

            fruit.position = new Vector3(CharacterController.LinePositions[randomLine], 0f, GameplayManager.SpawnZPosition);
            _liveFruits.Add(new FruitData {
                LineIndex = randomLine,
                Type = selectedFruit,
                Transform = fruit
            });
        }

        private void CheckFruits() {
            foreach (var data in _liveFruits) {
                if (_gameplayManager.PlayerPositionIndex == data.LineIndex) {
                    var playerPosition = _gameplayStateView.Player.position;
                    var obstaclePosition = data.Transform.position;

                    if (Mathf.Abs(playerPosition.z - obstaclePosition.z) < CollisionDistance) {
                        CollectFruit(data);
                        _gameplayManager.CollectFruit(data.Type);
                        return;
                    }
                }

                if (data.Transform.position.z < _gameplayStateView.Player.position.z - 5f) {
                    data.Transform.gameObject.SetActive(false);
                    _liveFruits.Remove(data);
                    ReturnFruitToPool(data);
                    break;
                }
            }
        }

        private void ReturnFruitToPool(FruitData data) {
            switch (data.Type) {
            case FruitType.Apple:
                _gameplayStateView.ApplePoolFactory.Release(data.Transform);
                break;
            case FruitType.Banana:
                _gameplayStateView.BananaPoolFactory.Release(data.Transform);
                break;
            case FruitType.Corn:
                _gameplayStateView.CornPoolFactory.Release(data.Transform);
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        private void CollectFruit(FruitData data) {
            data.Transform.gameObject.SetActive(false);
            _liveFruits.Remove(data);
            ReturnFruitToPool(data);
            _collectedFruits[data.Type]++;

            switch (data.Type) {
            case FruitType.Apple:
                _gameplayStateView.AppleScoreText.text = _collectedFruits[FruitType.Apple].ToString();
                break;
            case FruitType.Banana:
                _gameplayStateView.BananaScoreText.text = _collectedFruits[FruitType.Banana].ToString();
                break;
            case FruitType.Corn:
                _gameplayStateView.CornScoreText.text = _collectedFruits[FruitType.Corn].ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }
    }

    public struct FruitData {
        public int LineIndex;
        public FruitType Type;
        public Transform Transform;
    }

    public enum FruitType {
        Apple,
        Banana,
        Corn,
    }
}
