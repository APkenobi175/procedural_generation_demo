using Godot;
using System;

public partial class PerlinNoiseDemo : Node2D
{
    private Node controls;
    private TextureRect noiseView;

    public override void _Ready()
    {
        noiseView = GetNode<TextureRect>("NoiseView");
        controls = GetNode<Node>("PerlinControls");

        if (controls is PerlinControls pc)
        {
            pc.Connect("ParametersChanged", new Callable(this, nameof(SetupDemo)));
            GD.Print("Connected ParametersChanged signal from controls to SetupDemo");
        }
        else
        {
            GD.PrintErr("PerlinControls node is NOT a PerlinControls script instance.");
        }

        SetupDemo();
    }

    private void SetupDemo()
    {
        Random rng = new Random();

        // --- Read UI values
        HSlider widthSlider   = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/ChunkWidth");
        HSlider heightSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/ChunkHeight");

        CheckBox useFbmBox    = controls.GetNode<CheckBox>("ControlContainer/ShowHideContainer/UseFBM");
        HSlider octavesSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/NoiseOctaves");
        LineEdit seedBox      = controls.GetNode<LineEdit>("ControlContainer/ShowHideContainer/SeedLineEdit");

        // thresholds
        HSlider deepSlider     = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/DeepWaterThreshold");
        HSlider shallowSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/ShallowWaterThreshold");
        HSlider beachSlider    = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/BeachThreshold");
        HSlider grassSlider    = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/GrassThreshold");
        HSlider mountainSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/MountainThreshold");

        int chunkWidth = (int)widthSlider.Value;
        int chunkHeight = (int)heightSlider.Value;
        bool useFbm = useFbmBox.ButtonPressed;
        int octaves = (int)octavesSlider.Value;
        int seed = int.TryParse(seedBox.Text, out int s) ? s : 0;
        // if the seed is 0, generate a random seed
        if (seed == 0)        {
            seed = rng.Next(); // random seed
        }
        
        float deepThreshold = (float)deepSlider.Value;
        float shallowThreshold = (float)shallowSlider.Value;
        float beachThreshold = (float)beachSlider.Value;
        float grassThreshold = (float)grassSlider.Value;
        float mountainThreshold = (float)mountainSlider.Value;

        float scale = 0.05f;
        float persistence = 0.5f;
        float lacunarity = 2.0f;
        // Generate the noise map based on the parameters
        float[,] noiseValues = GenerateNoiseMap(chunkWidth, chunkHeight, seed, useFbm, octaves, scale, persistence, lacunarity);

        Image img = Image.Create(chunkWidth, chunkHeight, false, Image.Format.Rgb8);

        for (int y = 0; y < chunkHeight; y++)
            {
                for (int x = 0; x < chunkWidth; x++)
                {
                    // Based on the noise value lets assign a color for the terrain type
                    float v = noiseValues[y, x];
                    Color color;
                    if (v < deepThreshold)
                        color = new Color(0, 0, 0.5f); // Deep Water
                    else if (v < shallowThreshold)
                        color = new Color(0, 0, 1); // Shallow Water
                    else if (v < beachThreshold)
                        color = new Color(0.76f, 0.7f, 0.5f); // Beach
                    else if (v < grassThreshold)
                        color = new Color(0, 1, 0); // Grass
                    else if (v < mountainThreshold)
                        color = new Color(0.5f, 0.5f, 0); // Mountain
                    else
                        color = new Color(1, 1, 1); // Snow
                    img.SetPixel(x, y, color);

                }
            }

                ImageTexture tex = ImageTexture.CreateFromImage(img);
                noiseView.Texture = tex;
        
    }

    private float[,] GenerateNoiseMap(int width, int height, int seed, bool useFbm, int octaves, float scale, float persistence, float lacunarity)
    {
        float[,] noiseMap = new float[height, width];
        if (scale <= 0)
            scale = 0.0001f;

        Random rng = new Random(seed);
        float ox = (float)rng.NextDouble() * 1000f; // random offset for x
        float oy = (float)rng.NextDouble() * 1000f; // random offset for y

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                float noiseValue;
                if (useFbm)
                {
                    noiseValue = PerlinNoise.Fractal2D(x * scale + ox, y * scale + oy, octaves, persistence, seed);
                }
                else
                {
                    noiseValue = PerlinNoise.Noise2D(x * scale + ox, y * scale + oy, seed);
                }
                // Normalize to [0, 1]
                noiseMap[y, x] = (noiseValue + 1) / 2f;
            }
        }

            return noiseMap;
    }
   
}