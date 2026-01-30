using Godot;
using System.Collections.Generic;

public partial class GameManager : Node
{
    
    public static GameManager Instance { get; private set; } // Instance of the GameManager
    public Dictionary<string, PackedScene> Levels = new(); // Dictionary for valid levels (one for perlin noise, one for automata) this is using godots system.collections.generic

    public override void _Ready()
    {
        // Set up all the scenes in a dictionary for easy access
        Instance = this; // Set the instance to this object

        Levels["MainMenu"] = GD.Load<PackedScene>("Scenes/Main.tscn");
        Levels["Cellular"] = GD.Load<PackedScene>("Scenes/Demos/CellularAutomataDemo.tscn");
        Levels["Perlin"] = GD.Load<PackedScene>("Scenes/Demos/PerlinNoiseDemo.tscn");
    }

    public void GoTo (string key)
    {
        GetTree().ChangeSceneToPacked(Levels[key]); // Change the scene to the one associated with the key. We can call this function on the buttons in the home screen or the back button in the UI
    }
}
