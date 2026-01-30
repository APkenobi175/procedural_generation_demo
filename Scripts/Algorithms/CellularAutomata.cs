using System;

public static class CellularAutomata
{
    public static bool[,] Generate(
        int width, int height,
        float initialDensity,
        int steps,
        int birthLimit,
        int surviveLimit,
        int seed)
    {
        var rng = new Random(seed);

        bool[,] grid = new bool[height, width];

        // init
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
            grid[y, x] = rng.NextDouble() < initialDensity;

        // iterate
        for (int i = 0; i < steps; i++)
            grid = Step(grid, birthLimit, surviveLimit);

        return grid;
    }

    private static bool[,] Step(bool[,] oldGrid, int birthLimit, int surviveLimit)
    {
        int h = oldGrid.GetLength(0);
        int w = oldGrid.GetLength(1);
        bool[,] next = new bool[h, w];

        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            int n = CountAliveNeighbors(oldGrid, x, y);
            bool alive = oldGrid[y, x];

            if (alive)
                next[y, x] = n >= surviveLimit;      // survive if clumped enough
            else
                next[y, x] = n > birthLimit;         // born if surrounded enough
        }

        return next;
    }

    private static int CountAliveNeighbors(bool[,] grid, int x, int y)
    {
        int h = grid.GetLength(0);
        int w = grid.GetLength(1);
        int count = 0;

        for (int dy = -1; dy <= 1; dy++)
        for (int dx = -1; dx <= 1; dx++)
        {
            if (dx == 0 && dy == 0) continue;
            int nx = x + dx;
            int ny = y + dy;

            // Treat out-of-bounds as alive (makes caves closed-in, common technique)
            if (nx < 0 || nx >= w || ny < 0 || ny >= h)
                count++;
            else if (grid[ny, nx])
                count++;
        }

        return count;
    }
}
