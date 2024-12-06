using System;
using RunnerSurvivalCode.Services.PoolFactory;
using UnityEngine;
using UnityEngine.UI;

namespace RunnerSurvivalCode.Game.States.Views {
    [Serializable]
    public class LobbyStateView {
        public Transform Screen;
        public ComponentPoolFactory ScrollViewItemPoolFactory;
        public Button PlayButton;
    }
}
