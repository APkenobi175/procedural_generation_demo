using Godot;
using System;

public partial class CellularAutomataDemo : Node
{
    private CellularTiles cellularTiles;
    private DrawGridView drawGridView;
    private Node controls;

    public CheckBox enableTilingCheckbox;

    public override void _Ready()
    {
        drawGridView = GetNode<DrawGridView>("DrawGridView");
        controls = GetNode<Node>("CellularControls");
        cellularTiles = GetNode<CellularTiles>("CellularTiles");


        // If you get the parameters changed signals, rerender the demo

        if (controls is CellularControls cc)
        {
            cc.Connect("ParametersChanged", new Callable(this, nameof(SetupDemo)));
        }
        else
        {
            GD.PrintErr("Failed to connect ParametersChanged signal: controls node is not of type CellularControls");
        }


        SetupDemo(); 
    }

    private void SetupDemo()
    {
        Random rng = new Random();
        HSlider widthSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/WidthSlider");
        HSlider heightSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/HeightSlider");
        HSlider stepsSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/StepSlider"); 
        LineEdit seedBox     = controls.GetNode<LineEdit>("ControlContainer/ShowHideContainer/LineEdit");  
        enableTilingCheckbox = controls.GetNode<CheckBox>("ControlContainer/ShowHideContainer/enableTiling");

        int chunkWidth  = (int)widthSlider.Value;
        int chunkHeight = (int)heightSlider.Value;
        int numSteps    = (int)stepsSlider.Value;
        bool enableTiling = enableTilingCheckbox.ButtonPressed;

        int seed = int.TryParse(seedBox.Text, out int s) ? s : 0;
        // if the seed is 0, generate a random seed
        if (seed == 0)
        {
            seed = rng.Next(); // random seed
        }


        // generate the cellular automata grid we are defaulting to without tiling

        bool[,] grid = CellularAutomata.Generate(
            chunkWidth,
            chunkHeight,
            0.45f,
            numSteps,
            4,
            4,
            seed
        );
        
        // If enable tiling is checked, render with tiling, otherwise render without tiling
        if (enableTiling)
        {
            // make the cellular tiles layer visible and the draw grid view invisible
            cellularTiles.Layer.Visible = true;

            RenderWithTiling(grid);
        }
        else
        {
            cellularTiles.Layer.Visible = false;
            drawGridView.Visible = true;
            drawGridView.SetGrid(grid);
        }

    }

    private void RenderWithTiling(bool[,] grid)
    {
        drawGridView.Visible = false; 
        cellularTiles.RenderTerrainsFromGrid(grid);
    }


}
