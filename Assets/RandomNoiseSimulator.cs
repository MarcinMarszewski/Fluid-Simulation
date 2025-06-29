using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RandomNoiseSimulator : AbstractParticleSimulator {

    override public void SimulateStep() {
        /* for (int i = 0; i < particles.Length; i++) {
            particles[i].position += particles[i].velocity * Time.deltaTime;
            particles[i].velocity += Random.insideUnitSphere * Time.deltaTime;
        } */
    }
}