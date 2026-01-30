using Godot;

public partial class DrawGridView : Node2D
{
    [Export] public int CellSize = 8;

    private bool[,] grid;

    public void SetGrid(bool[,] newGrid)
    {
        grid = newGrid;
        QueueRedraw();
    }

    public override void _Draw()
    {
    GD.Print($"grid null? {grid == null}, CellSize: {CellSize}");

    DrawRect(new Rect2(0,0,200,200), Colors.Red);
    if (grid == null) return;

    int h = grid.GetLength(0);
    int w = grid.GetLength(1);

    for (int y = 0; y < h; y++)
    for (int x = 0; x < w; x++)
    {
        Color color = grid[y, x] ? Colors.Green : Colors.Blue;
        DrawRect(new Rect2(x * CellSize, y * CellSize, CellSize, CellSize), color);
    }
    }

}
    
    



