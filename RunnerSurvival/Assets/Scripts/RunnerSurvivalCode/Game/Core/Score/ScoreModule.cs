using System.Collections.Generic;
using RunnerSurvivalCode.Game.Core.Common;
using RunnerSurvivalCode.Game.Core.Fruits;
using RunnerSurvivalCode.Game.States;
using RunnerSurvivalCode.Game.States.Views;
using Zenject;

namespace RunnerSurvivalCode.Game.Core.Score {
    public class ScoreModule : Module {
        [Inject] private GameplayStateView _gameplayStateView;
        
        private readonly GameplayManager _gameplayManager;

        private readonly Dictionary<FruitType, int> _fruitToPointsMap = new Dictionary<FruitType, int>() {
            {
                FruitType.Apple, 1
            }, {
                FruitType.Corn, 2
            }, {
                FruitType.Banana, 3
            }
        };

        public ScoreModule(GameplayManager gameplayManager) {
            _gameplayManager = gameplayManager;
        }

        public override void Initialize() {
            _gameplayManager.OnFruitCollected += OnFruitCollected;
            _gameplayStateView.AllScoreText.text = _gameplayManager.Score.ToString();
        }

        public override void Dispose() {
            _gameplayManager.OnFruitCollected -= OnFruitCollected;
        }

        private void OnFruitCollected(FruitType fruit) {
            if (_fruitToPointsMap.TryGetValue(fruit, out int points)) {
                AddScore(points);
            }
        }
        
        private void AddScore(int points) {
            _gameplayManager.AddScore(points);
            _gameplayStateView.AllScoreText.text = _gameplayManager.Score.ToString();
        }
    }
}
