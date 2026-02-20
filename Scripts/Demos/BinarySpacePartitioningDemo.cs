using Godot;
using System;

public partial class BinarySpacePartitioningDemo : Node
{
    private DrawBspView drawBspView;
    private Node controls;

    public override void _Ready()
    {
        drawBspView = GetNode<DrawBspView>("DrawBspView");
        controls = GetNode<Node>("BspControls");

        if (controls is BspControls bc)
        {
            bc.Connect("ParametersChanged", new Callable(this, nameof(SetupDemo)));
        }
        else
        {
            GD.PrintErr("Failed to connect ParametersChanged signal: controls node is not of type BspControls");
        }

        SetupDemo();
    }

    private void SetupDemo()
    {
        if (controls is not BspControls bc) return;

        // Get our values from the UI

        HSlider widthSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/WidthSlider");
        HSlider heightSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/HeightSlider");
        HSlider minDepthSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/MinDepthSlider");
        HSlider maxDepthSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/MaxDepthSlider");
        HSlider splitChanceSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/SplitChanceSlider");
        HSlider roomMarginMinSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/MinMarginSlider");
        HSlider roomMarginMaxSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/MaxMarginSlider");
        HSlider minLeafSizeSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/MinLeafSizeSlider");

        LineEdit seedBox = controls.GetNode<LineEdit>("ControlContainer/ShowHideContainer/SeedLineEdit");

        int chunkWidth = (int)widthSlider.Value;
        int chunkHeight = (int)heightSlider.Value;
        int minDepth = (int)minDepthSlider.Value;
        int maxDepth = (int)maxDepthSlider.Value;
        float splitChance = (float)splitChanceSlider.Value;
        int seed = int.TryParse(seedBox.Text, out int parsedSeed) ? parsedSeed : 0;
        int roomMarginMin = (int)roomMarginMinSlider.Value;
        int roomMarginMax = (int)roomMarginMaxSlider.Value;
        int minLeafSize = (int)minLeafSizeSlider.Value;


        // generate random seed if user enters 0
        if (seed == 0)
        {
            seed = new Random().Next();
        }

        // generate the BSP data

        var result = BSP.Generate(
            chunkWidth,
            chunkHeight,
            minDepth,
            maxDepth,
            minLeafSize,
            splitChance,
            seed,
            roomMarginMin,
            roomMarginMax
        );

        // Draw the data

        drawBspView.SetData(result.Root, result.Rooms, result.Corridors);
    }
}