using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Kernel
{
    public static float Quad(float h, float r){
        if (r > h) return 0f;
        float volume = Mathf.PI * Mathf.Pow(h, 3) / 6f;
        return Mathf.Pow(h - r, 2) / volume;
    }

    public static float GradQuad(float h, float r){
        if (r > h) return 0f;
        float scale = 12f / (Mathf.Pow(h, 4) * Mathf.PI);
        return (r - h) * scale;
    }

    public static float NearDensity(float h, float r){
        if (r > h) return 0f;
        float volume = Mathf.PI * Mathf.Pow(h, 5) / 4f;
        float val = Mathf.Max(0, h - r);
        return Mathf.Pow(val, 4) / volume;
    }

    public static float Cubic(float h, float r) {
        if (r > h) return 0f;
        float volume = Mathf.PI * Mathf.Pow(h, 8) / 4f;
        float val = Mathf.Max(0, h - r);
        return Mathf.Pow(val, 3) / volume;
    }
}
