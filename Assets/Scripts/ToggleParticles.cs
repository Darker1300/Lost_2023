using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleParticles : MonoBehaviour
{
    [Tooltip("Particle System should be set to Looping: true, and Play On Awake: false.")]
    public ParticleSystem ParticleSystem = null;

    private void Start()
    {
        ParticleSystem = ParticleSystem != null ? ParticleSystem
            : GetComponent<ParticleSystem>();
    }

    public void Toggle()
    {
        ParticleSystem.EmissionModule emission = ParticleSystem.emission;
        emission.enabled = !emission.enabled;
        if (emission.enabled && !ParticleSystem.isPlaying)
            ParticleSystem.Play();
    }

    public void Play()
    {
        ParticleSystem.EmissionModule emission = ParticleSystem.emission;
        emission.enabled = true;
        if (!ParticleSystem.isPlaying)
            ParticleSystem.Play();
    }
    
    public void Pause()
    {
        ParticleSystem.EmissionModule emission = ParticleSystem.emission;
        emission.enabled = false;
    }
}
