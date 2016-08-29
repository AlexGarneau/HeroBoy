using UnityEngine;
using System.Collections;

public class DustParticleEffect : MonoBehaviour {

    public GameObject[] dustParticles;
    public int minParticleCount = 3;
    public int maxParticleCount = 6;

    public float minLife = 4f;
    public float maxLife = 6f;
    public float minVel = .1f;
    public float maxVel = .2f;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float minRotSpeed = -1f;
    public float maxRotSpeed = 1f;

    [Range(-1, 1)]
    public int direction = -1;

    protected ArrayList particles;
    protected int particleCount = 0;

	// Use this for initialization
	void Start () {
        // Create the particles and their respective stats. This includes velocity, rotation, etc.
        particleCount = Random.Range(minParticleCount, maxParticleCount);
        particles = new ArrayList(particleCount);
        DustParticle particle;
        for (int i = particleCount - 1; i >= 0; i--) {
            particle = new DustParticle();
            particle.obj = Instantiate(dustParticles[Random.Range(0, dustParticles.Length)], transform.position, transform.rotation) as GameObject;
            particle.obj.transform.parent = transform;
            particle.renderer = particle.obj.GetComponent<SpriteRenderer>();

            particle.velocity = Random.Range(minVel, maxVel);
            particle.angle = Random.Range(minAngle, maxAngle) * Mathf.PI / 180;
            particle.life = particle.lifeMax = Random.Range(minLife, maxLife);
            particle.rotationSpeed = Random.Range(minRotSpeed, maxRotSpeed);
            
            particles.Add(particle);
        }
	}
	
	// Update is called once per frame
	void Update () {
        DustParticle particle;
        Transform particleTF;
	    for (int i = particles.Count - 1; i >= 0; i--) {
            particle = particles[i] as DustParticle;
            particleTF = particle.obj.transform;
            particleTF.Translate(Mathf.Cos(particle.angle) * particle.velocity * direction, Mathf.Sin(particle.angle) * particle.velocity, 0, transform);
            particleTF.Rotate(Vector3.forward, particle.rotationSpeed);
            particle.renderer.color = new Color(1f, 1f, 1f, particle.life / particle.lifeMax);
            particle.life -= Time.deltaTime;
            if (particle.life <= 0) {
                Destroy(particle.obj);
                particles.RemoveAt(i);
            }
        }
        if (particles.Count == 0) {
            Destroy(this.gameObject);
        }
	}
}

class DustParticle {
    public float velocity = 1f;
    public float angle = 0f;
    public float rotationSpeed = 0f;
    public float life = 10f;
    public float lifeMax = 10f;
    public GameObject obj;
    public SpriteRenderer renderer;
}