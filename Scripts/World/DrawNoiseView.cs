using Godot;
using Colors = Godot.Colors;

public partial class DrawNoiseView : Node2D
{
    [Export] public int CellSize = 8;

    private float[,] noise;

    // Defaults

    private float deepT = -.60f;
    private float shallowT = -.20f;
    private float beachT = 0.05f;
    private float grassT = 0.40f;
    private float mountainT = 0.70f;

    // Pass in a Perlin noise map instead of a boolean grid
    public void SetNoise(float[,] map)
    {
        noise = map;
        CenterMap();
        QueueRedraw();
    }

    public void SetThresholds(float deep, float shallow, float beach, float grass, float mountain)
    {

    // Make sure thresholds are between -0.99 and 1
    deepT = Mathf.Clamp(deep, -0.99f, 1f);
    shallowT = Mathf.Clamp(shallow, -0.99f, 1f);
    beachT = Mathf.Clamp(beach, -0.99f, 1f);
    grassT = Mathf.Clamp(grass, -0.99f, 1f);
    mountainT = Mathf.Clamp(mountain, -0.99f, 1f);

     // enforce ordering so bands always exist
     shallowT = Mathf.Max(shallowT, deepT);
     beachT = Mathf.Max(beachT, shallowT);
     grassT = Mathf.Max(grassT, beachT);
     mountainT = Mathf.Max(mountainT, grassT);

     GD.Print($"Thresholds set: deep={deepT}, shallow={shallowT}, beach={beachT}, grass={grassT}, mountain={mountainT}");

    QueueRedraw();
    }


    public override void _Draw()
    {
        if (noise == null) return;

        int h = noise.GetLength(0);
        int w = noise.GetLength(1);

        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            float v = noise[y, x];

            Color color;

            if (v < deepT)
                color = new Color(0f, 0f, 0.55f);      // deep water
            else if (v < shallowT)
                color = new Color (0f, 0f, 1f);          // shallow water
            else if (v < beachT)
                color = new Color(0.86f, 0.78f, 0.63f);    // beach
            else if (v < grassT)
                color = new Color(0f, 0.82f, 0f);         // grass
            else if (v < mountainT)
                color = new Color(0.55f, 0.27f, 0.07f);          // mountains
            else
                color = Colors.White;         // snow or highest peaks

            DrawRect(new Rect2(x * CellSize, y * CellSize, CellSize, CellSize),color);
        }
    }

    public void CenterMap()

    // This function centers the world on the screen by adjusting the position of the node based on its width and height. Now every time we regenerate the map it will be centered in the screen.
    {
        if (noise == null) return;

        int h = noise.GetLength(0);
        int w = noise.GetLength(1);

        // Center the map by adjusting the position of this node
        float mapWidth = w * CellSize;
        float mapHeight = h * CellSize;
        Position = new Vector2(-mapWidth / 2, -mapHeight / 2);
    }
}
