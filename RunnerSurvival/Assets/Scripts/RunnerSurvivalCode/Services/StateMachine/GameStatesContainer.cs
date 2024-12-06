using System.Collections.Generic;
using RunnerSurvivalCode.Services.StateMachine.Contracts;

namespace RunnerSurvivalCode.Services.StateMachine {
    public partial class GameStatesContainer : IStatesContainer {
        public List<IState> States { get; set; }
    }
}
