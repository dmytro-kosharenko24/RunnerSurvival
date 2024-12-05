using System.Collections.Generic;
using RunnerSurvivalCode.Game.States;
using Zenject;

namespace RunnerSurvivalCode.Services.StateMachine {
    public partial class GameStatesContainer {
        public void CreateStates(DiContainer container) {
            States = new List<IState>();
            States.Add(new LoadingState());
            States.Add(new LobbyState());
            States.Add(new GameplayState());
            
            foreach (var state in States) {
                container.Inject(state);
            }
        }
    }
}
