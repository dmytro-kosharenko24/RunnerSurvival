using RunnerSurvivalCode.Game.Core.Common;
using RunnerSurvivalCode.Services.Ticker;
using UnityEngine;
using Zenject;

namespace RunnerSurvivalCode.Game.Core.Modules.Input {
    public class InputModule : Module {
        private readonly GameplayManager _gameplayManager;
        private const float MinSwipeDistance = 50f;

        [Inject] private UnityTicker _unityTicker;

        private Vector2 _startTouchPosition;
        private Vector2 _currentTouchPosition;
        private bool _isSwiping;

        public InputModule(GameplayManager gameplayManager) {
            _gameplayManager = gameplayManager;
        }

        public override void Initialize() {
            _unityTicker.Tick += Update;
        }

        public override void Dispose() {
            _unityTicker.Tick -= Update;
        }

        private void Update(float delta) {
            if (!_gameplayManager.IsStart) {
                return;
            }

            HandleSwipeInput();
        }

        private void HandleSwipeInput() {
            if (UnityEngine.Input.touchCount > 0) {
                Touch touch = UnityEngine.Input.GetTouch(0);

                switch (touch.phase) {
                case TouchPhase.Began:
                    _startTouchPosition = touch.position;
                    _isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    if (_isSwiping) {
                        _currentTouchPosition = touch.position;
                        CheckSwipe();
                    }

                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    _isSwiping = false;
                    break;
                }
            }
        }

        private void CheckSwipe() {
            Vector2 swipeDelta = _currentTouchPosition - _startTouchPosition;

            if (swipeDelta.magnitude >= MinSwipeDistance) {
                float x = swipeDelta.x;
                float y = swipeDelta.y;

                if (Mathf.Abs(x) > Mathf.Abs(y)) {
                    if (x > 0) {
                        _gameplayManager.SwipeRight();
                    } else {
                        _gameplayManager.SwipeLeft();
                    }
                }

                _isSwiping = false;
            }
        }
    }
}
