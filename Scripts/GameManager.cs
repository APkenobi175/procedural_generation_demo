using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
    
    public static GameManager Instance { get; private set; } // Instance of the GameManager
    public Dictionary<string, PackedScene> Levels = new(); // Dictionary for valid levels (one for perlin noise, one for automata) this is using godots system.collections.generic

    // Use these to determine camera startout position and zoom level for each demo
    public bool CellularActive = false;
    public bool PerlinActive = false;
    public bool WFCActive = false;
    public bool BSPActive = false;
    public bool RandomWalkActive = false;

    public override void _Ready()
    {
        // Set up all the scenes in a dictionary for easy access
        Instance = this; // Set the instance to this object

        Levels["MainMenu"] = GD.Load<PackedScene>("Scenes/Main.tscn");
        Levels["Cellular"] = GD.Load<PackedScene>("Scenes/Demos/CellularAutomataDemo.tscn");
        Levels["Perlin"] = GD.Load<PackedScene>("Scenes/Demos/PerlinNoiseDemo.tscn");
        Levels["WFC"] = GD.Load<PackedScene>("Scenes/Demos/WaveFunctionCollapse.tscn");
        // COMING SOON:
        Levels["BSP"] = GD.Load<PackedScene>("Scenes/Demos/BinarySpacePartitioning.tscn");
        Levels["RandomWalk"] = GD.Load<PackedScene>("Scenes/Demos/RandomWalkDemo.tscn");
    }

    public void GoTo (string key)
    {
        GetTree().ChangeSceneToPacked(Levels[key]); // Change the scene to the one associated with the key. We can call this function on the buttons in the home screen or the back button in the UI
        // used to determine starting position for camera
        if (key == "Cellular")
        {
            CellularActive = true;
            PerlinActive = false;
            WFCActive = true;
            BSPActive = false;
            RandomWalkActive = false;
        }
        else if (key == "Perlin")
        {
            CellularActive = false;
            PerlinActive = true;
            WFCActive = true;
            BSPActive = false;
            RandomWalkActive = false;

        } else if (key == "WFC")
        {
            CellularActive = false;
            PerlinActive = false;
            WFCActive = true;
            BSPActive = false;
            RandomWalkActive = false;
        }
        else if (key == "BSP")
        {
            CellularActive = false;
            PerlinActive = false;
            WFCActive = false;
            BSPActive = true;
            RandomWalkActive = false;
        }
         else if (key == "RandomWalk")
        {
            CellularActive = false;
            PerlinActive = false;
            WFCActive = false;
            BSPActive = false;
            RandomWalkActive = true;
        }
        else
        {
            CellularActive = false;
            PerlinActive = false;
            WFCActive = false;
            BSPActive = false;
            RandomWalkActive = false;
        }
    }
}
