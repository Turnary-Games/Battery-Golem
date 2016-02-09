using UnityEngine;
using System.Collections;

public class WaterDrop : MonoBehaviour {

    public GameObject particles;

    void OnTriggerEnter(Collider other)
    {
        particles.transform.SetParent(transform.parent);
        particles.SetActive(true);
        particles.transform.localScale = Vector3.one;
        Destroy(particles, 5f);

        Destroy(gameObject);
    }
}
