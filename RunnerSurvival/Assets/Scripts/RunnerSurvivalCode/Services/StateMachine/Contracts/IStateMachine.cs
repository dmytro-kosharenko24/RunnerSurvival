namespace RunnerSurvivalCode.Services.StateMachine.Contracts {
    public interface IStateMachine {
        IState CurrentState { get; }
        void ChangeState(IState newState);
        void Update();
    }
}
