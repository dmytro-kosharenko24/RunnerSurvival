using RunnerSurvivalCode.Services.StateMachine.Contracts;

namespace RunnerSurvivalCode.Services.StateMachine {
    public class StateMachine : IStateMachine {
        private IState _currentState;

        public IState CurrentState => _currentState;
        
        //ToDo: redev on enum + dictionary (enum -> state)
        public void ChangeState(IState newState) {
            if (_currentState != null) {
                _currentState.Exit();
            }

            _currentState = newState;
            _currentState.Enter();
        }

        public void Update() {
            if (_currentState != null) {
                _currentState.Update();
            }
        }
        
    }
}
