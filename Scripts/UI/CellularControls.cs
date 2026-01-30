using Godot;
using System;

public partial class CellularControls : Node2D
{
    private Button HideButton;
    private VBoxContainer ShowHideContainer;


    public override void _Ready()
    {
        HideButton = GetNode<Button>("ControlContainer/Hide");
        ShowHideContainer = GetNode<VBoxContainer>("ControlContainer/ShowHideContainer");
        ShowHideContainer.Visible = true; // Start with controls visible
    }

    public override void _Process(double delta)
    {
        HideButton.Pressed += OnHidePressed;
    }

    public void OnHidePressed()
    {
        ShowHideContainer.Visible = !ShowHideContainer.Visible; // Toggle visibility
        HideButton.Text = ShowHideContainer.Visible ? "Hide Controls" : "Show Controls";
    }
}


