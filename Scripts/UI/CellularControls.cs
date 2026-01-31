using Godot;
using System;

public partial class CellularControls : CanvasLayer
{
    private Button HideButton;
    private VBoxContainer ShowHideContainer;

    [Signal] // Lets send a signal when parameters change
    public delegate void ParametersChangedEventHandler();

    public int chunkWidth;
    public int chunkHeight;
    public int numSteps;
    public int seed;
    public string chunkWidthText;
    public string chunkHeightText;
    public string numStepsText;
    public Label chunkWidthLabel;
    public Label chunkHeightLabel;
    public Label numStepsLabel;
    public Button regenerateButton;


    public override void _Ready()
    {
        HideButton = GetNode<Button>("ControlContainer/Hide");
        ShowHideContainer = GetNode<VBoxContainer>("ControlContainer/ShowHideContainer");
        ShowHideContainer.Visible = true; // Start with controls visible
        HideButton.Pressed += OnHidePressed;
        chunkWidthLabel = GetNode<Label>("ControlContainer/ShowHideContainer/ChunkWidth");
        chunkHeightLabel = GetNode<Label>("ControlContainer/ShowHideContainer/ChunkHeight");
        numStepsLabel = GetNode<Label>("ControlContainer/ShowHideContainer/NumSteps");
        regenerateButton = GetNode<Button>("ControlContainer/ShowHideContainer/Generate");
        regenerateButton.Pressed += onRegeneratePressed;
    }

    public override void _Process(double delta)
    {

        // Make the labels update in real time with the values of their sliders
        
        chunkWidth = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/WidthSlider").Value;
        chunkHeight = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/HeightSlider").Value;
        numSteps = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/StepSlider").Value;
        chunkWidthText = "Chunk Width: " + chunkWidth.ToString();
        chunkHeightText = "Chunk Height: " + chunkHeight.ToString();
        numStepsText = "Num Steps: " + numSteps.ToString();
        chunkWidthLabel.Text = chunkWidthText;
        chunkHeightLabel.Text = chunkHeightText;
        numStepsLabel.Text = numStepsText;
    }

    public void OnHidePressed()
    {
        ShowHideContainer.Visible = !ShowHideContainer.Visible; // Toggle visibility
        HideButton.Text = ShowHideContainer.Visible ? "Hide Controls" : "Show Controls";
    }

    public void onRegeneratePressed()
    {
        EmitSignal(SignalName.ParametersChanged); // when we press the button we emit the signal
        GD.Print("Parameters Changed Signal Emitted");
    }

}


