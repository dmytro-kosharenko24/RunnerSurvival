using System.Collections.Generic;

namespace RunnerSurvivalCode.Game.Data {
    public class UserDataContainer {
        public readonly List<GamesInfo> GamesInfo = new();

        public void AddGameInfo(int score, int seed) {
            var newSession = new GamesInfo {
                Score = score,
                Seed = seed
            };

            int index = GamesInfo.BinarySearch(newSession, new ScoreComparer());
            if (index < 0) index = ~index;
            GamesInfo.Insert(index, newSession);
        }

    }

    public class ScoreComparer : IComparer<GamesInfo> {
        public int Compare(GamesInfo x, GamesInfo y) {
            if (y != null && x != null) {
                return y.Score.CompareTo(x.Score);
            }
            
            return 0;
        }
    }

    public class GamesInfo {
        public int Score { get; set; }

        //ToDo: think about the best way to save seed (get from server, random, etc.)
        public int Seed { get; set; }
    }
}
