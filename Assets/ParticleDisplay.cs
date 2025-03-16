using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisplay : MonoBehaviour
{
    private Particle[] particles { get; set; }

    ParticleDisplay()
    {
        particles = new Particle[0];
    }

    public void DisplayParticles(Particle[] particles)
    {
        this.particles = particles;
    }

    //TODO: Temporary, implement more efficient solution
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Particle particle in particles)
        {
            Gizmos.DrawSphere(particle.position, 0.1f);
        }
    }
}
