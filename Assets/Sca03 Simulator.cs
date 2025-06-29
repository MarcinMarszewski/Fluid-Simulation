using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : AbstractParticleSimulator
{
    [SerializeField]
    private float gasCosntant = 1.0f;
    [SerializeField]
    private float restDensity = 1.0f;
    [SerializeField]
    private float particleMass = 1.0f;
    [SerializeField]
    private float smoothingRadius = 1.0f;
    [SerializeField]
    private float boundaryDistance = 2f;
    [SerializeField]
    private float simulationTimeStep = 0.01f;
    [SerializeField]
    private float viscosityCoefficient = 0.1f;
    [SerializeField]
    private float gravityConstant = 1.0f;
    [SerializeField]
    private float boundarySpringConstant = 0.9f;
    public override void SimulateStep()
    {
        /* UpdateParticleDensities();
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].velocity += GetParticleAcceleration(i);
            particles[i].position += particles[i].velocity * simulationTimeStep; // Where do we apply time factor?
        }

        ComputeBoundaries(); */
    }

    /* private void UpdateParticleDensities()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].density = 0.0f;
            for (int j = 0; j < particles.Length; j++)
            {
                if (i == j)
                    continue;
                particles[i].density += particleMass * Poly6Kernel(i, j);
            }
        }
    }

    private void ComputeBoundaries()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].position.x < -boundaryDistance || particles[i].position.x > boundaryDistance)
            {
                particles[i].velocity.x *= -boundarySpringConstant;
                particles[i].position.x = Mathf.Clamp(particles[i].position.x, -boundaryDistance, boundaryDistance);
            }
            if (particles[i].position.y < -boundaryDistance || particles[i].position.y > boundaryDistance)
            {
                particles[i].velocity.y *= -boundarySpringConstant;
                particles[i].position.y = Mathf.Clamp(particles[i].position.y, -boundaryDistance, boundaryDistance);
            }
            if (particles[i].position.z < -boundaryDistance || particles[i].position.z > boundaryDistance)
            {
                particles[i].velocity.z *= -boundarySpringConstant;
                particles[i].position.z = Mathf.Clamp(particles[i].position.z, -boundaryDistance, boundaryDistance);
            }
        }
    }


    private Vector3 GetParticleAcceleration(int particleIndex)
    {
        if (particles[particleIndex].density == 0.0f)
            return Vector3.zero;
        return GetParticleForces(particleIndex) / particles[particleIndex].density;
    }

    private Vector3 GetParticleForces(int particleIndex)
    {
        return GetParticleModelingPressure(particleIndex)
            + GetParticleExternalForces(particleIndex)
            + GetParticleViscosity(particleIndex);
    }

    private Vector3 GetParticleExternalForces(int particleIndex)
    {
        Vector3 externalForce = Vector3.zero;
        externalForce += Vector3.down * gravityConstant * particles[particleIndex].density;
        return externalForce;
    }

    private Vector3 GetParticleViscosity(int particleIndex)
    {
        Vector3 viscosityForce = Vector3.zero;
        for (int i = 0; i < particles.Length; i++)
        {
            if (i == particleIndex || particles[i].density == 0.0f)
                continue;

            viscosityForce += particleMass
                * ((particles[i].velocity - particles[particleIndex].velocity) / particles[i].density)
                * Poly6Kernel(particleIndex, i);
        }
        return viscosityForce * viscosityCoefficient;
    }

    private Vector3 GetParticleModelingPressure(int particleIndex)
    {
        Vector3 pressureForce = Vector3.zero;
        for (int i = 0; i < particles.Length; i++)
        {
            if (i == particleIndex)
                continue;
            if (particles[i].density == 0.0f)
                continue;

            pressureForce += particleMass
                * ((GetParticlePressure(i) + GetParticlePressure(particleIndex)) / (2.0f * particles[i].density))
                * SpikyKernelGradient(particleIndex, i);
        }
        return pressureForce;
    }

    private float GetParticlePressure(int particleIndex)
    {
        return gasCosntant * (particles[particleIndex].density - restDensity);
    }

    private Vector3 SpikyKernel(int particleIndexA, int particleIndexB)
    {
        float distance = Vector3.Distance(particles[particleIndexA].position, particles[particleIndexB].position);
        if (distance > smoothingRadius)
            return Vector3.zero;
        return 15.0f / (Mathf.PI * Mathf.Pow(smoothingRadius, 6)
            * Mathf.Pow(smoothingRadius - distance, 3))
            * Vector3.Normalize(particles[particleIndexA].position - particles[particleIndexB].position);
    }

    private Vector3 SpikyKernelGradient(int particleIndexA, int particleIndexB)
    {
        float distance = Vector3.Distance(particles[particleIndexA].position, particles[particleIndexB].position);
        if (distance > smoothingRadius)
            return Vector3.zero;
        return 15.0f / (Mathf.PI * Mathf.Pow(smoothingRadius, 6)
            * (-0.25f) * Mathf.Pow(smoothingRadius - distance, 4))
            * Vector3.Normalize(particles[particleIndexA].position - particles[particleIndexB].position);
    }

    private float Poly6Kernel(int particleIndexA, int particleIndexB)
    {
        float distance = Vector3.Distance(particles[particleIndexA].position, particles[particleIndexB].position);
        if (distance > smoothingRadius)
            return 0.0f;
        return 315.0f / (64.0f * Mathf.PI * Mathf.Pow(smoothingRadius, 9))
            * Mathf.Pow(smoothingRadius*smoothingRadius - distance*distance, 6);
    } */
}