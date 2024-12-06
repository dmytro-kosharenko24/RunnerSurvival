using System;

namespace RunnerSurvivalCode.Services.Ticker {
    public class UnityTicker {
        public Action<float> Tick { get; set; }
        
        public void Update(float deltaTime) {
            Tick?.Invoke(deltaTime);
        }
    }
}
