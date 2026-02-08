using Godot;
using System;

public partial class PerlinNoiseDemo : Node2D
{
    private Node controls;
    private DrawNoiseView drawNoiseView;

    public override void _Ready()
    {
        drawNoiseView = GetNode<DrawNoiseView>("DrawNoiseView");
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
        HSlider widthSlider   = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/WidthSlider");
        HSlider heightSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/HeightSlider");

        CheckBox useFbmBox    = controls.GetNode<CheckBox>("ControlContainer/ShowHideContainer/CheckBox");
        HSlider octavesSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/NoiseSlider");
        LineEdit seedBox      = controls.GetNode<LineEdit>("ControlContainer/ShowHideContainer/LineEdit");

        // thresholds
        HSlider deepSlider     = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/DeepWaterSlider");
        HSlider shallowSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/ShallowWaterSlider");
        HSlider beachSlider    = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/BeachSlider");
        HSlider grassSlider    = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/GrassSlider");
        HSlider mountainSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/MountainSlider");
        HSlider scaleSlider      = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/ScaleSlider");

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

        drawNoiseView.SetThresholds(deepThreshold, shallowThreshold, beachThreshold, grassThreshold, mountainThreshold);


        float scale = (float)scaleSlider.Value; // put this in a slider in the UI
        float persistence = 0.5f;
        float lacunarity = 0.5f;
        // Generate the noise map based on the parameters
        float[,] noiseValues = GenerateNoiseMap(chunkWidth, chunkHeight, seed, useFbm, octaves, scale, persistence, lacunarity);
        drawNoiseView.Visible = true;
        drawNoiseView.SetNoise(noiseValues);
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
                noiseMap[y, x] = noiseValue;
            }
        }

            return noiseMap;
    }

    
   
}