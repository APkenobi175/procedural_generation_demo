using Godot;
using System;
using System.Collections.Generic;

public partial class DrawRandomWalkView : Node2D
{
    private List<RandomWalkRoom> rooms = new();
    private List<RandomWalkHallway> hallways = new();

    // Visual settings
    [Export] public int CellSize    = 32;   // pixels per grid cell
    [Export] public int RoomRadius  = 10;   // radius of room (used to be circle, now used for half the width/height of a square)
    [Export] public int HallwayWidth = 4;  // line width for hallways

    [Export] public Color RoomColor     = new Color(0.78f, 0.78f, 0.78f);
    [Export] public Color HallwayColor  = new Color(0.78f, 0.78f, 0.78f);
    [Export] public Color StartColor    = new Color(0.2f, 1.0f, 0.4f);   // green for start room



    public void SetData(List<RandomWalkRoom> rooms, List<RandomWalkHallway> hallways)
    {
        this.rooms = rooms ?? new List<RandomWalkRoom>();
        this.hallways = hallways ?? new List<RandomWalkHallway>();
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (rooms == null || rooms.Count == 0) return;

        // Find bounding box of all rooms so we can center the drawing
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach (var room in rooms)
        {
            if (room.Position.X < minX) minX = room.Position.X;
            if (room.Position.Y < minY) minY = room.Position.Y;
            if (room.Position.X > maxX) maxX = room.Position.X;
            if (room.Position.Y > maxY) maxY = room.Position.Y;
        }

        // Offset so the dungeon is centered on the viewport center
        var viewport = GetViewportRect();
        float offsetX = viewport.Size.X / 2f - ((minX + maxX) / 2f) * CellSize;
        float offsetY = viewport.Size.Y / 2f - ((minY + maxY) / 2f) * CellSize;
        var offset = new Vector2(offsetX, offsetY);

        // Draw hallways first (so rooms appear on top)
        foreach (var hall in hallways)
        {
            Vector2 from = GridToScreen(hall.From, offset);
            Vector2 to   = GridToScreen(hall.To,   offset);
            DrawLine(from, to, HallwayColor, HallwayWidth);
        }

        // Draw rooms
        for (int i = 0; i < rooms.Count; i++)
        {
            Vector2 screenPos = GridToScreen(rooms[i].Position, offset);
            Color color = (i == 0) ? StartColor : RoomColor;
            //DrawCircle(screenPos, RoomRadius, color);
            var rect = new Rect2(screenPos - new Vector2(RoomRadius, RoomRadius), new Vector2(RoomRadius * 2, RoomRadius * 2));
            DrawRect(rect, color);
        }
    }

    private Vector2 GridToScreen(Vector2I gridPos, Vector2 offset)
    {
        return new Vector2(gridPos.X * CellSize + offset.X, gridPos.Y * CellSize + offset.Y);
    }




}
   