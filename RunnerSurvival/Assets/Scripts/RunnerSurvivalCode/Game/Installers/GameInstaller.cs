using RunnerSurvivalCode.Bootstraps;
using RunnerSurvivalCode.Game.Data;
using RunnerSurvivalCode.Game.States.Views;
using RunnerSurvivalCode.Services.AddressablesManager;
using RunnerSurvivalCode.Services.StateMachine;
using Services.AssetManager;
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
        }

        private void InstallData() {
            Container.Bind<UserDataContainer>().AsSingle();
        }

        private void InstallServices() {
            Container.Bind<JsonDataManager>().AsSingle();
            
            Container.Bind<IAssetManager>().To<AddressablesAssetManager>().AsSingle();
            
            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            
            var gameStatesContainer = new GameStatesContainer();
            Container.Bind<IStatesContainer>().FromInstance(gameStatesContainer).AsSingle();
            gameStatesContainer.CreateStates(Container);
        }
    }
}
