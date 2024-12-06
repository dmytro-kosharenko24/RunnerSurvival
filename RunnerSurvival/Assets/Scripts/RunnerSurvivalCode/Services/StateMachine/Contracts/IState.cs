namespace RunnerSurvivalCode.Services.StateMachine.Contracts {
    public interface IState {
        void Enter();
        void Exit();
        void Update();
    }
}
