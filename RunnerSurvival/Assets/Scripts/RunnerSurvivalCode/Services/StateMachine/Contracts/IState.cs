namespace RunnerSurvivalCode.Services.StateMachine {
    public interface IState {
        void Enter();
        void Exit();
        void Update();
    }
}
