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

    public override void SimulateStep()
    {
        throw new System.NotImplementedException();
    }

    private void UpdateParticleDensities()
    {
        //TODO: Implement density updates
    }
    
    private void ComputeBoundaries()
    {
        //TODO: Implement boundary conditions
    }


    private Vector3 GetParticleAcceleration(int particleIndex)
    {
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
        return Vector3.zero; // TODO: Implement external forces
    }

    private Vector3 GetParticleViscosity(int particleIndex)
    {
        return Vector3.zero; // TODO: Implement viscosity
    }

    private Vector3 GetParticleModelingPressure(int particleIndex)
    {
        Vector3 pressureForce = Vector3.zero;
        for (int i = 0; i < particles.Length; i++)
        {
            if (i == particleIndex) 
                continue;

            pressureForce += particleMass 
                * ((GetParticlePressure(i) + GetParticlePressure(particleIndex)) / (2.0f * particles[i].density))
                * KernelGradient(particleIndex, i);
        }
        return pressureForce;
    }

        private float GetParticlePressure(int particleIndex)
    {
        return gasCosntant * (particles[particleIndex].density - restDensity);
    }

    private Vector3 Kernel(int particleIndexA, int particleIndexB)
    {
        return Vector3.zero; // TODO: Implement kernel
    }

    private Vector3 KernelGradient(int particleIndexA, int particleIndexB)
    {
        return Vector3.zero; // TODO: Implement kernel gradient
    }
}