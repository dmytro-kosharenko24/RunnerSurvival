using RunnerSurvivalCode.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace RunnerSurvivalCode.Bootstraps {
    public class GameBootstrap : MonoBehaviour{

        [Inject] private IStatesContainer _statesContainer;
        [Inject] private IStateMachine _stateMachine;
        
        public void StartGame() {
            _stateMachine.ChangeState(_statesContainer.States[0]);
        }
    }
}
