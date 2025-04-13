namespace GhostPlugin.API.Map
{
    public enum CellType
    {
        Wall,
        Path,
        Player,
    }
    public class MapGrid
    {
        public CellType[,] Grid = new CellType[32, 32]; // 예시 크기

        public void UpdatePlayerPosition(int x, int y)
        {
            // 기존 플레이어 위치 제거
            for (int i = 0; i < 32; i++)
            for (int j = 0; j < 32; j++)
                if (Grid[i, j] == CellType.Player)
                    Grid[i, j] = CellType.Path;

            Grid[x, y] = CellType.Player;
        }
    }
}