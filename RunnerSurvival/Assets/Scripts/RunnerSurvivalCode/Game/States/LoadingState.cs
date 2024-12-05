using DG.Tweening;
using RunnerSurvivalCode.Game.Data;
using RunnerSurvivalCode.Game.States.Views;
using RunnerSurvivalCode.Services.AddressablesManager;
using RunnerSurvivalCode.Services.StateMachine;
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
        [Inject] private IAssetManager _assetManager;
        [Inject] private JsonDataManager _jsonDataManager;
        [Inject] private UserDataContainer _userDataContainer;

        private float _startTime;
        private int _currentStep;

        public void Enter() {
            _startTime = Time.time;
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
                LoadingResources();
                break;
            case 1:
                LoadingData();
                break;
            case 2:
                AdditionalLoading();
                break;
            }
        }

        private void LoadingResources() {
            _assetManager.OnAssetsLoadSuccess += OnAssetsLoadSuccess;
            _assetManager.OnAssetsLoadFail += OnAssetsLoadFail;
            _loadingStateView.LoadingText.text = "Loading resources...";
            _assetManager.LoadAllAssets();
        }

        private void OnAssetsLoadSuccess() {
            _assetManager.OnAssetsLoadSuccess -= OnAssetsLoadSuccess;
            _assetManager.OnAssetsLoadFail -= OnAssetsLoadFail;
            _currentStep++;
            AddProgressByTween(StandardLoadingCoefficient * _currentStep);
        }

        private void OnAssetsLoadFail() {
            _assetManager.OnAssetsLoadSuccess -= OnAssetsLoadSuccess;
            _assetManager.OnAssetsLoadFail -= OnAssetsLoadFail;
            _loadingStateView.LoadingText.text = "Loading failed...";
        }

        private void LoadingData() {
            _loadingStateView.LoadingText.text = "Loading data...";
            UserDataContainer data = _jsonDataManager.LoadFromJson<UserDataContainer>(ProjectConsts.SavesPath);

            if (data == null) {
                Debug.Log("Data is null (probably first launch)");
                data = new UserDataContainer();
                _jsonDataManager.SaveToJson(ProjectConsts.SavesPath, data);
            }

            _userDataContainer = data;
            _currentStep++;
            AddProgressByTween(StandardLoadingCoefficient * _currentStep);
        }

        private void AdditionalLoading() {
            float loadingTime = Time.time - _startTime;
            _loadingStateView.LoadingText.text = "Additional loading...";
            if (loadingTime < MinimalLoadingTime) {
                _loadingStateView.LoadingProgressBarSlider.DOValue(1f, MinimalLoadingTime - loadingTime)
                    .OnComplete(OnExit).SetId(this);
            }
            else {
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