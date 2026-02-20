using Godot;
using System;

public partial class RandomWalkControls : CanvasLayer
{
    [Signal] // Lets send a signal when parameters change
    public delegate void ParametersChangedEventHandler();

    // Min Step
    public Label minStepLabel;
    public HSlider minStepSlider;
    public int minStep;

    // Max Step
    public Label maxStepLabel;
    public HSlider maxStepSlider;
    public int maxStep;

    // Step Chance
    public Label stepChanceLabel;
    public HSlider stepChanceSlider;
    public float stepChance;

    // Check Buttons
    public CheckButton allowLoopsCheckButton;
    public bool allowLoops;
    public CheckButton branchesCheckButton;
    public bool branches;
    public CheckButton allowBranchesToConnectCheckButton;
    public bool allowBranchesToConnect;

    // Branch Chance
    public Label branchChanceLabel;
    public HSlider branchChanceSlider;
    public float branchChance;

    // Regenerate And Hide Buttons
    public Button regenerateButton;
    private Button HideButton;
    private VBoxContainer ShowHideContainer;



    public override void _Ready()
    {
    GD.Print("Random Walk Controls Ready");
        // Get Labels and Sliders
        minStepLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MinStepsLabel");
        minStepSlider = GetNode<HSlider>("ControlContainer/ShowHideContainer/MinStepsSlider");

        maxStepLabel = GetNode<Label>("ControlContainer/ShowHideContainer/MaxStepsLabel");
        maxStepSlider = GetNode<HSlider>("ControlContainer/ShowHideContainer/MaxStepsSlider");

        stepChanceLabel = GetNode<Label>("ControlContainer/ShowHideContainer/StepChanceLabel");
        stepChanceSlider = GetNode<HSlider>("ControlContainer/ShowHideContainer/StepChanceSlider");

        allowLoopsCheckButton = GetNode<CheckButton>("ControlContainer/ShowHideContainer/AllowLoopsCheckButton");
        branchesCheckButton = GetNode<CheckButton>("ControlContainer/ShowHideContainer/BranchesCheckButton");
        allowBranchesToConnectCheckButton = GetNode<CheckButton>("ControlContainer/ShowHideContainer/AllowBranchesToConnect");

        branchChanceLabel = GetNode<Label>("ControlContainer/ShowHideContainer/BranchChanceLabel");
        branchChanceSlider = GetNode<HSlider>("ControlContainer/ShowHideContainer/BranchChanceSlider");

        // Get Hide Button and ShowHideContainer
        HideButton = GetNode<Button>("ControlContainer/Hide");
        ShowHideContainer = GetNode<VBoxContainer>("ControlContainer/ShowHideContainer");
        regenerateButton = GetNode<Button>("ControlContainer/ShowHideContainer/Generate");
        ShowHideContainer.Visible = true; // Start with controls visible
        HideButton.Pressed += () =>
        ProceduralGenerationUtilities.OnHideButtonPressed(HideButton, ShowHideContainer);

        regenerateButton.Pressed += () => 
        ProceduralGenerationUtilities.OnRegenerateButtonPressed(this);

    }

    public override void _Process(double delta)
    {
        // Update values from UI
        minStep = (int)minStepSlider.Value;
        maxStep = (int)maxStepSlider.Value;
        stepChance = (float)stepChanceSlider.Value;
        branchChance = (float)branchChanceSlider.Value;

        // Update label text
        minStepLabel.Text = $"Min Step: {minStep}";
        maxStepLabel.Text = $"Max Step: {maxStep}";
        stepChanceLabel.Text = $"Step Chance: {stepChance:F2}";
        branchChanceLabel.Text = $"Branch Chance: {branchChance:F2}";

    }
}
