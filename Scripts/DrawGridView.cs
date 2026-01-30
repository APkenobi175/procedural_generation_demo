using Godot;

public partial class DrawGridView : Node2D
{
    [Export] public int CellSize = 8;

    private bool[,] _cells;
    private float[,] _heights;
    private bool _useHeights;

    public void SetCells(bool[,] cells)
    {
        _cells = cells;
        _useHeights = false;
        QueueRedraw();
    }

    public void SetHeights(float[,] heights)
    {
        _heights = heights;
        _useHeights = true;
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (!_useHeights && _cells == null) return;
        if (_useHeights && _heights == null) return;

        if (!_useHeights)
            DrawBoolGrid();
        else
            DrawHeightGrid();
    }

    private void DrawBoolGrid()
    {
        int h = _cells.GetLength(0);
        int w = _cells.GetLength(1);

        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            // alive = wall/land, dead = empty/water (your choice)
            var c = _cells[y, x] ? Colors.White : Colors.Black;
            DrawRect(new Rect2(x * CellSize, y * CellSize, CellSize, CellSize), c);
        }
    }

    private void DrawHeightGrid()
    {
        int h = _heights.GetLength(0);
        int w = _heights.GetLength(1);

        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            float v = _heights[y, x]; // assume 0..1
            // Quick grayscale visualization (we’ll do biomes in the Perlin demo controller)
            var c = new Color(v, v, v);
            DrawRect(new Rect2(x * CellSize, y * CellSize, CellSize, CellSize), c);
        }
    }
}
