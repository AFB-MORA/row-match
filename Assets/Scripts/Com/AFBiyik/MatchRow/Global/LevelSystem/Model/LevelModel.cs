namespace Com.AFBiyik.MatchRow.Global.LevelSystem
{
    public class LevelModel
    {
        public int LevelNumber { get; set; }
        public int GridWidth { get; set; }
        public int GridHeight { get; set; }
        public int MoveCount { get; set; }
        public string[] Grid { get; set; }
        public int HighScore { get; set; }
        public bool IsLocked { get; set; }
    }
}