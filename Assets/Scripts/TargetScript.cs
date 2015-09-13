using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {

    public int MaxLoad = 50;
    public int CurrentLoad = 0;
    public float radius = 1;
    float loading = 0;

    SpriteRenderer SelfSprite;
    Color CurrSpriteColor;
    public ParticleSystem Emitter;
    ParticleSystem.Particle[] Particles;

    // Use this for initialization
    void Start () {
        SelfSprite = GetComponent<SpriteRenderer>();
        CurrSpriteColor = SelfSprite.color;
    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentLoad > 0) {
            CurrentLoad--;
        } 
	}

    private void LateUpdate() {
        //Initializing particle vars
        InitializeIfNeeded();
        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int numParticlesAlive = Emitter.GetParticles(Particles);
        ParticleOverlapCheck(numParticlesAlive);
        loading = ((float)CurrentLoad) / ((float)MaxLoad);
        CurrSpriteColor.a = loading + .2f;
        SelfSprite.color = CurrSpriteColor;
        GetComponent<AudioSource>().volume = loading;
    }

    private void ParticleOverlapCheck(int numParticlesAlive) {
        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++) {
            //...and in marker radius
            float d = Mathf.Sqrt(Mathf.Pow(transform.position.x - Particles[i].position.x, 2)
                                + Mathf.Pow(transform.position.y - Particles[i].position.y, 2));
            if (d > radius) continue;
            if (CurrentLoad < MaxLoad) {
                CurrentLoad++;
            }
        }
        Emitter.SetParticles(Particles, numParticlesAlive);
    }

    private void InitializeIfNeeded() {
        if (Emitter == null)
            Emitter = FindObjectOfType<ParticleSystem>();

        if (Particles == null || Particles.Length < Emitter.maxParticles)
            Particles = new ParticleSystem.Particle[Emitter.maxParticles];
    }

}
