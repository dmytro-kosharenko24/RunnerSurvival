using System.Collections.Generic;

namespace RunnerSurvivalCode.Services.StateMachine {
    public partial class GameStatesContainer : IStatesContainer {
        public List<IState> States { get; set; }
    }
}
