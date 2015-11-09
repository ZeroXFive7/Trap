using UnityEngine;

public static class PerlinNoise
{
    #region Constants

    private readonly static float[] GradientLookup1D = new float[4]
    {
        0,
        1,
        0,
        -1
    };

    private readonly static Vector2[] GradientLookup2D = new Vector2[8]
    {
        new Vector2(0,1),
        new Vector2(1,1),
        new Vector2(1,0),
        new Vector2(1,-1),
        new Vector2(0,-1),
        new Vector2(-1,-1),
        new Vector2(-1,0),
        new Vector2(-1,1)
    };

    private readonly static Vector3[] GradientLookup3D = new Vector3[16]
    {
        new Vector3(1,1,0),
        new Vector3(-1,1,0),
        new Vector3(1,-1,0),
        new Vector3(-1,-1,0),
        new Vector3(1,0,1),
        new Vector3(-1,0,1),
        new Vector3(1,0,-1),
        new Vector3(-1,0,-1),
        new Vector3(0,1,1),
        new Vector3(0,-1,1),
        new Vector3(0,1,-1),
        new Vector3(0,-1,-1),
        new Vector3(1,1,0),
        new Vector3(0,-1,1),
        new Vector3(-1,1,0),
        new Vector3( 0,-1,-1)
    };

    private readonly static Vector4[] GradientLookup4D = new Vector4[32]
    {
        new Vector4(0,-1,-1,-1),
        new Vector4(0,-1,-1,1),
        new Vector4(0,-1,1,-1),
        new Vector4(0,-1,1,1),
        new Vector4(0,1,-1,-1),
        new Vector4(0,1,-1,1 ),
        new Vector4(0,1,1,-1),
        new Vector4(0,1,1,1),
        new Vector4(-1,-1,0,-1),
        new Vector4(-1,1,0,-1),
        new Vector4(1,-1,0,-1),
        new Vector4(1,1,0,-1),
        new Vector4(-1,-1,0,1),
        new Vector4(-1,1,0,1),
        new Vector4(1,-1,0,1),
        new Vector4(1,1,0,1),
        new Vector4(-1,0,-1,-1),
        new Vector4(1,0,-1,-1),
        new Vector4(-1,0,-1,1),
        new Vector4(1,0,-1,1),
        new Vector4(-1,0,1,-1),
        new Vector4(1,0,1,-1),
        new Vector4(-1,0,1,1),
        new Vector4(1,0,1,1),
        new Vector4(0,-1,-1,0),
        new Vector4(0,-1,-1,0),
        new Vector4(0,-1,1,0),
        new Vector4(0,-1,1,0),
        new Vector4(0,1,-1,0),
        new Vector4(0,1,-1,0),
        new Vector4(0,1,1,0),
        new Vector4(0,1,1,0)
    };

    private const int PermutationCount = 256;

    #endregion

    public static Texture2D PermutationTexture
    {
        get; private set;
    }

    public static Texture2D GradientTexture2D
    {
        get;
        private set;
    }

    public static Texture2D GradientTexture3D
    {
        get;
        private set;
    }

    public static Texture2D GradientTexture4D
    {
        get;
        private set;
    }

    private static int[] p = null;

    static PerlinNoise()
    {
        PermutationTexture = null;

        GradientTexture2D = null;
        GradientTexture3D = null;
        GradientTexture4D = null;
    }

    public static void GenerateNoise(int seed)
    {
        GeneratePermutations(seed);
        GeneratePermutationTexture();

        GenerateGradientTexture2D();
        GenerateGradientTexture3D();
        GenerateGradientTexture4D();
    }

    #region Noise Generation
    
    public static float Noise(float x)
    {
        // Calculate unit point index.
        int xIndex = (int)x % PermutationCount;

        x -= (int)x;

        float xSmooth = Fade(x);

        // Hash coordinates for 2 unit cube corners.
        int a = p[xIndex] % 4;
        int b = p[xIndex + 1] % 4;

        // Blend gradient dot product results from 2 corners.
        return Mathf.Lerp(GradientLookup1D[a] * x,
                          GradientLookup1D[b] * (x - 1),
                          xSmooth);
    }

    public static float Noise(float x, float y)
    {
        // Calculate unit square indices.
        int xIndex = (int)x % PermutationCount;
        int yIndex = (int)y % PermutationCount;

        x -= (int)x;
        y -= (int)y;

        float xSmooth = Fade(x);
        float ySmooth = Fade(y);

        // Hash coordinates for 4 unit cube corners.
        int aa = p[p[xIndex    ] + yIndex    ] % 8;
        int ab = p[p[xIndex    ] + yIndex + 1] % 8;
        int ba = p[p[xIndex + 1] + yIndex    ] % 8;
        int bb = p[p[xIndex + 1] + yIndex + 1] % 8;

        // Blend gradient dot product results from 8 corners.
        return Mathf.Lerp(Mathf.Lerp(Vector2.Dot(GradientLookup2D[aa], new Vector2(x, y)),
                                     Vector2.Dot(GradientLookup2D[ba], new Vector2(x - 1, y)),
                                     xSmooth),
                          Mathf.Lerp(Vector2.Dot(GradientLookup2D[ab], new Vector2(x, y - 1)),
                                     Vector2.Dot(GradientLookup2D[bb], new Vector2(x - 1, y - 1)),
                                     xSmooth),
                          ySmooth);
    }

    public static float Noise(float x, float y, float z)
    {
        // Calculate unit cube indices.
        int xIndex = (int)x % PermutationCount;
        int yIndex = (int)y % PermutationCount;
        int zIndex = (int)z % PermutationCount;

        x -= (int)x;
        y -= (int)y;
        z -= (int)z;

        float xSmooth = Fade(x);
        float ySmooth = Fade(y);
        float zSmooth = Fade(z);

        // Hash coordinates for 8 unit cube corners.
        int aaa = p[p[p[xIndex    ] + yIndex    ] + zIndex    ] % 16;
        int aab = p[p[p[xIndex    ] + yIndex    ] + zIndex + 1] % 16;
        int aba = p[p[p[xIndex    ] + yIndex + 1] + zIndex    ] % 16;
        int abb = p[p[p[xIndex    ] + yIndex + 1] + zIndex + 1] % 16;
        int baa = p[p[p[xIndex + 1] + yIndex    ] + zIndex    ] % 16;
        int bab = p[p[p[xIndex + 1] + yIndex    ] + zIndex + 1] % 16;
        int bba = p[p[p[xIndex + 1] + yIndex + 1] + zIndex    ] % 16;
        int bbb = p[p[p[xIndex + 1] + yIndex + 1] + zIndex + 1] % 16;

        // Blend gradient dot product results from 8 corners.
        return Mathf.Lerp(
            Mathf.Lerp(
                Mathf.Lerp(Vector3.Dot(GradientLookup3D[aaa], new Vector3(x, y, z)),
                           Vector3.Dot(GradientLookup3D[baa], new Vector3(x - 1, y, z)),
                           xSmooth),
                Mathf.Lerp(Vector3.Dot(GradientLookup3D[aba], new Vector3(x, y - 1, z)),
                           Vector3.Dot(GradientLookup3D[bba], new Vector3(x - 1, y - 1, z)),
                           xSmooth),
                ySmooth),
            Mathf.Lerp(
                Mathf.Lerp(Vector3.Dot(GradientLookup3D[aab], new Vector3(x, y, z - 1)),
                           Vector3.Dot(GradientLookup3D[bab], new Vector3(x - 1, y, z - 1)),
                           xSmooth),
                Mathf.Lerp(Vector3.Dot(GradientLookup3D[abb], new Vector3(x, y - 1, z - 1)),
                           Vector3.Dot(GradientLookup3D[bbb], new Vector3(x - 1, y - 1, z - 1)),
                           xSmooth),
                ySmooth),
            zSmooth);
    }

    public static float Noise(float x, float y, float z, float w)
    {
        // Calculate unit hypercube indices.
        int xIndex = (int)x % PermutationCount;
        int yIndex = (int)y % PermutationCount;
        int zIndex = (int)z % PermutationCount;
        int wIndex = (int)w % PermutationCount;

        x -= (int)x;
        y -= (int)y;
        z -= (int)z;
        w -= (int)w;

        float xSmooth = Fade(x);
        float ySmooth = Fade(y);
        float zSmooth = Fade(z);
        float wSmooth = Fade(w);

        // Hash coordinates for 16 unit cube corners.
        int aaaa = p[p[p[p[xIndex    ] + yIndex    ] + zIndex    ] + wIndex    ] % 32;
        int aaab = p[p[p[p[xIndex    ] + yIndex    ] + zIndex    ] + wIndex + 1] % 32;
        int aaba = p[p[p[p[xIndex    ] + yIndex    ] + zIndex + 1] + wIndex    ] % 32;
        int aabb = p[p[p[p[xIndex    ] + yIndex    ] + zIndex + 1] + wIndex + 1] % 32;
        int abaa = p[p[p[p[xIndex    ] + yIndex + 1] + zIndex    ] + wIndex    ] % 32;
        int abab = p[p[p[p[xIndex    ] + yIndex + 1] + zIndex    ] + wIndex + 1] % 32;
        int abba = p[p[p[p[xIndex    ] + yIndex + 1] + zIndex + 1] + wIndex    ] % 32;
        int abbb = p[p[p[p[xIndex    ] + yIndex + 1] + zIndex + 1] + wIndex + 1] % 32;
        int baaa = p[p[p[p[xIndex + 1] + yIndex    ] + zIndex    ] + wIndex    ] % 32;
        int baab = p[p[p[p[xIndex + 1] + yIndex    ] + zIndex    ] + wIndex + 1] % 32;
        int baba = p[p[p[p[xIndex + 1] + yIndex    ] + zIndex + 1] + wIndex    ] % 32;
        int babb = p[p[p[p[xIndex + 1] + yIndex    ] + zIndex + 1] + wIndex + 1] % 32;
        int bbaa = p[p[p[p[xIndex + 1] + yIndex + 1] + zIndex    ] + wIndex    ] % 32;
        int bbab = p[p[p[p[xIndex + 1] + yIndex + 1] + zIndex    ] + wIndex + 1] % 32;
        int bbba = p[p[p[p[xIndex + 1] + yIndex + 1] + zIndex + 1] + wIndex    ] % 32;
        int bbbb = p[p[p[p[xIndex + 1] + yIndex + 1] + zIndex + 1] + wIndex + 1] % 32;

        // Blend gradient dot product results from 16 corners.
        return Mathf.Lerp(Mathf.Lerp(Mathf.Lerp(Mathf.Lerp(Vector4.Dot(GradientLookup4D[aaaa], new Vector4(x, y, z)),
                                                           Vector4.Dot(GradientLookup4D[baaa], new Vector4(x - 1, y, z, w)),
                                                           xSmooth),
                                                Mathf.Lerp(Vector4.Dot(GradientLookup4D[abaa], new Vector4(x, y - 1, z, w)),
                                                           Vector4.Dot(GradientLookup4D[bbaa], new Vector4(x - 1, y - 1, z, w)),
                                                           xSmooth),
                                                ySmooth),
                                     Mathf.Lerp(Mathf.Lerp(Vector4.Dot(GradientLookup4D[aaba], new Vector4(x, y, z - 1, w)),
                                                           Vector4.Dot(GradientLookup4D[baba], new Vector4(x - 1, y, z - 1, w)),
                                                           xSmooth),
                                                Mathf.Lerp(Vector4.Dot(GradientLookup4D[abba], new Vector4(x, y - 1, z - 1, w)),
                                                           Vector4.Dot(GradientLookup4D[bbba], new Vector4(x - 1, y - 1, z - 1, w)),
                                                           xSmooth),
                                                ySmooth),
                                     zSmooth),
                          Mathf.Lerp(Mathf.Lerp(Mathf.Lerp(Vector4.Dot(GradientLookup4D[aaab], new Vector4(x, y, z, w - 1)),
                                                           Vector4.Dot(GradientLookup4D[baab], new Vector4(x - 1, y, z, w - 1)),
                                                           xSmooth),
                                                Mathf.Lerp(Vector4.Dot(GradientLookup4D[abab], new Vector4(x, y - 1, z, w - 1)),
                                                           Vector4.Dot(GradientLookup4D[bbab], new Vector4(x - 1, y - 1, z, w - 1)),
                                                           xSmooth),
                                                ySmooth),
                                     Mathf.Lerp(Mathf.Lerp(Vector4.Dot(GradientLookup4D[aabb], new Vector4(x, y, z - 1, w - 1)),
                                                           Vector4.Dot(GradientLookup4D[babb], new Vector4(x - 1, y, z - 1, w - 1)),
                                                           xSmooth),
                                                Mathf.Lerp(Vector4.Dot(GradientLookup4D[abbb], new Vector4(x, y - 1, z - 1, w - 1)),
                                                           Vector4.Dot(GradientLookup4D[bbbb], new Vector4(x - 1, y - 1, z - 1, w - 1)),
                                                           xSmooth),
                                                ySmooth),
                                     zSmooth),
                           wSmooth);
    }

    private static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    #endregion

    #region Input Generation

    private static void GeneratePermutations(int seed)
    {
        p = new int[PermutationCount + PermutationCount];

        // Generate base permutations
        for (int i = 0; i < PermutationCount; ++i)
        {
            p[i] = i;
        }

        // Randomize permutation indices.
        UnityEngine.Random.seed = seed;

        int randomIndex, swapTemp;
        for (int i = 0; i < PermutationCount; ++i)
        {
            randomIndex = UnityEngine.Random.Range(0, PermutationCount);
            swapTemp = p[i];
            p[i] = p[randomIndex];
            p[randomIndex] = swapTemp;
        }

        // Duplicate entries to avoid overflow
        for (int i = 0; i < PermutationCount; ++i)
        {
            p[PermutationCount + i] = p[i];
        }
    }

    private static void GeneratePermutationTexture()
    {
        // Generate permutation texture 1D.
        PermutationTexture = new Texture2D(PermutationCount, 1, TextureFormat.Alpha8, false, true);
        PermutationTexture.filterMode = FilterMode.Point;
        PermutationTexture.wrapMode = TextureWrapMode.Repeat;

        Color[] pixels = new Color[PermutationCount];
        float maxValue = (float)(PermutationCount - 1);
        for (int i = 0; i < PermutationCount; ++i)
        {
            pixels[i] = new Color(0.0f, 0.0f, 0.0f, (float)p[i] / maxValue);
        }
        PermutationTexture.SetPixels(pixels);
        PermutationTexture.Apply();
    }

    private static void GenerateGradientTexture2D()
    {
        GradientTexture2D = new Texture2D(PermutationCount, 1, TextureFormat.RGB24, false, true);
        GradientTexture2D.filterMode = FilterMode.Point;
        GradientTexture2D.wrapMode = TextureWrapMode.Repeat;

        Color[] pixels = new Color[PermutationCount];
        int index = 0;
        for (int i = 0; i < PermutationCount; ++i)
        {
            index = i % 8;
            pixels[i] = new Color(GradientLookup2D[index].x, GradientLookup2D[index].y, 0, 1);
        }
        GradientTexture2D.SetPixels(pixels);
        GradientTexture2D.Apply();
    }

    private static void GenerateGradientTexture3D()
    {
        GradientTexture3D = new Texture2D(PermutationCount, 1, TextureFormat.RGB24, false, true);
        GradientTexture3D.filterMode = FilterMode.Point;
        GradientTexture3D.wrapMode = TextureWrapMode.Repeat;

        Color[] pixels = new Color[PermutationCount];
        int index = 0;
        for (int i = 0; i < PermutationCount; ++i)
        {
            index = i % 16;
            pixels[i] = new Color(GradientLookup3D[index].x, GradientLookup3D[index].y, GradientLookup3D[index].z, 1);
        }
        GradientTexture3D.SetPixels(pixels);
        GradientTexture3D.Apply();
    }

    private static void GenerateGradientTexture4D()
    {
        GradientTexture4D = new Texture2D(PermutationCount, 1, TextureFormat.RGB24, false, true);
        GradientTexture4D.filterMode = FilterMode.Point;
        GradientTexture4D.wrapMode = TextureWrapMode.Repeat;

        Color[] pixels = new Color[PermutationCount];
        int index = 0;
        for (int i = 0; i < PermutationCount; ++i)
        {
            index = 0 % 32;
            pixels[i] = GradientLookup4D[index];
        }
        GradientTexture4D.SetPixels(pixels);
        GradientTexture4D.Apply();
    }
    #endregion
}
