using RunnerSurvivalCode.Game.Data;
using RunnerSurvivalCode.Game.States.Views;
using RunnerSurvivalCode.Services.StateMachine;
using RunnerSurvivalCode.UI.GameInfo;
using UnityEngine;
using Zenject;

namespace RunnerSurvivalCode.Game.States {
    public class LobbyState : IState{

        [Inject] private UserDataContainer _userDataContainer;
        [Inject] private LobbyStateView _lobbyStateView;
        [Inject] private IStateMachine _stateMachine;
        [Inject] private IStatesContainer _statesContainer;
        
        public void Enter() {
            _lobbyStateView.Screen.gameObject.SetActive(true);
            CreateScrollViewItems();
            BindEvents();
        }

        public void Exit() {
            _lobbyStateView.Screen.gameObject.SetActive(false);
            _lobbyStateView.ScrollViewItemPoolFactory.ReleaseAllInstances();
            UnbindEvents();
        }

        public void Update() {
        }
        
        //Todo: show only top 10 games for optimization
        private void CreateScrollViewItems() {
            for (int i = 0; i < _userDataContainer.GamesInfo.Count; i++) {
                var scrollViewItem = _lobbyStateView.ScrollViewItemPoolFactory.Get<Transform>();
                var view = scrollViewItem.GetComponent<GameInfoItemView>();
                view.GameScoreText.text = _userDataContainer.GamesInfo[i].Score.ToString();
                view.GameIndexText.text = (i+1).ToString();
            }
        }
        
        private void BindEvents() {
            _lobbyStateView.PlayButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void UnbindEvents() {
            _lobbyStateView.PlayButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
        
        private void OnPlayButtonClicked() {
            _stateMachine.ChangeState(_statesContainer.States.Find(s => s.GetType() == typeof(GameplayState)));
        }
    }
}
