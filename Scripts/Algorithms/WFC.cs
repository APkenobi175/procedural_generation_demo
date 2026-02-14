// WFC.cs
// Minimal, pseudocode-style WFC (bitmask version, 16 tiles, NO backtracking)

using System;
using System.Collections.Generic;

public sealed class WFC
{

    // TILE DEFINITIONS!!!!!!!!!!!!! //

    public const ushort T0  = 0b0000000000000001;
    public const ushort T1  = 0b0000000000000010;
    public const ushort T2  = 0b0000000000000100;
    public const ushort T3  = 0b0000000000001000;
    public const ushort T4  = 0b0000000000010000;
    public const ushort T5  = 0b0000000000100000;
    public const ushort T6  = 0b0000000001000000;
    public const ushort T7  = 0b0000000010000000;
    public const ushort T8  = 0b0000000100000000;
    public const ushort T9  = 0b0000001000000000;
    public const ushort T10 = 0b0000010000000000;
    public const ushort T11 = 0b0000100000000000;
    public const ushort T12 = 0b0001000000000000;
    public const ushort T13 = 0b0010000000000000;
    public const ushort T14 = 0b0100000000000000;
    public const ushort T15 = 0b1000000000000000;

    public const int TILE_COUNT = 16;

    public enum Dir { N = 0, E = 1, S = 2, W = 3 }

    private ushort[] grid;
    private int width;
    private int height;

    private ushort[,] constraintRules; // bitmask of allowed tiles
    private int[] weights; // weight for each tile, used in random selection

    private Random rng; // initialized with seed in Initialize()

    // ----------------------------
    // Initialize (matches pseudocode)
    // ----------------------------
    public void Initialize(int w, int h, ushort[,] rules, int[] tileWeights, int? seed = null)
    {
        width = w;
        height = h;

        rng = seed.HasValue ? new Random(seed.Value) : new Random();

        grid = new ushort[width * height];

        constraintRules = rules;      // [16,4]
        weights = tileWeights;        // [16]

        // Create a grid where each cell contains all possible tiles (superposition)
        // OH YEAH THIS IS WHERE WE USE OUR BIT MASKS BABY
        ushort initialMask = 0;

        if (weights[0] > 0) initialMask |= T0;
        if (weights[1] > 0) initialMask |= T1;
        if (weights[2] > 0) initialMask |= T2;
        if (weights[3] > 0) initialMask |= T3;
        if (weights[4] > 0) initialMask |= T4;
        if (weights[5] > 0) initialMask |= T5;
        if (weights[6] > 0) initialMask |= T6;
        if (weights[7] > 0) initialMask |= T7;
        if (weights[8] > 0) initialMask |= T8;
        if (weights[9] > 0) initialMask |= T9;
        if (weights[10] > 0) initialMask |= T10;
        if (weights[11] > 0) initialMask |= T11;
        if (weights[12] > 0) initialMask |= T12;
        if (weights[13] > 0) initialMask |= T13;
        if (weights[14] > 0) initialMask |= T14;
        if (weights[15] > 0) initialMask |= T15;

        if (initialMask == 0)
            throw new Exception("All weights are 0. No tiles available.");

        // Fill grid
        for (int i = 0; i < grid.Length; i++)
            grid[i] = initialMask;

    }

    public bool WaveFunctionCollapse()
    {
        // While there are uncollapsed cells:
        while (HasUncollapsedCells())
        {
            // 1. Find the minimum entropy cells (fewest possibilities)
            int cellIndex = FindMinimumEntropyCell();

            // 2. Collapse the cell to a single tile
            ushort possibilities = grid[cellIndex];
            // 3. Choose randomly from remaining possibilities, with weights
            int chosenTile = RandomlySelectFrom(possibilities);

            if (chosenTile < 0)
                return false; // contradiction (no valid tile to choose) we aint gonna do nothing about this
            // Collapse
            grid[cellIndex] = TileBit(chosenTile);

            // 4. Propagate constraints to neighbors
            Queue<int> propagationQueue = new Queue<int>();
            propagationQueue.Enqueue(cellIndex);
            while (propagationQueue.Count > 0)
            {
                int currentCell = propagationQueue.Dequeue();

                int cx = currentCell % width;
                int cy = currentCell / width;

                // For each neighbor of currentCell:
                for (int d = 0; d < 4; d++)
                {
                    int nx, ny;
                    if (!TryGetNeighbor(cx, cy, (Dir)d, out nx, out ny))
                        continue;

                    int neighborIndex = Index(nx, ny);

                    ushort oldPossibilities = grid[neighborIndex];
                    ushort newPossibilities = oldPossibilities;

                    // 5. Remove Incompatible Tiles based on constraint rules
                    for (int neighborTile = 0; neighborTile < TILE_COUNT; neighborTile++)
                    {
                        // if neighborTile isn't currently possible, skip it
                        if ((newPossibilities & TileBit(neighborTile)) == 0)
                            continue;
                        if (!IsCompatibleWithCurrentSuperposition(currentCell, neighborTile, (Dir)d))
                        {
                            newPossibilities = (ushort)(newPossibilities & (ushort)~TileBit(neighborTile));
                        }
                    }

                    // contradiction check
                    if (newPossibilities == 0)
                        return false;

                    // 6. If possibilities changed, add neighbor to propagation queue
                    if (newPossibilities != oldPossibilities)
                    {
                        grid[neighborIndex] = newPossibilities;
                        propagationQueue.Enqueue(neighborIndex);
                    }
                }
            }
        }

        // 7. Return SUCCESS!!!
        return true;
    }

    private int FindMinimumEntropyCell()
    {
        int minEntropy = int.MaxValue;
        List<int> candidates = new List<int>();

        // For each uncollapsed cell in grid:
        for (int i = 0; i < grid.Length; i++)
        {
            int entropy = CountPossibilities(grid[i]);
            if (entropy <= 1) continue;

            if (entropy < minEntropy)
            {
                minEntropy = entropy;
                candidates.Clear();
                candidates.Add(i);
            }
            else if (entropy == minEntropy)
            {
                candidates.Add(i);
            }
        }

        // Return randomChoice(candidates)
        int pick = rng.Next(candidates.Count);
        return candidates[pick];
    }

    private bool IsCompatible(int tile1, int tile2, Dir direction)
    {
        // 1. Check if tile2 can be placed in the given direction relative to tile1
        ushort allowedMask = constraintRules[tile1, (int)direction];
        return (allowedMask & TileBit(tile2)) != 0;
    }

    // Helper: in propagation, current cell might still be superposition.
    // neighborTile is allowed if it matches at least ONE possible tile in current cell.
    private bool IsCompatibleWithCurrentSuperposition(int currentIndex, int neighborTile, Dir direction)
    {
        ushort currentMask = grid[currentIndex];

        for (int possibleTile = 0; possibleTile < TILE_COUNT; possibleTile++)
        {
            if ((currentMask & TileBit(possibleTile)) == 0) continue;

            if (IsCompatible(possibleTile, neighborTile, direction))
                return true;
        }

        return false;
    }

    private int RandomlySelectFrom(ushort possibilities)
    // Helper function to randomly select a tile from the possibilities bitmask, using weights
    {
        int total = 0;
        for (int t = 0; t < TILE_COUNT; t++)
        {
            if ((possibilities & TileBit(t)) != 0)
                total += weights[t];
        }

        if (total <= 0)
            return -1;

        int roll = rng.Next(1, total + 1);
        int running = 0;

        for (int t = 0; t < TILE_COUNT; t++)
        {
            if ((possibilities & TileBit(t)) == 0) continue;

            running += weights[t];
            if (roll <= running)
                return t;
        }

        return -1;
    }

    private bool HasUncollapsedCells()
    // Helper function to check if there are any uncollapsed cells left (more than 1 possibility)
    {
        for (int i = 0; i < grid.Length; i++)
        {
            if (CountPossibilities(grid[i]) > 1)
                return true;
        }
        return false;
    }

    private int CountPossibilities(ushort mask)
    {
        // Helper function to count the number of possible tiles in a bitmask (number of bits set to 1)
        // popcount
        int c = 0;
        while (mask != 0)
        {
            mask = (ushort)(mask & (ushort)(mask - 1));
            c++;
        }
        return c;
    }

        private static ushort TileBit(int tile)
        {
            // Helper function to get the bitmask for a given tile index
            if (tile == 0) return T0;
            if (tile == 1) return T1;
            if (tile == 2) return T2;
            if (tile == 3) return T3;
            if (tile == 4) return T4;
            if (tile == 5) return T5;
            if (tile == 6) return T6;
            if (tile == 7) return T7;
            if (tile == 8) return T8;
            if (tile == 9) return T9;
            if (tile == 10) return T10;
            if (tile == 11) return T11;
            if (tile == 12) return T12;
            if (tile == 13) return T13;
            if (tile == 14) return T14;
            if (tile == 15) return T15;

            return 0;
        }


    private int Index(int x, int y)
    {
        // Helper function to convert (x, y) to grid index
        return y * width + x;
    }

    private bool TryGetNeighbor(int x, int y, Dir d, out int nx, out int ny)
    {
        // Helper function to get neighbor coordinates based on direction, returns false if out of bounds
        nx = x;
        ny = y;

        if (d == Dir.N) ny = y - 1;
        else if (d == Dir.S) ny = y + 1;
        else if (d == Dir.E) nx = x + 1;
        else if (d == Dir.W) nx = x - 1;

        if (nx < 0 || nx >= width || ny < 0 || ny >= height)
            return false;

        return true;
    }

    // Getters for visulation that I might use in DrawWaveView.
    public ushort GetCellMask(int x, int y)
    {
        return grid[Index(x, y)];
    }

    public int GetCollapsedTile(int x, int y)
    {
        ushort m = GetCellMask(x, y);
        if (CountPossibilities(m) != 1) return -1;

        for (int t = 0; t < TILE_COUNT; t++)
            if ((m & TileBit(t)) != 0) return t;

        return -1;
    }
}
