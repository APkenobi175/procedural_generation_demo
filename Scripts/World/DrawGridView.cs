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
        // if grid is empy do nothing
    if (grid == null) return;

    // draw the grid h x w
    int h = grid.GetLength(0);
    int w = grid.GetLength(1);


    for (int y = 0; y < h; y++)
    for (int x = 0; x < w; x++)
    // blue for water, green for land
    {
        Color color = grid[y, x] ? Colors.Green : Colors.Blue;
        DrawRect(new Rect2(x * CellSize, y * CellSize, CellSize, CellSize), color);
    }
    }

}
    
    



