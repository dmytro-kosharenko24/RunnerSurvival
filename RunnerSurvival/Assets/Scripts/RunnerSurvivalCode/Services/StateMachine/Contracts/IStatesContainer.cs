using System.Collections.Generic;

namespace RunnerSurvivalCode.Services.StateMachine.Contracts {
    public interface IStatesContainer {
        public List<IState> States { get; set; }
    }
}
