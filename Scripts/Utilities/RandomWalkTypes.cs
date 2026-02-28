using Godot;

// Define a room and a hallway

public class RandomWalkRoom
{
    // Grid position of the room
    public Vector2I Position;

    // Constructor
    public RandomWalkRoom(Vector2I pos) { Position = pos; }
}

public class RandomWalkHallway
{
    // Position of hallway start and end points
    public Vector2I From;
    public Vector2I To;

    // Constructor
    public RandomWalkHallway(Vector2I from, Vector2I to) { From = from; To = to; }
}