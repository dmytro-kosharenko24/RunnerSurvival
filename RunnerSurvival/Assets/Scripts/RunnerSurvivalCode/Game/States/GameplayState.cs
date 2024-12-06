using RunnerSurvivalCode.Game.Core.Common;
using RunnerSurvivalCode.Game.Data;
using RunnerSurvivalCode.Game.States.Views;
using RunnerSurvivalCode.Services.SaveManager;
using RunnerSurvivalCode.Services.StateMachine.Contracts;
using Zenject;

namespace RunnerSurvivalCode.Game.States {
    public class GameplayState : IState {
        [Inject] private GameplayStateView _gameplayStateView;
        [Inject] private UserDataContainer _userDataContainer;
        [Inject] private IStateMachine _stateMachine;
        [Inject] private IStatesContainer _statesContainer;
        [Inject] private IDataManager _jsonDataManager;

        private readonly DiContainer _container;
        private readonly int[] _seeds = {
            1, 2, 3, 4, 5
        };

        private GameplayManager _gameplayManager;
        private int _seed;

        public GameplayState(DiContainer container) {
            _container = container;
        }

        public void Enter() {
            CreateGameplayManager();
            _seed = _seeds[UnityEngine.Random.Range(0, _seeds.Length)];
        }

        public void Exit() {
            _gameplayManager.Dispose();
        }

        public void Update() {
        }

        private void OnLose() {
            _userDataContainer.AddGameInfo(_gameplayManager.Score, _seed);
            _jsonDataManager.Save(ProjectConsts.SavesPath, _userDataContainer);
            _stateMachine.ChangeState(_statesContainer.States.Find(s => s.GetType() == typeof(LobbyState)));
        }

        private void CreateGameplayManager() {
            _gameplayManager = new GameplayManager(_container, _gameplayStateView, _seed, OnLose);
            _gameplayManager.Initialize();
        }
    }
}
