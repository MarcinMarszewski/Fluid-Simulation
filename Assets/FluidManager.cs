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
            particles[i].position = new Vector3(((i%100)/10)/10.0f, (i%10)/10.0f, 0.0f); // 2d spawn
            particles[i].velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
        particleSimulator.particles = particles;
    }

    void Update()
    {
        particleSimulator.SimulateStep();
        particleDisplay.DisplayParticles(particleSimulator.particles);
    }
}