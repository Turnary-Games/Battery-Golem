using UnityEngine;
using System.Collections;

public class WaterDrop : MonoBehaviour {

    public ParticleSystem particles;

    void OnTriggerEnter(Collider other)
    {
        particles.transform.SetParent(transform.parent);
        particles.transform.localScale = Vector3.one;
		particles.Play();
		Destroy(particles.gameObject, particles.startLifetime);

        Destroy(gameObject, 0.1f);
    }
}
