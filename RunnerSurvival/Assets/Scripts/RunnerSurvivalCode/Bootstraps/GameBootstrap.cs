using RunnerSurvivalCode.Services.StateMachine.Contracts;
using RunnerSurvivalCode.Services.Ticker;
using UnityEngine;
using Zenject;

namespace RunnerSurvivalCode.Bootstraps {
    public class GameBootstrap : MonoBehaviour{

        [Inject] private IStatesContainer _statesContainer;
        [Inject] private IStateMachine _stateMachine;
        [Inject] private UnityTicker _unityTicker;
        
        private void Update() {
            _unityTicker.Update(Time.deltaTime);
        }
        
        public void StartGame() {
            _stateMachine.ChangeState(_statesContainer.States[0]);
        }
    }
}
