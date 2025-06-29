using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Particle {
    public Vector2 position;
    public Vector2 predictedPosition;
    public Vector2 velocity;
    public float density;
    public float nearDensity;
}