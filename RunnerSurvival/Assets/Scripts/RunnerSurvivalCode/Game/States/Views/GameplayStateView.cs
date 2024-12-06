using System;
using RunnerSurvivalCode.Services.PoolFactory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RunnerSurvivalCode.Game.States.Views {
    [Serializable]
    public class GameplayStateView {
        public Transform Screen;
        
        public TMP_Text AllScoreText;
        public TMP_Text AppleScoreText;
        public TMP_Text CornScoreText;
        public TMP_Text BananaScoreText;
        
        public Transform GameOverPopup;
        public Button GoToLobbyButton;

        public Button StartPlayButton;
        public Transform Player;
        
        public Transform ObtaclesSpawnParent;
        public ComponentPoolFactory ObstaclePoolFactory;
        
        public ComponentPoolFactory ApplePoolFactory;
        public ComponentPoolFactory BananaPoolFactory;
        public ComponentPoolFactory CornPoolFactory;
        
    }
}
