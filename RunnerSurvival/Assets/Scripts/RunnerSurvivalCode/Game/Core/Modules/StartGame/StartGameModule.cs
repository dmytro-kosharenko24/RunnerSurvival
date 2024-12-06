using RunnerSurvivalCode.Game.Core.Common;
using RunnerSurvivalCode.Game.States;
using RunnerSurvivalCode.Game.States.Views;
using Zenject;

namespace RunnerSurvivalCode.Game.Core.Modules.StartGame {
    public class StartGameModule : Module {
        private readonly GameplayManager _gameplayManager;
        [Inject] private GameplayStateView _gameplayStateView;

        public StartGameModule(GameplayManager gameplayManager) {
            _gameplayManager = gameplayManager;
        }

        public override void Initialize() {
            _gameplayStateView.StartPlayButton.onClick.AddListener(OnStartPlayButtonClicked);
            _gameplayStateView.StartPlayButton.gameObject.SetActive(true);
            _gameplayStateView.Player.gameObject.SetActive(false);
        }

        public override void Dispose() {
            _gameplayStateView.StartPlayButton.onClick.RemoveListener(OnStartPlayButtonClicked);
        }

        private void OnStartPlayButtonClicked() {
            _gameplayStateView.StartPlayButton.gameObject.SetActive(false);
            _gameplayStateView.Player.gameObject.SetActive(true);
            _gameplayManager.StartGame();
        }
    }
}
