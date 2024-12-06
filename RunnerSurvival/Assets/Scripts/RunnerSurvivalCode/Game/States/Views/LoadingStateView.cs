using System;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace RunnerSurvivalCode.Game.States.Views {
    [Serializable]
    public class LoadingStateView {
        public Slider LoadingProgressBarSlider;
        public TMP_Text LoadingText;
        public Transform Screen;
    }
}
