using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator {
    public static float[,] GenerateFalloffMap(int width, int height, float start, float end) {
        float[,] map = new float[width, height];

        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                float x = i / (float)width * 2 - 1;
                float y = j / (float)height * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

                if (value < start) {
                    map[i, j] = 1;
                } else if (value > end) {
                    map[i, j] = 0;
                } else {
                    map[i, j] = Mathf.SmoothStep(1, 0, Mathf.InverseLerp(start, end, value));
                }
            }
        }

        return map;
    }
}
