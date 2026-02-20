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

    // Split Chance
    public string splitChanceText;
    public float splitChance;

    // Seed
    public int seed;

    // Nodes
    public Button regenerateButton;
    public Label widthLabel;
    public Label heightLabel;
    public Label minDepthLabel;
    public Label maxDepthLabel;
    public Label splitChanceLabel;

    public override void _Ready()
    {
        // Get all the nodes we need
        // 1. Labels
        widthLabel = GetNode<Label>("ControlContainer/ShowHideContainer/Width");
        heightLabel = GetNode<Label>("ControlContainer/ShowHideContainer/Height");
        minDepthLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MinDepth");
        maxDepthLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MaxDepth");
        splitChanceLabel = GetNode<Label>("ControlContainer/ShowHideContainer/SplitChance");

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

}
