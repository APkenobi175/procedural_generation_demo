// This class will be used to handle logic for all show/hide buttons in the project so I don't have to keep redoing it.

using Godot;
using System;
public static class ProceduralGenerationUtilities
{
    public static void OnHideButtonPressed(Button hideButton, VBoxContainer showHideContainer)
    {
        showHideContainer.Visible = !showHideContainer.Visible; // Toggle visibility
        hideButton.Text = showHideContainer.Visible ? "Hide Controls" : "Show Controls";
    }

    public static void OnRegenerateButtonPressed(CanvasLayer controlLayer)
    {
        // We need to check what type of control layer we are dealing with so we can emit the correct signal
        if (controlLayer is CellularControls cellularControls)
        {
            cellularControls.EmitSignal(CellularControls.SignalName.ParametersChanged);
            GD.Print("Cellular Controls Parameters Changed Signal Emitted");
        }
        else if (controlLayer is PerlinControls perlinControls)
        {
            perlinControls.EmitSignal(PerlinControls.SignalName.ParametersChanged);
            GD.Print("Perlin Controls Parameters Changed Signal Emitted");
        }
        else if (controlLayer is WaveControls waveControls)
        {
            waveControls.EmitSignal(WaveControls.SignalName.ParametersChanged);
            GD.Print("Wave Controls Parameters Changed Signal Emitted");
        }
        else if (controlLayer is BspControls bspControls)
        {
            bspControls.EmitSignal(BspControls.SignalName.ParametersChanged);
            GD.Print("BSP Controls Parameters Changed Signal Emitted");
        }
        else if (controlLayer is RandomWalkControls randomWalkControls)
        {
            randomWalkControls.EmitSignal(RandomWalkControls.SignalName.ParametersChanged);
            GD.Print("Random Walk Controls Parameters Changed Signal Emitted");
        }
        else
        {
            GD.PrintErr("Unknown control layer type: " + controlLayer.GetType());
        }
    }
}