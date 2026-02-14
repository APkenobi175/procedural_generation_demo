using Godot;
using System;

public partial class HomeScreen : CanvasLayer
{
    
    public Button CellularAutomataButton;
    public Button PerlinNoiseButton;
    public Button ExitGame;
    public Button WaveFunctionButton;
    public Button BSPButton;
    public Button RandomWalkButton;
    public override void _Ready()
    {
        CellularAutomataButton = GetNode<Button>("HomeContainer/ToCell");
        PerlinNoiseButton = GetNode<Button>("HomeContainer/ToPerlin");
        ExitGame = GetNode<Button>("HomeContainer/ExitGame");
        WaveFunctionButton = GetNode<Button>("HomeContainer/ToWFC");
        BSPButton = GetNode<Button>("HomeContainer/ToBSP");
        RandomWalkButton = GetNode<Button>("HomeContainer/ToRandomWalk");

        CellularAutomataButton.Pressed += OnCeullarButtonPressed;
        PerlinNoiseButton.Pressed += OnPerlinButtonPressed;
        ExitGame.Pressed += OnExitButtonPressed; 
        WaveFunctionButton.Pressed += OnWaveFunctionButtonPressed;
        BSPButton.Pressed += OnBSPButtonPressed;
        RandomWalkButton.Pressed += OnRandomWalkButtonPressed;



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

    public void OnWaveFunctionButtonPressed()
    {
        GameManager.Instance.GoTo("WFC");
    }

    public void OnBSPButtonPressed()
    {
        GameManager.Instance.GoTo("BSP");
    }

    public void OnRandomWalkButtonPressed()
    {
        GameManager.Instance.GoTo("RandomWalk");
    }



    public void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
