using System.Collections.Generic;
using RunnerSurvivalCode.Game.States;
using RunnerSurvivalCode.Services.StateMachine.Contracts;
using Zenject;

namespace RunnerSurvivalCode.Services.StateMachine {
    public partial class GameStatesContainer {
        public void CreateStates(DiContainer container) {
            States = new List<IState> {
                new LoadingState(),
                new LobbyState(),
                new GameplayState(container)
            };

            foreach (var state in States) {
                container.Inject(state);
            }
        }
    }
}
