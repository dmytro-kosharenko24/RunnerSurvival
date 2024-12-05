using System.Collections.Generic;

namespace RunnerSurvivalCode.Services.StateMachine {
    public interface IStatesContainer {
        public List<IState> States { get; set; }
    }
}
