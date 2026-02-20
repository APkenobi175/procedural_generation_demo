using Godot;
using System.Collections.Generic;

public partial class DrawBspView : Node2D
{

    // How big each cell is
    [Export] public float CellSize = 0.1f;
    // How thick corridors should be when drawn
    [Export] public float CorridorWidth = 3f;

    private BspNode _root;
    private List<Room> _rooms = new();
    private List<Corridor> _corridors = new();

    public void SetData(BspNode root, List<Room> rooms, List<Corridor> corridors)
    {
        _root = root;
        _rooms = rooms ?? new List<Room>();
        _corridors = corridors ?? new List<Corridor>();
        QueueRedraw();
        CenterMap();
    }

    public override void _Draw()
    {
        if (_root == null) return;

        //1. Draw Corridor (lines)
        DrawCorridors();

        //2. Draw Rooms (filled rectangles)
        DrawRoomsFilled();
    }

    private void DrawRoomsFilled()
    {
        foreach (var room in _rooms)
        {
            Rect2I r = room.Rect;
            var topLeft = new Vector2(r.Position.X * CellSize, r.Position.Y * CellSize);
            var size = new Vector2(r.Size.X * CellSize, r.Size.Y * CellSize);

            DrawRect(new Rect2(topLeft, size), new Color(0.55f, 0.55f, 0.55f), filled: true);
        }
    }

    private void DrawCorridors()
    {
        var col = new Color(0.65f, 0.65f, 0.65f);

        foreach (var c in _corridors)
        {
            DrawLine(GridToWorld(c.A), GridToWorld(c.Corner), col, CorridorWidth, true);
            DrawLine(GridToWorld(c.Corner), GridToWorld(c.B), col, CorridorWidth, true);
        }
    }

    private Vector2 GridToWorld(Vector2I p)
    {
        return new Vector2((p.X + 0.5f) * CellSize, (p.Y + 0.5f) * CellSize);
    }

    private void CenterMap()
    {
        if (_root == null) return;

        float mapWidth = _root.Region.Size.X * CellSize;
        float mapHeight = _root.Region.Size.Y * CellSize;

        Vector2 viewportSize = GetViewportRect().Size;

        Position = new Vector2(
            (viewportSize.X - mapWidth) / 2f,
            (viewportSize.Y - mapHeight) / 2f
        );
    }
}