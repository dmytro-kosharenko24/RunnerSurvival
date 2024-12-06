namespace RunnerSurvivalCode.Services.SaveManager {
    public interface IDataManager {
        public void Save<T>(string key, T data);
        public T Load<T>(string key);
    }
}
