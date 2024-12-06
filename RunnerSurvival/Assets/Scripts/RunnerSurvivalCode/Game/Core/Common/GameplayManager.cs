using System;
using RunnerSurvivalCode.Game.Core.Fruits;
using RunnerSurvivalCode.Game.Core.Lose;
using RunnerSurvivalCode.Game.Core.Modules.Character;
using RunnerSurvivalCode.Game.Core.Modules.Input;
using RunnerSurvivalCode.Game.Core.Modules.Obstacles;
using RunnerSurvivalCode.Game.Core.Modules.StartGame;
using RunnerSurvivalCode.Game.Core.ObjectsMover;
using RunnerSurvivalCode.Game.Core.Score;
using RunnerSurvivalCode.Game.States.Views;
using Zenject;

namespace RunnerSurvivalCode.Game.Core.Common {
    public class GameplayManager : BaseGameplayManager {
        public const float SpawnZPosition = 10f;

        private readonly GameplayStateView _view;
        private readonly Action _loseAction;
        private readonly Random _random;
        
        public int PlayerPositionIndex;
        
        public Action SwipeRightAction;
        public Action SwipeLeftAction;
        public Action OnGameStart;
        public Action OnHalfDistancePassed;
        public Action OnFullDistancePassed;
        public Action<FruitType> OnFruitCollected;
        public Action OnLose;

        public bool IsStart { get; private set; }
        public int Score { get; private set; }
        
        public GameplayManager(DiContainer container, GameplayStateView view, int seed, Action loseAction) : base(container) {
            _view = view;
            _loseAction = loseAction;
            
            _random = new Random(seed);
            
            container.Inject(this);
        }

        public override void Initialize() {
            AddModule<StartGameModule>(this);
            AddModule<InputModule>(this);
            AddModule<CharacterController>(this);
            AddModule<ObstaclesController>(this, _random);
            AddModule<ObjectsMoverModule>(this);
            AddModule<FruitController>(this, _random);
            AddModule<ScoreModule>(this);
            AddModule<LoseModule>(this);
            
            _view.Screen.gameObject.SetActive(true);
        }
        
        public override void Dispose() {
            DisposeModules();
            _view.Screen.gameObject.SetActive(false);
        }
        
        public void SwipeRight() {
            SwipeRightAction?.Invoke();
        }
        
        public void SwipeLeft() {
            SwipeLeftAction?.Invoke();
        }
        
        public void StartGame() {
            IsStart = true;
            OnGameStart?.Invoke();
        }
        
        public void HalfDistancePassed() {
            OnHalfDistancePassed?.Invoke();
        }
        
        public void FullDistancePassed() {
            OnFullDistancePassed?.Invoke();
        }
        
        public void Lose() {
            IsStart = false;
            OnLose?.Invoke();
        }
        
        public void CollectFruit(FruitType type) {
            OnFruitCollected?.Invoke(type);
        }
        
        public void AddScore(int points) {
            Score += points;
        }
        
        public void GoToLobby() {
            _loseAction?.Invoke();
        }
    }
}
