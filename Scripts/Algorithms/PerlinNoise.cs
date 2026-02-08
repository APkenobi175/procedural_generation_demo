using System;

using Godot;

public static class PerlinNoise
{

    // 1. Gradient vectors for 2D Perlin noise
    // These are the 4 unit vectors pointing to the corners of a square
    private static readonly Vector2[] Gradients = new Vector2[]
    {
        new Vector2(1, 1),
        new Vector2(-1, 1),
        new Vector2(1, -1),
        new Vector2(-1, -1),
    };


    // 2. Permutation table for hashing

    private static int[] P = null;
    private static int currentSeed = int.MinValue;

    // Function to set the seed and generate permutation table
    public static void SetSeed(int seed)
    {
        if (P != null && seed == currentSeed) return;
        currentSeed = seed;

        int[] perm = new int[256];

        // Initialize the permutation with values 0-255

        for (int i = 0; i < 256; i++)
            perm[i] = i;

        // Shuffle the permutation using the seed.
        Random rand = new Random(seed);
        for (int i = 255; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (perm[i], perm[j]) = (perm[j], perm[i]);
        }
        // Duplicate the permutation
        P = new int[512];
        for (int i = 0; i < 512; i++)
            P[i] = perm[i & 255];
    }


    // Function to compute Perlin noise value at (x, y)

    public static float Noise2D(float x, float y, int seed = 0)
    {
        SetSeed(seed);

        // 1. integer coordinates of the square containing the point
        int x0 = (int)Math.Floor(x);
        int y0 = (int)Math.Floor(y);
        int x1 = x0 + 1;
        int y1 = y0 + 1;

        //2. Get the decimal part of the coordinates
        float dx = x - x0;
        float dy = y - y0;

        //3. Wrap the integer coordinates to 0-255 for hashing
        int X0 = x0 & 255; // Wrap to 0-255
        int Y0 = y0 & 255;
        int X1 = x1 & 255;
        int Y1 = y1 & 255;

        // 4. Hash the coordinates 

        int p00 = P[P[X0] + Y0];
        int p10 = P[P[X1] + Y0];
        int p01 = P[P[X0] + Y1];
        int p11 = P[P[X1] + Y1];

        // 5. Get the gradient vectors for the corners
        int g00 = p00 % Gradients.Length;
        int g10 = p10 % Gradients.Length;
        int g01 = p01 % Gradients.Length;
        int g11 = p11 % Gradients.Length;

        // 6. Compute the dot product between the gradient and the distance vector
        float d00 = Gradients[g00].Dot(new Vector2(dx, dy));
        float d10 = Gradients[g10].Dot(new Vector2(dx - 1f, dy));
        float d01 = Gradients[g01].Dot(new Vector2(dx, dy - 1f));
        float d11 = Gradients[g11].Dot(new Vector2(dx - 1f, dy - 1f));

        // 7. Smooth interpolation using formula from notes (t * t * t * (t * (t * 6 - 15) + 10))

        float sx = dx * dx * dx * (dx * (dx * 6 - 15) + 10);
        float sy = dy * dy * dy * (dy * (dy * 6 - 15) + 10);

        // 8. Lerp along x and then along y
        float ix0 = Mathf.Lerp(d00, d10, sx);
        float ix1 = Mathf.Lerp(d01, d11, sx);
        float result = Mathf.Lerp(ix0, ix1, sy);

        return result; // all done

    }



    // Fractal Noise OCTAVES
    public static float Fractal2D(float x, float y, int octaves, float persistence, int seed = 0)
    {
        float total = 0f;
        float frequency = 1f;
        float amplitude = 1f;
        float maxValue = 0f; // Used for normalizing result to 0 to 1



        for (int i = 0; i< octaves; i++)
        {
            total += Noise2D(x * frequency, y * frequency, seed) * amplitude;

            maxValue += amplitude;

            amplitude *= persistence; // Decrease amplitude for next octave
            frequency *= 2f; // Increase frequency for next octave
        }

        return (maxValue > 0f) ? total / maxValue : 0f; 

    }
}