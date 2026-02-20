using Godot;
using System;

// Defines what a room is
public class Room
{
    public Rect2I Rect;
    public Room(Rect2I rect)
    {
        Rect = rect;
    }
    public Vector2I Center => new Vector2I(Rect.Position.X + Rect.Size.X / 2, Rect.Position.Y + Rect.Size.Y / 2);
}

// Defines what a corridor is
public class Corridor
{
    // L-shaped corridor: a -> corner -> b
    public Vector2I A;
    public Vector2I Corner;
    public Vector2I B;

    public Corridor(Vector2I a, Vector2I corner, Vector2I b)
    {
        A = a; Corner = corner; B = b;
    }
}

// Defines what a node in the BSP tree is

public class BspNode
{
    public Rect2I Region;     // stores position + size (x,y,w,h)
    public BspNode Left;
    public BspNode Right;
    public bool IsLeaf => Left == null && Right == null;

    // Only leaf nodes get rooms (per pseudocode)
    public Room Room;

    public BspNode(Rect2I region)
    {
        Region = region;
    }

    public Vector2I Center => new Vector2I(
        Region.Position.X + Region.Size.X / 2,
        Region.Position.Y + Region.Size.Y / 2
    );
}

