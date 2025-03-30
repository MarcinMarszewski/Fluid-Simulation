using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class SimpleSPHSimulator : AbstractParticleSimulator
{
    [SerializeField]
    private float gravity = 9.8f;
    [SerializeField]
    private float mass = 1.0f;
    [SerializeField]
    private float dampingFactor = 1.0f;
    [SerializeField]
    private float boundaryDistance = 1.0f;
    [SerializeField]
    private float smoothingRadius = 1.0f;
    [SerializeField]
    private float restDensity = 1.0f;
    [SerializeField]
    private float pressureStiffness = 1.0f;
    [SerializeField]
    private float simulationTimeStep = 0.01f;
    override public void SimulateStep() {
        ComputeDensities();
        SimulateGravityForces();
        SimulatePressureForces();
        ApplyVelocities();
        ResolveBoundaryCollision();
    }

    private void ApplyVelocities(){
        for (int i = 0; i < particles.Length; i++) {
            particles[i].position += particles[i].velocity * simulationTimeStep;
        }
    }

    private void SimulatePressureForces() {
        for (int i = 0; i < particles.Length; i++) {
            particles[i].velocity += PressureForceMultiplier(particles[i].density) 
            * PressureGradientAtPoint(particles[i].position) * simulationTimeStep / particles[i].density;
        }
    }

    private void SimulateGravityForces() {
        for (int i = 0; i < particles.Length; i++) {
            particles[i].velocity += Vector3.down * gravity * simulationTimeStep;
        }
    }

    private void ResolveBoundaryCollision() {
        for (int i = 0; i < particles.Length; i++) {
            if (particles[i].position.y < 0.0f) {
                particles[i].position.y = 0.0f;
                particles[i].velocity.y *= -dampingFactor;
            }
            if(Abs(particles[i].position.x) > boundaryDistance) {
                particles[i].position.x = boundaryDistance * Sign(particles[i].position.x);
                particles[i].velocity.x *= -dampingFactor;
            }
            if(Abs(particles[i].position.z) > boundaryDistance) {
                particles[i].position.z = boundaryDistance * Sign(particles[i].position.z);
                particles[i].velocity.z *= -dampingFactor;
            }
        }
    }

    private float DensityAtPoint(Vector3 point) {
        float density = 0.0f;
        for (int i = 0; i < particles.Length; i++) {
            density += mass * DebrunsKernel(Vector3.Distance(point, particles[i].position));
        }
        return density;
    }

    private Vector3 PressureGradientAtPoint(Vector3 point) {
        Vector3 pressureGradient = Vector3.zero;
        for (int i = 0; i < particles.Length; i++) {
            pressureGradient += mass * DebrunsKernelGradient(Vector3.Distance(point, particles[i].position)) 
            * ((particles[i].position - point).normalized) / particles[i].density;
        }
        return pressureGradient;
    }

    private void ComputeDensities() {
        for (int i = 0; i < particles.Length; i++) {
            particles[i].density = DensityAtPoint(particles[i].position);
        }
    }

    private float PressureForceMultiplier(float density) {
        return pressureStiffness * (density - restDensity);
    }

        private float CosineKernel(float distance) {
        if(distance < smoothingRadius) {
            return (float)Cos(PI*distance/smoothingRadius)+1.0f;
        } else {
            return 0.0f;
        }
    }

    private float CosineKernelGradient(float distance) {
        if(distance < smoothingRadius) {
            return (float)((-PI / smoothingRadius) * Sin(PI * distance / smoothingRadius));
        } else {
            return 0.0f;
        }
    }

    private float CubicSplineSmoothingKernel(float distance) {
        float q = distance / smoothingRadius;
        if (q < 1.0f) {
            return (float)(((2.0f / 3.0f) - Pow(q, 2) + 0.5f * Pow(q, 3)) 
                * (3.0f/(2.0f*PI*Pow(smoothingRadius, 3))));
        } else if (q < 2.0f) {
            return (float)((Pow(2.0f - q, 3) / 6.0f) 
                * (3.0f/(2.0f*PI*Pow(smoothingRadius, 3))));
        } else {
            return 0.0f;
        }
    }

    private float CubicSplineSmoothingKernelGradient(float distance) {
        float q = distance / smoothingRadius;
        if (q < 1.0f) {
            return (float)(((3.0f * smoothingRadius * smoothingRadius - 4.0f *smoothingRadius * distance)
                / (2.0f * Pow(smoothingRadius, 3))) 
                * (3.0f/(2.0f*PI*Pow(smoothingRadius, 3))));
        } else if (q < 2.0f) {
            return (float)(((-4.0f * smoothingRadius * smoothingRadius + 4.0f * smoothingRadius * distance - distance * distance)
            / (2.0f * Pow(smoothingRadius, 3)))
            * (3.0f/(2.0f*PI*Pow(smoothingRadius, 3))));
        } else {
            return 0.0f;
        }
    }

    private float DebrunsKernel(float distance) {
        if (distance < smoothingRadius) {
            return (float)((15.0f /(PI * Pow(smoothingRadius, 6)))
                    * Pow((smoothingRadius - distance), 3));
        } else {
            return 0.0f;
        }
    }

    private float DebrunsKernelGradient(float distance) {
        if (distance < smoothingRadius) {
            return (float)((-45.0f /(PI * Pow(smoothingRadius, 6)))
                    * Pow((smoothingRadius - distance), 2));
        } else {
            return 0.0f;
        }
    }
}
