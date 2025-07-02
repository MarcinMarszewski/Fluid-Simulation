using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;
using System;
using System.Threading.Tasks;

public class SimpleSPHSimulator : MonoBehaviour
{
        //Simulation parameter
    [SerializeField]
    private float particleGridSize = 20;
    [SerializeField]
    private float spawnSpacingMultiplier = 1.0f;
    [SerializeField]
    private float simulationTimeStep = 0.01f;

        //Base
    [SerializeField]
    private float mass = 1.0f;
    [SerializeField]
    private float smoothingRadius = 1.0f;

        //Boundary
    [SerializeField]
    private float dampingFactor = 1.0f;
    [SerializeField]
    private float boundaryDistance = 3.0f;

        //External forces
    [SerializeField]
    private float gravity = 1.0f;

        //Pressure
    [SerializeField]
    private float restDensity = 1.0f;
    [SerializeField]
    private float pressureMultiplier = 1.0f;
    [SerializeField]
    private float nearPressureMultiplier = 1.0f;

        //Viscosity
    [SerializeField]
    private float viscosityMultiplier = 1.0f;


    public List<Particle> particles = new List<Particle>();

    private System.Random random = new System.Random();


    void Start(){
        for(int i = 0; i<particleGridSize; i++){
            for (int j = 0; j < particleGridSize; j++)
            {
                Particle particle = new Particle();
                particle.position = new Vector2(i * spawnSpacingMultiplier / restDensity, j * spawnSpacingMultiplier / restDensity);
                particle.predictedPosition = new Vector2(0.0f, 0.0f);
                particle.velocity = new Vector2(0.0f, 0.0f);
                particle.density = 0.0f;
                particle.nearDensity = 0.0f;
                particles.Add(particle);
            }
        }
    }

    void Update() {
        SimulateStep();
    }


    public void SimulateStep() {
        ResolveBoundaryCollision();
        PredictPositions();
        ComputeDensities();
        ComputeNearDensities();
        SimulateExternalForces();
        SimulatePressureForces();
        SimulateViscosityForces();
        ApplyVelocities();
    }

    private void ResolveBoundaryCollision() {
        Parallel.For(0, particles.Count, i =>
        {
            Particle particle = particles[i];
            if (particle.position.x < 0 || particle.position.x > boundaryDistance)
            {
                particle.velocity.x = -particle.velocity.x * dampingFactor;
                particle.position.x = Mathf.Clamp(particle.position.x, 0, boundaryDistance);
            }
            if (particle.position.y < 0 || particle.position.y > boundaryDistance)
            {
                particle.velocity.y = -particle.velocity.y * dampingFactor;
                particle.position.y = Mathf.Clamp(particle.position.y, 0, boundaryDistance);
            }
            particles[i] = particle;
        });
    }

    private void PredictPositions(){
        Parallel.For(0, particles.Count, i =>
        {
            Particle particle = particles[i];
            particle.predictedPosition = particle.position + particle.velocity * simulationTimeStep;
            particles[i] = particle;
        });
    }

    private void ComputeDensities() {
        Parallel.For(0, particles.Count, i =>
        {
            Particle particle = particles[i];
            particle.density = 0.0f;
            for (int j = 0; j < particles.Count; j++)
            {
                if (i != j)
                {
                    Vector2 r = particle.predictedPosition - particles[j].predictedPosition;
                    float distance = r.magnitude;
                    if (distance < smoothingRadius)
                    {
                        particle.density += mass * Kernel.Quad(smoothingRadius, distance);
                    }
                }
            }
            particles[i] = particle;
        });
    }

    private void ComputeNearDensities() {
        Parallel.For(0, particles.Count, i =>
        {
            Particle particle = particles[i];
            particle.nearDensity = 0.0f;
            for (int j = 0; j < particles.Count; j++)
            {
                if (i != j)
                {
                    Vector2 r = particle.predictedPosition - particles[j].predictedPosition;
                    float distance = r.magnitude;
                    if (distance < smoothingRadius)
                    {
                        particle.nearDensity += mass * Kernel.NearDensity(smoothingRadius, distance);
                    }
                }
            }
            particles[i] = particle;
        });
    }

    private void SimulateExternalForces() {
        Parallel.For(0, particles.Count, i =>
        {
            if (particles[i].density < 0) return;
            Particle particle = particles[i];
            particle.velocity += new Vector2(0.0f, -gravity) * simulationTimeStep / particle.density;
            particles[i] = particle;
        });
    }

    private void SimulatePressureForces() {
        Parallel.For(0, particles.Count, i =>
        {
            Particle particle = particles[i];
            Vector2 pressureGradient = Vector2.zero;
            for (int j = 0; j < particles.Count; j++)
            {
                if (i == j)
                    continue;
                Particle other = particles[j];
                float distance = Vector2.Distance(particle.position, other.position);
            Vector2 direction = distance == 0 ? new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble()-0.5f) : (particle.position - other.position).normalized;
                float slope = Kernel.GradQuad(smoothingRadius, distance);
                if (slope == 0)
                    continue;
                float sharedPressure = (DensityToPressure(particle.density) + DensityToPressure(other.density)) / 2.0f;
                float sharedNearPressure = (particle.nearDensity + other.nearDensity) * nearPressureMultiplier / 2.0f;
                pressureGradient += (-sharedNearPressure - sharedPressure) * slope * direction * mass / other.density;
            }
            if (particle.density > 0)
            {
                particle.velocity += pressureGradient * simulationTimeStep / particle.density;
            }
            particles[i] = particle;
        });
    }

    private void SimulateViscosityForces() {
        Parallel.For(0, particles.Count, i =>
        {
            Particle particle = particles[i];
            Vector2 viscosityForce = Vector2.zero;
            for (int j = 0; j < particles.Count; j++)
            {
                if (i == j) continue;
                Particle other = particles[j];
                float distance = Vector2.Distance(particle.position, other.position);
                if (distance < smoothingRadius)
                {
                    float influence = Kernel.Cubic(smoothingRadius, distance);
                    viscosityForce += (other.velocity - particle.velocity) * influence * mass / other.density;
                }
            }
            if (particle.density > 0)
            {
                particle.velocity += viscosityForce * simulationTimeStep * viscosityMultiplier / particle.density;
            }
            particles[i] = particle;
        });
    }

    private void ApplyVelocities(){
        Parallel.For(0, particles.Count, i =>
        {
            Particle particle = particles[i];
            particle.position += particle.velocity * simulationTimeStep;
            particles[i] = particle;
        });
    }


    private float DensityToPressure(float density) {
        return pressureMultiplier * (density - restDensity);
    }

    void OnDrawGizmos() {
        Console.WriteLine("Drawing Gizmos for particles");
        Gizmos.color = Color.blue;
        if (particles == null || particles.Count == 0) {
            // Preview particles in edit mode
            for (int i = 0; i < particleGridSize; i++) {
                for (int j = 0; j < particleGridSize; j++)
                {
                    Vector2 pos = new Vector2(i * spawnSpacingMultiplier / restDensity, j * spawnSpacingMultiplier / restDensity);
                    Gizmos.DrawSphere(new Vector3(pos.x, pos.y, 0), 0.1f);
                    Console.WriteLine($"Particle Position: {pos.x}, {pos.y}");
                }
            }
        } else {
            foreach (Particle particle in particles) {
                Gizmos.DrawSphere(new Vector3(particle.position.x, particle.position.y, 0), 0.1f);
            }
        }
    }
}
