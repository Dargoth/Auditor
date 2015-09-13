using UnityEngine;
using System.Collections;

public class MarkerScript : MonoBehaviour {

    public GameObject Circle;
    SpriteRenderer CircleSprite;
    public Color CircleColor;
    Color CurrCircleColor;
    public float radius = 1;
    public ParticleSystem Emitter;
    ParticleSystem.Particle[] Particles;
    public float VerticalDrift = 0.01f;
    public float HorizontalDrift = 0.01f;
    Color[] colors = { Color.green, Color.red, Color.yellow, Color.blue, Color.magenta };
    Color randColor;

    private void LateUpdate() {
        //Initializing particle vars
        InitializeIfNeeded();

        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int numParticlesAlive = Emitter.GetParticles(Particles);

        ParticleOverlapCheck(numParticlesAlive);
        
    }

    void OnMouseDrag() {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
        Vector3 markerPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = markerPos;
    }

    private void InitializeIfNeeded() {
        if (Emitter == null)
            Emitter = FindObjectOfType<ParticleSystem>();

        if (Particles == null || Particles.Length < Emitter.maxParticles)
            Particles = new ParticleSystem.Particle[Emitter.maxParticles];
    }

    private void ParticleOverlapCheck(int numParticlesAlive) {
        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++) {
            //...and in marker radius
            float d = Mathf.Sqrt(Mathf.Pow(transform.position.x - Particles[i].position.x, 2)
                                + Mathf.Pow(transform.position.y - Particles[i].position.y, 2));
            if (d > radius) continue;

            Particles[i].velocity += Vector3.up * VerticalDrift;
            Particles[i].velocity += Vector3.right * HorizontalDrift;
            randColor = colors[Random.Range(0, 5)];
            Particles[i].color = randColor;
            Particles[i].lifetime += .5f;
            
            CircleSprite.color = CircleColor;
        }
        Emitter.SetParticles(Particles, numParticlesAlive);
    }

    // Use this for initialization
    void Start () {
        CircleSprite = Circle.GetComponent<SpriteRenderer>();
        CircleSprite.color = CircleColor;
    }
	
	// Update is called once per frame
	void Update () {
        if (CircleSprite.color.a > .2) {
            CurrCircleColor = CircleSprite.color;
            CurrCircleColor.a -= .01f;
            CircleSprite.color = CurrCircleColor;
        }
	}
}
