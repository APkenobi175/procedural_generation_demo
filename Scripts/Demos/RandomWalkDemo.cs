using Godot;
using System;

public partial class RandomWalkDemo : Node2D
{
    
    private DrawRandomWalkView drawRandomView;
    private Node controls;
    private Label errorLabel;


    // SAME SET UP AS THE OTHERS
    public override void _Ready()
    {
        
        drawRandomView = GetNode<DrawRandomWalkView>("DrawRandomWalkView");
        controls = GetNode<Node>("RandomWalkControls");
        

        if(controls is RandomWalkControls rwc)
        {
            rwc.Connect("ParametersChanged", new Callable(this, nameof(SetupDemo)));
        }
        else
        {
            GD.PrintErr("Failed to connect ParametersChanged: node is not RandomWalkControls");
        }
        SetupDemo();
    }

    private void SetupDemo()
    {
        if (controls is not RandomWalkControls rwc) return;

        int minStep    = (int)controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/MinStepsSlider").Value;
        int maxStep    = (int)controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/MaxStepsSlider").Value;
        float stepChance   = (float)controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/StepChanceSlider").Value;
        float branchChance = (float)controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/BranchChanceSlider").Value;
        bool allowLoops    = controls.GetNode<CheckButton>("ControlContainer/ShowHideContainer/AllowLoopsCheckButton").ButtonPressed;
        bool allowBranches = controls.GetNode<CheckButton>("ControlContainer/ShowHideContainer/BranchesCheckButton").ButtonPressed;
        bool allowBranchesToConnect = controls.GetNode<CheckButton>("ControlContainer/ShowHideContainer/AllowBranchesToConnect").ButtonPressed;
        errorLabel = controls.GetNode<Label>("ControlContainer/ShowHideContainer/MaxLimit");
        int seed = (int)controls.GetNode<LineEdit>("ControlContainer/ShowHideContainer/SeedLineEdit").Text.ToInt();

        if (minStep > maxStep) minStep = maxStep;

        var randomWalk = new RandomWalk();

        var result = randomWalk.Generate(
            minStep,
            maxStep,
            stepChance,
            branchChance,
            allowLoops,
            allowBranches,
            allowBranchesToConnect,
            seed
        );

        if (result.maxRoomsHit)
        {
            GD.PrintErr("Max room limit hit");
            errorLabel.Text = "Max Room Limit or\nNumber of Recursive Calls Limit Hit\nDuring Generation. Dungeon Incomplete\n(Infinite Loop Protector)";

        }
        else
        {
            errorLabel.Text = "";
        }

        drawRandomView.SetData(result.Rooms, result.Hallways);
    }

}
