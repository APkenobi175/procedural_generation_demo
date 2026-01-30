using Godot;
using System;

public partial class CellularAutomataDemo : Node
{
    private DrawGridView drawGridView;
    private Node controls;

    public override void _Ready()
    {
        drawGridView = GetNode<DrawGridView>("DrawGridView");
        controls = GetNode<Node>("CellularControls");

        // if the signal is emmited from the controls node, we call SetupDemo, and print that we recieved the signal


        if (controls is CellularControls cc)
        {
            cc.Connect("ParametersChanged", new Callable(this, nameof(SetupDemo)));
            GD.Print("Connected ParametersChanged signal from controls to SetupDemo");
        }
        else
        {
            GD.PrintErr("CellularControls node is NOT a CellularControls script instance.");
        }


        SetupDemo(); // after refs + connect
    }

    private void SetupDemo()
    {
        Random rng = new Random();
        HSlider widthSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/WidthSlider");
        HSlider heightSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/HeightSlider");
        HSlider stepsSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/StepSlider"); 
        LineEdit seedBox     = controls.GetNode<LineEdit>("ControlContainer/ShowHideContainer/LineEdit");  

        int chunkWidth  = (int)widthSlider.Value;
        int chunkHeight = (int)heightSlider.Value;
        int numSteps    = (int)stepsSlider.Value;



        int seed = int.TryParse(seedBox.Text, out int s) ? s : 0;

        if (seed == 0)
        {
            seed = rng.Next(); // random seed
        }

        bool[,] grid = CellularAutomata.Generate(
            chunkWidth,
            chunkHeight,
            0.45f,
            numSteps,
            4,
            4,
            seed
        );
        GD.Print($"$ generated w={grid.GetLength(1)} h = {grid.GetLength(0)}");

        drawGridView.SetGrid(grid);
    }
}
