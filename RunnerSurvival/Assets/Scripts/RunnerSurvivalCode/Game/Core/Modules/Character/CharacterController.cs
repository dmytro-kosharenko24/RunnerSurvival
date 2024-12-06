using DG.Tweening;
using RunnerSurvivalCode.Game.Core.Common;
using RunnerSurvivalCode.Game.States.Views;
using UnityEngine;
using Zenject;

namespace RunnerSurvivalCode.Game.Core.Modules.Character {
    public class CharacterController : Module {
        public static float[] LinePositions = {
            -1.4f, 0.6f, 2.3f
        };

        private const float SwitchDuration = 0.2f;

        private readonly GameplayManager _gameplayManager;
        [Inject] private GameplayStateView _gameplayStateView;


        private int _currentLineIndex = 1;

        private Transform _transform;

        public CharacterController(GameplayManager gameplayManager) {
            _gameplayManager = gameplayManager;
        }

        public override void Initialize() {
            _transform = _gameplayStateView.Player.transform;
            _gameplayManager.OnGameStart += StartProgress;
            _gameplayManager.SwipeLeftAction += MoveLeft;
            _gameplayManager.SwipeRightAction += MoveRight;
            _gameplayManager.PlayerPositionIndex = _currentLineIndex;
        }

        public override void Dispose() {
            _gameplayManager.OnGameStart -= StartProgress;
            _gameplayManager.SwipeLeftAction -= MoveLeft;
            _gameplayManager.SwipeRightAction -= MoveRight;
            DOTween.Kill(this);
        }

        private void StartProgress() {
            if (LinePositions.Length != 3) {
                Debug.LogError("LineSwitcher: need 3 line positions");
                return;
            }

            _transform.position = new Vector3(LinePositions[_currentLineIndex], _transform.position.y, _transform.position.z);
        }

        private void MoveLeft() {
            if (_currentLineIndex > 0) {
                _currentLineIndex--;
                SwitchLine();
            }
        }

        private void MoveRight() {
            if (_currentLineIndex < LinePositions.Length - 1) {
                _currentLineIndex++;
                SwitchLine();
            }
        }

        private void SwitchLine() {
            float targetX = LinePositions[_currentLineIndex];
            _transform.DOMoveX(targetX, SwitchDuration).SetEase(Ease.OutQuad).SetId(this);
            _gameplayManager.PlayerPositionIndex = _currentLineIndex;
        }
    }
}
