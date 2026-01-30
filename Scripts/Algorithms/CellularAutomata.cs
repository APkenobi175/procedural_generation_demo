using Godot;
using System;

public static class CellularAutomata
{
    
    public static bool[,] Generate(int width, int height, float density, int steps, int birthLimit, int surviveLimit, int seed)
    {

        bool[,] grid = new bool[height, width]; // create a grid object of the given width and height
        Random rng = new Random(seed);

        // For all the cells in the grid, initialize them to alive
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[y,x] = true; // initialize all cells to alive
            }
        }

        // for all cells in the grid, intalialize them to alive
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
            grid[y,x] = true; // initialize all cells to alive
        // C# has some very interesting syntax rules I didn't know about, but if you run a for loop without braces, it only applies to the next line.

        // Now that all the cells are alive, we can randomly kill some cells ased on the density

        for(int y = 0; y<height; y++)
        {
            for(int x = 0; x<width; x++)
            {
                if (rng.NextDouble() < density)
                {
                    grid[y,x] = false; // kill the random cell, will be 50% chance
                }
            }
        }

        // Now that we have 50% alive and 50% dead cells, we can run the algorithm to make sure they are grouped nicely and it looks good
        

        //1. Count the number of neighbors that are alive for each cell
        //2. If a cell has more than 4 alive neighbors, it becomes alive
        //3. If a cell has less than 4 alive neighbors, it becomes dead
        //4. Repeat for the number of steps specified
        // We will change the hardcoded 4 into a variable birthLimit and surviveLimit, that way if we want to, in the future that could be a changeable parameter to see
        // What interesting patterns emerge
        // We will also do the same thing with the density. For now though these variables will be hardcoded into the function call

        for (int i = 0; i < steps; i++)
        {
            grid = Step(grid, birthLimit, surviveLimit);
        }

        return grid;
  
    }

    private static bool[,] Step(bool[,] oldGrid, int birthLimit, int surviveLimit)
    {
    int h = oldGrid.GetLength(0); // height
    int w = oldGrid.GetLength(1); // width
    bool[,] next = new bool[h, w]; // new grid for next state

    for (int y = 0; y < h; y++) // for each row and each column
    {
        for (int x = 0; x < w; x++)
        {
            int n = CountAliveNeighbors(oldGrid, x, y); // count the alive neighbors

            if (oldGrid[y, x]) // alive
                next[y, x] = (n >= surviveLimit); // stays alive if enough neighbors (4)
            else               // dead
                next[y, x] = (n > birthLimit); // becomes alive if enough neighbors (4)
        }
    }

    return next;
    }

    private static int CountAliveNeighbors(bool[,] grid, int x, int y)
    {
    // This method counts the number of alive neighbors around the cell at (x, y)
    int h = grid.GetLength(0); 
    int w = grid.GetLength(1);
    int count = 0;

    for (int dy = -1; dy <= 1; dy++) // for each neioghboring cell, including diagonals
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            if (dx == 0 && dy == 0) continue; // if both cells are 0, skip (the cell itself)

            int nx = x + dx; // neighbor x
            int ny = y + dy; // neighbor y

            if (nx < 0 || nx >= w || ny < 0 || ny >= h) // if the neighbor is out of bounds
                count++;  // increment count for out of bounds (treat as alive)
            else if (grid[ny, nx]) // if the neighbor is alive
                count++; // increment count for alive neighbor
        }
    }

    return count; // return the count of alive neighbors
    }









}