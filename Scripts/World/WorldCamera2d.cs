using Godot;
using System;

public partial class WorldCamera2d : Camera2D
{
    //TODO: We want to add the ability to zoom, and pan the camera

    [Export]
    public float ZoomStep = 0.05f; // The amount to zoom in/out with each step
    [Export]
    public float MinZoom = 0.02f; // Minimum zoom level
    [Export]
    public float MaxZoom = 4.0f; // Maximum zoom level

    private bool dragging; // Is the camera currently being dragged?
    private Vector2 lastMousePosition; // Last mouse position for dragging



    public override void _Ready()
    {
        // Set the initial zoom level
        if (GameManager.Instance.CellularActive){
            // More zoomed in for cellular automata since the grid is smaller and we want to see more detail
            Zoom = new Vector2(0.25f, 0.25f);
        }
         else if (GameManager.Instance.PerlinActive)
        {
                Zoom = new Vector2(.02f, .02f);
        }
        else if(GameManager.Instance.WFCActive)  
        {
                Zoom = new Vector2(.17f, .17f);
        }
        else if (GameManager.Instance.BSPActive)  
        {
                Zoom = new Vector2(.25f, .25f);
        }
        else
        {
            Zoom = new Vector2(0.1f, 0.1f); // Default zoom level for other demos
        }


    }
    public override void _UnhandledInput(InputEvent e)
    {
        if (e is InputEventMouseButton mb)
        {
            if (mb.ButtonIndex == MouseButton.Left)
            {
                dragging = mb.Pressed;
                lastMousePosition = mb.Position;
            }

            if (mb.ButtonIndex == MouseButton.WheelUp && mb.Pressed)
            {
                SetZoomClamped(Zoom - Vector2.One * ZoomStep);

            }
            if (mb.ButtonIndex == MouseButton.WheelDown && mb.Pressed)
            {
                SetZoomClamped(Zoom + Vector2.One * ZoomStep);
            }
        }
        if (e is InputEventMouseMotion mm && dragging)
        {
            var current = GetViewport().GetMousePosition();
            var delta = current - lastMousePosition;
            GlobalPosition -= delta / Zoom; // Adjust for zoom level
            lastMousePosition = current;
        }
    }

    private void SetZoomClamped(Vector2 z)
    {
        var v = Mathf.Clamp(z.X, MinZoom, MaxZoom);
        Zoom = new Vector2(v, v);
    } 
}
