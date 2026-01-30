using Godot;
using System;

public partial class CommonDemoHud : Node2D
{
    
    private Button menuButton;
    private Button homeButton;
    private Button exitButton;
    private VBoxContainer menuContainer;
    public override void _Ready()
    {
        menuButton = GetNode<Button>("MenuButton");
        homeButton = GetNode<Button>("MenuContainer/Home");
        exitButton = GetNode<Button>("MenuContainer/Exit");
        menuContainer = GetNode<VBoxContainer>("MenuContainer");
        menuContainer.Visible = false; // Hide the menu at start
        menuButton.Pressed += OnMenuPressed;
        homeButton.Pressed += OnHomePressed;
        exitButton.Pressed += OnExitPressed;
    }

    public override void _Process(double delta)
    {


    }

    public void OnMenuPressed()
    {
        menuContainer.Visible = !menuContainer.Visible; // Toggle menu visibility (this way we can open and close it)
    }

    public void OnHomePressed()
    {
        GameManager.Instance.GoTo("MainMenu");
    }

    public void OnExitPressed()
    {
        GetTree().Quit();
    }
}
