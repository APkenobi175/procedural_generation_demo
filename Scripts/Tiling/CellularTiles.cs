using Godot;
using System;

public partial class CellularTiles : Node2D
{
    [Export] public TileMapLayer Layer; 

    // Terrain IDs (adjust to match your TileSet)
    private const int TERRAIN_SET = 0;
    private const int TERRAIN_GRASS = 0;
    private const int TERRAIN_WATER = 1;


    public void RenderTerrainsFromGrid(bool[,] grid)
    {
        Layer.Clear();

        var grassCells = new Godot.Collections.Array<Vector2I>();
        var waterCells = new Godot.Collections.Array<Vector2I>();

        int h = grid.GetLength(0);
        int w = grid.GetLength(1);

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                // Convention: true = grass, false = water
                if (grid[y, x])
                    grassCells.Add(new Vector2I(x, y));
                else
                    waterCells.Add(new Vector2I(x, y));
            }
        }

        Layer.SetCellsTerrainConnect(grassCells, TERRAIN_SET, TERRAIN_GRASS, true);
        Layer.SetCellsTerrainConnect(waterCells, TERRAIN_SET, TERRAIN_WATER, true);

        Layer.UpdateInternals();
    }
}