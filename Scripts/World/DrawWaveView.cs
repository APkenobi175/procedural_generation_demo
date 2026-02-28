using Godot;

public partial class DrawWaveView : Node
{
    [Export] public TileMapLayer Layer;
    [Export] public int SourceId = 0; // atlas source id (usually 0)

    private int[,] tiles;

    // WFC ATLAS COORDINATES FOR EACH TILE
    private static readonly Vector2I[] AtlasCoordsForTile = new Vector2I[16]
    {
        new Vector2I(3, 1),  // 0  Empty
        new Vector2I(4, 1),  // 1  N
        new Vector2I(1, 4),  // 2  E
        new Vector2I(6, 2),  // 3  N+E
        new Vector2I(10, 1),  // 4  S
        new Vector2I(6, 0),  // 5  N+S
        new Vector2I(0, 2),  // 6  E+S
        new Vector2I(14, 2), // 7  N+E+S

        new Vector2I(3, 2),  // 8  W
        new Vector2I(4, 2),  // 9  W+N
        new Vector2I(4, 0),  // 10 E+W
        new Vector2I(8, 2),  // 11 W+N+E

        new Vector2I(2, 2),  // 12 S+W
        new Vector2I(12, 2), // 13 S+W+N
        new Vector2I(10, 2), // 14 E+S+W
        new Vector2I(0, 4),  // 15 4-way
    };

    public void SetTiles(int[,] newTiles)
    {
        tiles = newTiles;
        Render();
        centerMap();
    }

    private void Render()
    {
        if (Layer == null) return;
        if (tiles == null) return;

        int h = tiles.GetLength(0);
        int w = tiles.GetLength(1);

        Layer.Clear();


        var ts = Layer.TileSet;
        for (int i = 0; i < ts.GetSourceCount(); i++)
        {
            int id = ts.GetSourceId(i);
        }


        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            int t = tiles[y, x];
            if (t < 0 || t > 15) continue;

            Layer.SetCell(new Vector2I(x, y), SourceId, AtlasCoordsForTile[t]);
        }

    }

    public void centerMap()
    {
        if (Layer == null || tiles == null) return;

        int h = tiles.GetLength(0);
        int w = tiles.GetLength(1);

        float tileSize = 16f;

        float mapW = w * tileSize;
        float mapH = h * tileSize;

        Layer.Position = new Vector2(-mapW / 2f, -mapH / 2f);
    }

}

