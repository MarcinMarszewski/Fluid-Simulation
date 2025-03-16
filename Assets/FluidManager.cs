using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidManager : MonoBehaviour
{
    [SerializeField]
    private int particleCount = 1000;
    
    private Particle[] particles;

    [SerializeField]
    private ParticleDisplay particleDisplay;

    [SerializeReference]
    private AbstractParticleSimulator particleSimulator;

    void Start()
    {
        particles = new Particle[particleCount];
        for (int i = 0; i < particleCount; i++)
        {
            particles[i] = new Particle();
            particles[i].position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
            particles[i].velocity = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            particles[i].color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
            particles[i].angularVelocity = Random.Range(-1, 1);
        }
        particleSimulator.particles = particles;
    }

    void Update()
    {
        particleSimulator.SimulateStep();
        particleDisplay.DisplayParticles(particleSimulator.particles);
    }
}