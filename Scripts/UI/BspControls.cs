using Godot;
using System;

public partial class BspControls : CanvasLayer
{
    
    private Button HideButton;
    private VBoxContainer ShowHideContainer;

    [Signal] // Lets send a signal when parameters change
    public delegate void ParametersChangedEventHandler();

    // Width
    public string chunkWidthText;
    public int width;
    // Height
    public string chunkHeightText;
    public int height;

    // Mininum Depth
    public string minDepthText;
    public int minDepth;
    // Maximum Depth
    public string maxDepthText;
    public int maxDepth;

    // Minimum Rectangle Margin
    public string roomMarginMinText;
    public int roomMarginMin;
    // Maximum Rectangle Margin
    public string roomMarginMaxText;
    public int roomMarginMax;

    // Split Chance
    public string splitChanceText;
    public float splitChance;

    // Minimum Leaf Size
    public string minLeafSizeText;
    public int minLeafSize;
    // Minimum Room Size
    public string minRoomSizeText;
    public int minRoomSize;

    // Seed
    public int seed;

    // Nodes
    public Button regenerateButton;
    public Label widthLabel;
    public Label heightLabel;
    public Label minDepthLabel;
    public Label maxDepthLabel;
    public Label roomMarginMinLabel;
    public Label roomMarginMaxLabel;
    public Label splitChanceLabel;
    public Label minLeafSizeLabel;
    public Label minRoomSizeLabel;

    public override void _Ready()
    {
        // Get all the nodes we need to update
        // 1. Labels
        widthLabel = GetNode<Label>("ControlContainer/ShowHideContainer/ChunkWidth");
        heightLabel = GetNode<Label>("ControlContainer/ShowHideContainer/ChunkHeight");
        minDepthLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MinDepthLabel");
        maxDepthLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MaxDepthLabel");
        splitChanceLabel = GetNode<Label>("ControlContainer/ShowHideContainer/SplitChanceLabel");
        roomMarginMinLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MinMargin");
        roomMarginMaxLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MaxMargin");
        minLeafSizeLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MinLeafSize");
        // minRoomSizeLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MinRoomSize");

        // UI Elements (buttons, containers)
        regenerateButton = GetNode<Button>("ControlContainer/ShowHideContainer/Generate");
        HideButton = GetNode<Button>("ControlContainer/Hide");
        ShowHideContainer = GetNode<VBoxContainer>("ControlContainer/ShowHideContainer");
        ShowHideContainer.Visible = true; // Start with controls visible

        // Connect button signals to utilities class methods
        HideButton.Pressed += () =>
        ProceduralGenerationUtilities.OnHideButtonPressed(HideButton, ShowHideContainer);

        regenerateButton.Pressed += () =>
        ProceduralGenerationUtilities.OnRegenerateButtonPressed(this);
    }

    public override void _Process(double delta)
    {
        width = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/WidthSlider").Value;
        height = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/HeightSlider").Value;
        minDepth = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/MinDepthSlider").Value;
        maxDepth = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/MaxDepthSlider").Value;
        splitChance = (float)GetNode<HSlider>("ControlContainer/ShowHideContainer/SplitChanceSlider").Value;
        roomMarginMin = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/MinMarginSlider").Value;
        roomMarginMax = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/MaxMarginSlider").Value;
        minLeafSize = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/MinLeafSizeSlider").Value;
        // minRoomSize = (int)GetNode<HSlider>("ControlContainer/ShowHideContainer/MinRoomSizeSlider").Value;
        UpdateLabels();
    }

    public void UpdateLabels()
    {
        widthLabel.Text = $"Width: {width}";
        heightLabel.Text = $"Height: {height}";
        minDepthLabel.Text = $"Min Depth: {minDepth}";
        maxDepthLabel.Text = $"Max Depth: {maxDepth}";
        splitChanceLabel.Text = $"Split Chance: {splitChance}";
        roomMarginMinLabel.Text = $"Min Margin: {roomMarginMin}";
        roomMarginMaxLabel.Text = $"Max Margin: {roomMarginMax}";
        minLeafSizeLabel.Text = $"Min Leaf Size: {minLeafSize}";
        // minRoomSizeLabel.Text = $"Min Room Size: {minRoomSize}";
    }

}
