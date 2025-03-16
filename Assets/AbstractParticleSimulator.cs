using UnityEngine;

public abstract class AbstractParticleSimulator : MonoBehaviour
{
    public Particle[] particles {get; set;}
    public abstract void SimulateStep();
}