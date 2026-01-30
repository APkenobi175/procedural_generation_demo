using Godot;
using System;

public partial class HomeScreen : CanvasLayer
{
    
    public Button CellularAutomataButton;
    public Button PerlinNoiseButton;
    public Button ExitGame;
    public override void _Ready()
    {
        CellularAutomataButton = GetNode<Button>("HomeContainer/ToCell");
        PerlinNoiseButton = GetNode<Button>("HomeContainer/ToPerlin");
        ExitGame = GetNode<Button>("HomeContainer/ExitGame");
        CellularAutomataButton.Pressed += OnCeullarButtonPressed;
        PerlinNoiseButton.Pressed += OnPerlinButtonPressed;
        ExitGame.Pressed += OnExitButtonPressed; 


    }

    public override void _Process(double delta)
    {
     
  
    }



    public void OnCeullarButtonPressed()
    {
        GameManager.Instance.GoTo("Cellular");
    }

    public void OnPerlinButtonPressed()
    {
        GameManager.Instance.GoTo("Perlin");
    }

    public void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
