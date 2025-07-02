using UnityEngine;

public class SPHShaderController : MonoBehaviour
{
    public ComputeShader fluidShader;
    public ComputeBuffer particleBuffer;
    public float timeStep;
    public float gravityConstant;
    public float boundarySize;
    public float boundaryDampingFactor;
    public float particleMass;
    public float smoothingRadius;
    public float pressureConstant;
    public float nearPressureConstant;
    public float viscosityConstant;
    public float restDensity;
    public int particleGridSize;
    public float spawnSpacingMultiplier;

    struct Particle
    {
        public Vector2 position;
        public Vector2 predictedPosition;
        public Vector2 velocity;
        public float density;
        public float nearDensity;
    }

    Particle[] particles;

    void Start()
    {
        particleBuffer = new ComputeBuffer(particleGridSize*particleGridSize, sizeof(float) * 8);
        particles = new Particle[particleGridSize * particleGridSize];
        for (int i = 0; i < particleGridSize; i++)
        {
            for(int j = 0; j < particleGridSize; j++)
            {
                particles[i * particleGridSize + j].position = new Vector2(i * spawnSpacingMultiplier/restDensity, j * spawnSpacingMultiplier/restDensity);
                particles[i * particleGridSize + j].predictedPosition = Vector2.zero;
                particles[i * particleGridSize + j].velocity = Vector2.zero;
                particles[i * particleGridSize + j].density = 0.0f;
                particles[i * particleGridSize + j].nearDensity = 0.0f;
            }
        }
        particleBuffer.SetData(particles);
    }

void Update()
    {
        int threadGroups = Mathf.CeilToInt(particleGridSize*particleGridSize / 64.0f);

        // Set parameters for all kernels
        SetShaderParams();
        int kernel;
        // Resolve collisions
        kernel = fluidShader.FindKernel("ResolveCollision");
        fluidShader.SetBuffer(kernel, "particles", particleBuffer);
        fluidShader.Dispatch(kernel, threadGroups, 1, 1);

        // Predict positions
        kernel = fluidShader.FindKernel("PredictPosition");
        fluidShader.SetBuffer(kernel, "particles", particleBuffer);
        fluidShader.Dispatch(kernel, threadGroups, 1, 1);

        // Compute densities
        kernel = fluidShader.FindKernel("ComputeDensity");
        fluidShader.SetBuffer(kernel, "particles", particleBuffer);
        fluidShader.Dispatch(kernel, threadGroups, 1, 1);

        // Compute pressure forces
        kernel = fluidShader.FindKernel("ComputePressureForce");
        fluidShader.Dispatch(kernel, threadGroups, 1, 1);
        fluidShader.SetBuffer(kernel, "particles", particleBuffer);

        // Apply external forces (gravity)
        kernel = fluidShader.FindKernel("ApplyExternalForce");
        fluidShader.SetBuffer(kernel, "particles", particleBuffer);
        fluidShader.Dispatch(kernel, threadGroups, 1, 1);

        // Apply viscosity
        kernel = fluidShader.FindKernel("ComputeViscosityForce");
        fluidShader.SetBuffer(kernel, "particles", particleBuffer);
        fluidShader.Dispatch(kernel, threadGroups, 1, 1);

        // Update positions
        kernel = fluidShader.FindKernel("UpdatePosition");
        fluidShader.SetBuffer(kernel, "particles", particleBuffer);
        fluidShader.Dispatch(kernel, threadGroups, 1, 1);

    }

    void SetShaderParams()
    {
        fluidShader.SetFloat("timeStep", timeStep);
        fluidShader.SetFloat("gravityConstant", gravityConstant);
        fluidShader.SetInt("particleCount", particleGridSize * particleGridSize);
        fluidShader.SetFloat("boundarySize", boundarySize);
        fluidShader.SetFloat("boundaryDampingFactor", boundaryDampingFactor);
        fluidShader.SetFloat("particleMass", particleMass);
        fluidShader.SetFloat("smoothingRadius", smoothingRadius);
        fluidShader.SetFloat("pressureConstant", pressureConstant);
        fluidShader.SetFloat("nearPressureConstant", nearPressureConstant);
        fluidShader.SetFloat("restDensity", restDensity);
        fluidShader.SetFloat("viscosityConstant", viscosityConstant);
    }

    void OnDrawGizmos()
    {
        if (particleBuffer != null)
        {
            particleBuffer.GetData(particles);
            Gizmos.color = Color.blue;
            foreach (var particle in particles)
            {
                Gizmos.DrawSphere(new Vector3(particle.position.x, particle.position.y, 0), 0.1f);
                Debug.Log("Density " + particle.density +  " particle position: " + particle.position);
            }
        }
    }

    void OnDestroy()
    {
        particleBuffer?.Release();
    }
}
