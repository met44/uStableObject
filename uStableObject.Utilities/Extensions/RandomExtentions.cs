using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    public static class         RandomEx
    {
        public static float     PerlinNormal(Vector2 p, int octaves, float offset, float frequency, float amplitude, float lacunarity, float persistence)
        {
	        float               sum = 0;

	        for (int i = 0; i < octaves; i++)
	        {
		        float h = 0;
		        h = Mathf.PerlinNoise((p.x + offset) * frequency, (p.y + offset) * frequency);
		        sum += h * amplitude;
		        frequency *= lacunarity;
		        amplitude *= persistence;
	        }
	        return sum;
        }

        public static float     PerlinBillowed(Vector2 p, int octaves, float offset, float frequency, float amplitude, float lacunarity, float persistence)
        {
	        float               sum = 0;

	        for (int i = 0; i < octaves; i++)
	        {
		        float h = 0;
		        h = Mathf.Abs(Mathf.PerlinNoise((p.x + offset) * frequency, (p.y + offset) * frequency));
		        sum += h * amplitude;
		        frequency *= lacunarity;
		        amplitude *= persistence;
	        }
	        return sum;
        }

        public static float     PerlinRidged(Vector2 p, int octaves, float offset, float frequency, float amplitude, float lacunarity, float persistence, float ridgeOffset)
        {
	        float               sum = 0;

	        for (int i = 0; i < octaves; i++)
	        {
		        float h = 0;
		        h = 0.5f * (ridgeOffset - Mathf.Abs(4f * Mathf.PerlinNoise((p.x + offset) * frequency, (p.y + offset) * frequency)));
		        sum += h * amplitude;
		        frequency *= lacunarity;
		        amplitude *= persistence;
	        }
	        return sum;
        }
    }
}
