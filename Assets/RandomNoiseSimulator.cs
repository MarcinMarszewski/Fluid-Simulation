using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RandomNoiseSimulator : AbstractParticleSimulator {

    public void SetParticles(Particle[] particles) {
        this.particles = particles;
    }

    public Particle[] GetParticles() {
        return particles;
    }

    override public void SimulateStep() {
        for (int i = 0; i < particles.Length; i++) {
            particles[i].position += particles[i].velocity * Time.deltaTime;
            particles[i].velocity += Random.insideUnitSphere * Time.deltaTime;
            particles[i].color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
            particles[i].angularVelocity += Random.Range(-1, 1);
        }
    }
}