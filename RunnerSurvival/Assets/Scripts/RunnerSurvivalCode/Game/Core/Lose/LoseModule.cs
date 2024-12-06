using RunnerSurvivalCode.Game.Core.Common;
using RunnerSurvivalCode.Game.States.Views;
using Zenject;

namespace RunnerSurvivalCode.Game.Core.Lose {
    public class LoseModule : Module {
        [Inject] private GameplayStateView _gameplayStateView;

        private readonly GameplayManager _gameplayManager;

        public LoseModule(GameplayManager gameplayManager) {
            _gameplayManager = gameplayManager;
        }

        public override void Initialize() {
            _gameplayManager.OnLose += OnLose;
            _gameplayStateView.GoToLobbyButton.onClick.AddListener(OnGoToLobbyButtonClick);
        }

        public override void Dispose() {
            _gameplayManager.OnLose -= OnLose;
            _gameplayStateView.GoToLobbyButton.onClick.RemoveListener(OnGoToLobbyButtonClick);
            _gameplayStateView.GameOverPopup.gameObject.SetActive(false);
        }

        private void OnLose() {
            _gameplayStateView.GameOverPopup.gameObject.SetActive(true);
        }

        private void OnGoToLobbyButtonClick() {
            _gameplayStateView.GameOverPopup.gameObject.SetActive(false);
            _gameplayManager.GoToLobby();
        }
    }
}
