using DG.Tweening;
using RunnerSurvivalCode.Game.Data;
using RunnerSurvivalCode.Game.States.Views;
using RunnerSurvivalCode.Services.SaveManager;
using RunnerSurvivalCode.Services.StateMachine.Contracts;
using UnityEngine;
using Zenject;

namespace RunnerSurvivalCode.Game.States {
    public class LoadingState : IState {

        private const float MinimalLoadingTime = 3f;
        private const float StandardLoadingCoefficient = 0.33f;
        private const float TweenTime = 0.2f;

        [Inject] private IStateMachine _stateMachine;
        [Inject] private IStatesContainer _statesContainer;
        [Inject] private LoadingStateView _loadingStateView;
        [Inject] private JsonDataManager _jsonDataManager;
        [Inject] private UserDataContainer _userDataContainer;

        private float _startTime;
        private int _currentStep;

        public void Enter() {
            _startTime = Time.time;
            _loadingStateView.Screen.gameObject.SetActive(true);
            _loadingStateView.LoadingProgressBarSlider.value = 0;
            DoNextStep();
        }

        public void Exit() {
            DOTween.Kill(this);
            _loadingStateView.Screen.gameObject.SetActive(false);
        }

        public void Update() {
        }

        private void OnExit() {
            _stateMachine.ChangeState(_statesContainer.States.Find(s => s.GetType() == typeof(LobbyState)));
        }

        private void DoNextStep() {
            switch (_currentStep) {
            case 0:
                LoadingData();
                break;
            case 1:
                AdditionalLoading();
                break;
            }
        }

        private void LoadingData() {
            _loadingStateView.LoadingText.text = "Loading data...";
            UserDataContainer data = _jsonDataManager.LoadFromJson<UserDataContainer>(ProjectConsts.SavesPath);

            if (data == null) {
                Debug.Log("Data is null (probably first launch)");
                data = new UserDataContainer();
                _jsonDataManager.SaveToJson(ProjectConsts.SavesPath, data);
            }

            _userDataContainer.CopyFrom(data);
            _currentStep++;
            AddProgressByTween(StandardLoadingCoefficient * _currentStep);
        }

        private void AdditionalLoading() {
            float loadingTime = Time.time - _startTime;
            _loadingStateView.LoadingText.text = "Additional loading...";
            if (loadingTime < MinimalLoadingTime) {
                _loadingStateView.LoadingProgressBarSlider.DOValue(1f, MinimalLoadingTime - loadingTime)
                    .OnComplete(OnExit).SetId(this);
            } else {
                _loadingStateView.LoadingProgressBarSlider.DOValue(1f, TweenTime)
                    .OnComplete(OnExit).SetId(this);
            }
        }

        private void AddProgressByTween(float targetValue) {
            _loadingStateView.LoadingProgressBarSlider.DOValue(targetValue, TweenTime)
                .OnComplete(DoNextStep).SetId(this);
        }
    }
}
