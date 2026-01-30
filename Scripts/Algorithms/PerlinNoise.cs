using System;

public static class PerlinNoise
{
    public static float[,] Generate2D(
        int width, int height,
        int seed,
        int octaves,
        float lacunarity,
        float persistence,
        float scale)
    {
        float[,] map = new float[height, width];

        // Prevent division by zero / ugly “everything same”
        if (scale <= 0.0001f) scale = 0.0001f;

        var rng = new Random(seed);

        // Precompute octave offsets (gives nicer variety)
        float[] offX = new float[octaves];
        float[] offY = new float[octaves];
        for (int i = 0; i < octaves; i++)
        {
            offX[i] = (float)(rng.NextDouble() * 20000 - 10000);
            offY[i] = (float)(rng.NextDouble() * 20000 - 10000);
        }

        float minV = float.MaxValue;
        float maxV = float.MinValue;

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            float amplitude = 1f;
            float frequency = 1f;
            float value = 0f;

            for (int o = 0; o < octaves; o++)
            {
                float sx = (x / scale) * frequency + offX[o];
                float sy = (y / scale) * frequency + offY[o];

                value += Noise(sx, sy, seed) * amplitude;

                amplitude *= persistence;
                frequency *= lacunarity;
            }

            if (value < minV) minV = value;
            if (value > maxV) maxV = value;

            map[y, x] = value;
        }

        // Normalize to 0..1
        float range = maxV - minV;
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
            map[y, x] = (map[y, x] - minV) / (range == 0 ? 1 : range);

        return map;
    }

    // 2D gradient noise
    private static float Noise(float x, float y, int seed)
    {
        int x0 = FastFloor(x);
        int y0 = FastFloor(y);
        int x1 = x0 + 1;
        int y1 = y0 + 1;

        float sx = x - x0;
        float sy = y - y0;

        float n0 = DotGridGradient(x0, y0, x, y, seed);
        float n1 = DotGridGradient(x1, y0, x, y, seed);
        float ix0 = Lerp(n0, n1, Fade(sx));

        n0 = DotGridGradient(x0, y1, x, y, seed);
        n1 = DotGridGradient(x1, y1, x, y, seed);
        float ix1 = Lerp(n0, n1, Fade(sx));

        float v = Lerp(ix0, ix1, Fade(sy));

        // Convert from roughly -1..1 to -1..1 (already), keep as is
        return v;
    }

    private static float DotGridGradient(int ix, int iy, float x, float y, int seed)
    {
        // Pseudo-random gradient from hash
        int h = Hash(ix, iy, seed);
        // 8 directions
        float gx, gy;
        switch (h & 7)
        {
            case 0: gx = 1;  gy = 0;  break;
            case 1: gx = -1; gy = 0;  break;
            case 2: gx = 0;  gy = 1;  break;
            case 3: gx = 0;  gy = -1; break;
            case 4: gx = 0.7071f;  gy = 0.7071f;  break;
            case 5: gx = -0.7071f; gy = 0.7071f;  break;
            case 6: gx = 0.7071f;  gy = -0.7071f; break;
            default: gx = -0.7071f; gy = -0.7071f; break;
        }

        float dx = x - ix;
        float dy = y - iy;

        return dx * gx + dy * gy;
    }

    private static int Hash(int x, int y, int seed)
    {
        unchecked
        {
            int h = seed;
            h ^= x * 374761393;
            h = (h << 13) | (h >> 19);
            h ^= y * 668265263;
            h *= 1274126177;
            return h;
        }
    }

    private static int FastFloor(float f) => (f >= 0) ? (int)f : (int)f - 1;

    private static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);

    private static float Lerp(float a, float b, float t) => a + (b - a) * t;
}
