using UnityEngine;
using System.Collections;

public class WaterDrop : MonoBehaviour {

    public ParticleSystem particles;

    void OnTriggerEnter(Collider other)
    {
        particles.transform.SetParent(transform.parent);
        particles.transform.localScale = Vector3.one;
		particles.Play();
		Destroy(particles.gameObject, 5f);

        Destroy(gameObject, 0.1f);
    }
}
