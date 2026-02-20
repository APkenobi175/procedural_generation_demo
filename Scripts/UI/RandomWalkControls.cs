using Godot;
using System;

public partial class RandomWalkControls : CanvasLayer
{
    [Signal] // Lets send a signal when parameters change
    public delegate void ParametersChangedEventHandler();
    public override void _Ready()
    {
    GD.Print("Random Walk Controls Ready");
    }
}
