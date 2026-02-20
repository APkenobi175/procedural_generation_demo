using Godot;
using System;

public partial class WaveControls : CanvasLayer
{
    
    private Button HideButton;
    private VBoxContainer ShowHideContainer;
    [Signal] // Lets send a signal when parameters change
    public delegate void ParametersChangedEventHandler();

    // Values from UI
    public int chunkWidth;
    public int chunkHeight;

    public int seed;

    public OptionButton weightSet;

    
    // Labels
    public Label chunkWidthLabel;
    public Label chunkHeightLabel;
    public Label WeightSetLabel;

    // Text for Labels
    public string chunkWidthText;
    public string chunkHeightText;
    public string weightSetText;


    public Button regenerateButton;

    public override void _Ready()
    {
        HideButton = GetNode<Button>("ControlContainer/Hide");
        ShowHideContainer = GetNode<VBoxContainer>("ControlContainer/ShowHideContainer");
        ShowHideContainer.Visible = true; // Start with controls visible
        HideButton.Pressed += () =>
        ProceduralGenerationUtilities.OnHideButtonPressed(HideButton, ShowHideContainer);

        // Get Labels
        chunkWidthLabel = GetNode<Label>("ControlContainer/ShowHideContainer/ChunkWidth");
        chunkHeightLabel = GetNode<Label>("ControlContainer/ShowHideContainer/ChunkHeight");

        // get regenerate button
        regenerateButton = GetNode<Button>("ControlContainer/ShowHideContainer/Generate");
        regenerateButton.Pressed += () => 
        ProceduralGenerationUtilities.OnRegenerateButtonPressed(this);

    }

    public override void _Process(double delta)
    {
        // Update values from UI
        HSlider widthSlider  = GetNode<HSlider>("ControlContainer/ShowHideContainer/WidthSlider");
        HSlider heightSlider = GetNode<HSlider>("ControlContainer/ShowHideContainer/HeightSlider");

        chunkWidth = (int)widthSlider.Value;
        chunkHeight = (int)heightSlider.Value;

        // Update label text
        chunkWidthLabel.Text = $"Chunk Width: {chunkWidth}";
        chunkHeightLabel.Text = $"Chunk Height: {chunkHeight}";

    }

}
