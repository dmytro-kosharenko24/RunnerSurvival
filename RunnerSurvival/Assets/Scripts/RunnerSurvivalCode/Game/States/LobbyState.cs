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
        
        public void Enter() {
            _lobbyStateView.Screen.gameObject.SetActive(true);
            CreateScrollViewItems();
        }

        public void Exit() {
            _lobbyStateView.Screen.gameObject.SetActive(false);
            _lobbyStateView.ScrollViewItemPoolFactory.ReleaseAllInstances();
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
    }
}
