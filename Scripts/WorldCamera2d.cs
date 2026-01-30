using Godot;
using System;

public partial class WorldCamera2d : Camera2D
{
    //TODO: We want to add the ability to zoom, and pan the camera

    [Export]
    public float ZoomStep = 0.1f; // The amount to zoom in/out with each step
    [Export]
    public float MinZoom = 0.2f; // Minimum zoom level
    [Export]
    public float MaxZoom = 4.0f; // Maximum zoom level

    private bool dragging; // Is the camera currently being dragged?
    private Vector2 lastMousePosition; // Last mouse position for dragging




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
