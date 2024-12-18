using RunnerSurvivalCode.Bootstraps;
using RunnerSurvivalCode.Game.Data;
using RunnerSurvivalCode.Game.States.Views;
using RunnerSurvivalCode.Services.SaveManager;
using RunnerSurvivalCode.Services.StateMachine;
using RunnerSurvivalCode.Services.StateMachine.Contracts;
using RunnerSurvivalCode.Services.Ticker;
using Zenject;

namespace RunnerSurvivalCode.Game.Installers {
    public class GameInstaller : MonoInstaller {
        public MainStatesView MainStatesView;
        public GameBootstrap GameBootstrap;
        
        public override void InstallBindings() {
            InstallViews();
            InstallData();
            InstallServices();
            
            Container.Inject(GameBootstrap);
            
            GameBootstrap.StartGame();
        }
        
        private void InstallViews() {
            Container.Bind<LoadingStateView>().FromInstance(MainStatesView.LoadingStateView).AsSingle();
            Container.Bind<LobbyStateView>().FromInstance(MainStatesView.LobbyStateView).AsSingle();
            Container.Bind<GameplayStateView>().FromInstance(MainStatesView.GameplayStateView).AsSingle();
        }

        private void InstallData() {
            Container.Bind<UserDataContainer>().AsSingle();
        }

        private void InstallServices() {
            Container.Bind<IDataManager>().To<JsonDataManager>().AsSingle();
            
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            
            var gameStatesContainer = new GameStatesContainer();
            Container.Bind<IStatesContainer>().FromInstance(gameStatesContainer).AsSingle();
            gameStatesContainer.CreateStates(Container);

            Container.Bind<UnityTicker>().AsSingle();
        }
    }
}
