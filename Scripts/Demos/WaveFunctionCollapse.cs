using Godot;
using System;

public partial class WaveFunctionCollapse : Node2D
{
    private DrawWaveView drawWaveView;
    private CanvasLayer controls;

    public override void _Ready()
    {
        drawWaveView = GetNode<DrawWaveView>("DrawWaveView");
        controls = GetNode<CanvasLayer>("WaveControls");

        WaveControls wc = controls as WaveControls;
        wc.Connect("ParametersChanged", new Callable(this, nameof(SetupDemo)));
        SetupDemo(); // Initial setup

    }


    private void SetupDemo()
    {
        // Read the ui
        HSlider widthSlider  = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/WidthSlider");
        HSlider heightSlider = controls.GetNode<HSlider>("ControlContainer/ShowHideContainer/HeightSlider");
        LineEdit seedBox     = controls.GetNode<LineEdit>("ControlContainer/ShowHideContainer/LineEdit");
        OptionButton preset  = controls.GetNode<OptionButton>("ControlContainer/ShowHideContainer/WeightSetOptions");

        int w = (int)widthSlider.Value;
        int h = (int)heightSlider.Value;

        int seed;
        if (!int.TryParse(seedBox.Text, out seed)) seed = 0;
        if (seed == 0) seed = new Random().Next();

        int[] weights = GetWeightsForPreset((int)preset.Selected);


        // Road rules based on edge matching (N/E/S/W bits in tile index)
        ushort[,] rules = BuildRoadRules16();

        WFC wfc = new WFC();
        wfc.Initialize(w, h, rules, weights, seed);

        // If contradiction happens, we can retry a few times with new seeds
        bool ok = false;
        int attempts = 20;

        for (int i = 0; i < attempts; i++)
        {
            ok = wfc.WaveFunctionCollapse();
            if (ok) break;

            seed = new Random().Next();
            wfc.Initialize(w, h, rules, weights, seed);
        }

        if (!ok)
        {
            GD.PrintErr("WFC failed (contradiction). Try a different preset or smaller grid.");
        }


        int[,] tiles = new int[h, w];
        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            tiles[y, x] = wfc.GetCollapsedTile(x, y);
        }

        // Render 
        drawWaveView.SetTiles(tiles);
    }

    // -------------------------------------------------------
    // Weight Sets
    // 0: All Tiles
    // 1: No Intersections
    // 2: Short Walls
    // 3: All - Perfect Intersections
    // 4: Favored Empty Space
    // 5: No Dead Ends
    // 6: All Straights
    // 7: Favor Empty - No Dead Ends
    // -------------------------------------------------------
    private int[] GetWeightsForPreset(int selected)
    {
        if (selected == 1) return Preset_NoIntersections();
        if (selected == 2) return Preset_ShortWalls();
        if (selected == 3) return Preset_AllPerfectIntersections();
        if (selected == 4) return Preset_FavoredEmpty();
        if (selected == 5) return Preset_NoDeadEnds();
        if (selected == 6) return Preset_AllStraights();
        if (selected == 7) return Preset_FavorEmptyNoDeadEnds();
        return Preset_AllTiles();
    }

    private int[] Preset_AllTiles()
    {
        int[] w = new int[16];
        for (int i = 0; i < 16; i++) w[i] = 1;
        return w;
    }

    private int[] Preset_NoIntersections()
    {
        // Disable degree 3 and 4 tiles (T-junctions and 4-way)
        int[] w = new int[16];
        for (int t = 0; t < 16; t++)
        {
            int deg = EdgeDegree(t);
            if (deg == 3 || deg == 4) w[t] = 0;
            else w[t] = 1;
        }
        return w;
    }

    private int[] Preset_ShortWalls()
    {
        // Favor empty + dead ends + corners; de-emphasize long straights; remove intersections
        int[] w = new int[16];
        for (int t = 0; t < 16; t++)
        {
            int deg = EdgeDegree(t);

            if (deg == 3 || deg == 4)
            {
                w[t] = 0;
                continue;
            }

            if (deg == 0) w[t] = 10;          // empty
            else if (deg == 1) w[t] = 8;      // dead ends
            else if (IsCorner(t)) w[t] = 6;   // corners
            else w[t] = 1;                    // straights low
        }
        return w;
    }

    private int[] Preset_AllPerfectIntersections()
    {
        // Strongly favor 4-way and T's, still allow others
        int[] w = new int[16];
        for (int t = 0; t < 16; t++)
        {
            int deg = EdgeDegree(t);
            if (deg == 4) w[t] = 30;
            else if (deg == 3) w[t] = 15;
            else if (deg == 2) w[t] = 2;
            else w[t] = 1;
        }
        return w;
    }

    private int[] Preset_FavoredEmpty()
    {
        // Favor empty tiles more, but still allow all
        int[] w = new int[16];
        for (int t = 0; t < 16; t++)
        {
            if (t == 0) w[t] = 20; // empty
            else w[t] = 1;
        }
        return w;
    }

    private int[] Preset_NoDeadEnds()
    {
        // Disable dead ends (degree 1)
        int[] w = new int[16];
        for (int t = 0; t < 16; t++)
        {
            int deg = EdgeDegree(t);
            if (deg == 1) w[t] = 0;
            else w[t] = 1;
        }
        return w;
    }

    private int[] Preset_AllStraights()
    {
        // Favor straight paths (degree 2 straight) and empty, no intersections
        int[] w = new int[16];
        for (int t = 0; t < 16; t++)
        {
            int deg = EdgeDegree(t);
            if (deg == 2 && !IsCorner(t)) w[t] = 10; // straights
            else if (deg == 0) w[t] = 5;               // empty
            else w[t] = 1;
        }
        return w;
    }
    private int[] Preset_FavorEmptyNoDeadEnds()
    {
        // Favor empty and straight paths, no dead ends, still allow intersections but low weight
        int[] w = new int[16];
        for (int t = 0; t < 16; t++)
        {
            int deg = EdgeDegree(t);
            if (deg == 0) w[t] = 20; // empty
            else if (deg == 2 && !IsCorner(t)) w[t] = 10; // straights
            else if (deg == 1) w[t] = 0; // no dead ends
            else w[t] = 1;
        }
        return w;
    }

    private ushort[,] BuildRoadRules16()
    {
        ushort[,] rules = new ushort[16, 4];

        for (int a = 0; a < 16; a++)
        {
            for (int di = 0; di < 4; di++)
            {
                WFC.Dir dir = (WFC.Dir)di;
                bool aEdge = HasEdge(a, dir);

                ushort allowed = 0;
                for (int b = 0; b < 16; b++)
                {
                    bool bEdge = HasEdge(b, Opp(dir));
                    if (aEdge == bEdge)
                    {
                        allowed = (ushort)(allowed | TileBitManual(b));
                    }
                }

                rules[a, di] = allowed;
            }
        }

        return rules;
    }

    private bool HasEdge(int tile, WFC.Dir d)
    {
        // tile index encodes edges: N=bit0,E=bit1,S=bit2,W=bit3
        int bit = (int)d;
        return ((tile >> bit) & 1) == 1;
    }

    private WFC.Dir Opp(WFC.Dir d)
    {
        if (d == WFC.Dir.N) return WFC.Dir.S;
        if (d == WFC.Dir.S) return WFC.Dir.N;
        if (d == WFC.Dir.E) return WFC.Dir.W;
        return WFC.Dir.E;
    }

    private ushort TileBitManual(int t)
    {
        if (t == 0) return WFC.T0;
        if (t == 1) return WFC.T1;
        if (t == 2) return WFC.T2;
        if (t == 3) return WFC.T3;
        if (t == 4) return WFC.T4;
        if (t == 5) return WFC.T5;
        if (t == 6) return WFC.T6;
        if (t == 7) return WFC.T7;
        if (t == 8) return WFC.T8;
        if (t == 9) return WFC.T9;
        if (t == 10) return WFC.T10;
        if (t == 11) return WFC.T11;
        if (t == 12) return WFC.T12;
        if (t == 13) return WFC.T13;
        if (t == 14) return WFC.T14;
        return WFC.T15;
    }

    private int EdgeDegree(int tile)
    {
        int v = tile & 0xF;
        int c = 0;
        while (v != 0)
        {
            v &= (v - 1);
            c++;
        }
        return c;
    }

    private bool IsCorner(int tile)
    {
        int n = (tile >> 0) & 1;
        int e = (tile >> 1) & 1;
        int s = (tile >> 2) & 1;
        int w = (tile >> 3) & 1;

        int deg = n + e + s + w;
        if (deg != 2) return false;

        // straight if N+S or E+W
        if (n == 1 && s == 1) return false;
        if (e == 1 && w == 1) return false;

        return true;
    }
}
