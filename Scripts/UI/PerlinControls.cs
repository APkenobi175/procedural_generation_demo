using Godot;
using System;

public partial class PerlinControls : CanvasLayer
{
    private Button HideButton;
    private VBoxContainer ShowHideContainer;
    [Signal] // Lets send a signal when parameters change
    public delegate void ParametersChangedEventHandler();

    // Slider Values
    public int chunkWidth;
    public int chunkHeight;
    public int noiseOctaves;
    public int seed;
    public int deepWaterThreshold;
    public int shallowWaterThreshold;
    public int beachThreshold;
    public int grassThreshold;
    public int mountainThreshold;

    // Labels for Sliders
    public Label chunkWidthLabel;
    public Label chunkHeightLabel;
    public Label noiseOctavesLabel;
    public Label deepWaterThresholdLabel;
    public Label shallowWaterThresholdLabel;
    public Label beachThresholdLabel;
    public Label grassThresholdLabel;
    public Label mountainThresholdLabel;

    // Text for Labels
    public string chunkWidthText;
    public string chunkHeightText;
    public string noiseOctavesText;
    public string deepWaterThresholdText;
    public string shallowWaterThresholdText;
    public string beachThresholdText;
    public string grassThresholdText;
    public string mountainThresholdText;

    // CheckBox for FBM
    public CheckBox useFbmBox;

    public Button regenerateButton;

    public override void _Ready()
    {
        HideButton = GetNode<Button>("ControlContainer/Hide");
        ShowHideContainer = GetNode<VBoxContainer>("ControlContainer/ShowHideContainer");
        ShowHideContainer.Visible = true; // Start with controls visible
        HideButton.Pressed += OnHidePressed;

        // Get Labels
        chunkWidthLabel = GetNode<Label>("ControlContainer/ShowHideContainer/ChunkWidth");
        chunkHeightLabel = GetNode<Label>("ControlContainer/ShowHideContainer/ChunkHeight");
        noiseOctavesLabel = GetNode<Label>("ControlContainer/ShowHideContainer/NosieOctaves"); // I spelled it wrong.....
        deepWaterThresholdLabel = GetNode<Label>("ControlContainer/ShowHideContainer/DeepWater");
        shallowWaterThresholdLabel = GetNode<Label>("ControlContainer/ShowHideContainer/ShallowWater");
        beachThresholdLabel = GetNode<Label>("ControlContainer/ShowHideContainer/Beach");
        grassThresholdLabel = GetNode<Label>("ControlContainer/ShowHideContainer/Grass");
        mountainThresholdLabel = GetNode<Label>("ControlContainer/ShowHideContainer/Mountain");

        // Get CheckBox
        useFbmBox = GetNode<CheckBox>("ControlContainer/ShowHideContainer/CheckBox");

        // Regenerate Button
        regenerateButton = GetNode<Button>("ControlContainer/ShowHideContainer/Generate");
        regenerateButton.Pressed += onRegeneratePressed;

    }

    public override void _Process(double delta)
    {
        // Make the labels update in real time with the values of their sliders
        chunkWidth = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/WidthSlider").Value;
        chunkHeight = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/HeightSlider").Value;
        noiseOctaves = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/NoiseSlider").Value;
        deepWaterThreshold = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/DeepWaterSlider").Value;
        shallowWaterThreshold = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/ShallowWaterSlider").Value;
        beachThreshold = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/BeachSlider").Value;
        grassThreshold = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/GrassSlider").Value;
        mountainThreshold = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/MountainSlider").Value;
        // update the labels with the new values
        chunkWidthText = "Chunk Width: " + chunkWidth.ToString();
        chunkHeightText = "Chunk Height: " + chunkHeight.ToString();
        noiseOctavesText = "Noise Octaves: " + noiseOctaves.ToString();
        deepWaterThresholdText = "Deep Water Threshold: " + deepWaterThreshold.ToString();
        shallowWaterThresholdText = "Shallow Water Threshold: " + shallowWaterThreshold.ToString();
        beachThresholdText = "Beach Threshold: " + beachThreshold.ToString();
        grassThresholdText = "Grass Threshold: " + grassThreshold.ToString();
        mountainThresholdText = "Mountain Threshold: " + mountainThreshold.ToString();

        chunkWidthLabel.Text = chunkWidthText;
        chunkHeightLabel.Text = chunkHeightText;
        noiseOctavesLabel.Text = noiseOctavesText;
        deepWaterThresholdLabel.Text = deepWaterThresholdText;
        shallowWaterThresholdLabel.Text = shallowWaterThresholdText;
        beachThresholdLabel.Text = beachThresholdText;
        grassThresholdLabel.Text = grassThresholdText;
        mountainThresholdLabel.Text = mountainThresholdText;

    }

    public void OnHidePressed()
    {
        ShowHideContainer.Visible = !ShowHideContainer.Visible; // Toggle visibility
        HideButton.Text = ShowHideContainer.Visible ? "Hide Controls" : "Show Controls";
    }

    public void onRegeneratePressed()
    {
        // Emit a signal to let the main scene know parameters have changed and it should regenerate the noise map
        EmitSignal(SignalName.ParametersChanged);
        GD.Print("Parameters Changed! Emitting signal to regenerate noise map...");
    }








}
