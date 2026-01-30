using Godot;

public partial class CellularAutomataDemo : Node
{


    public void Regenerate()
    {
        // Replace these with values read from your UI controls
        int width = 120;
        int height = 80;
        float density = 0.45f;
        int steps = 4;
        int birthLimit = 4;
        int surviveLimit = 4;
        int seed = (int)Time.GetUnixTimeFromSystem();

        var grid = CellularAutomata.Generate(width, height, density, steps, birthLimit, surviveLimit, seed);

    }
}