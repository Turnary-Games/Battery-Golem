using UnityEngine;
using System.Collections;

public class AnimationPlayer : MonoBehaviour {

    public Animator anim;
    public string parameter = "Speed";
    public float fullSpeedAfter;
	public float speedMultiplier = 1;

    private bool active;
	[SerializeThis]
    private float speed = 0;

#if UNITY_EDITOR
	void OnValidate() {
		speedMultiplier = Mathf.Max(speedMultiplier, 0);
		fullSpeedAfter = Mathf.Max(fullSpeedAfter, 0);
	}
#endif

	void Update() {
        if (fullSpeedAfter <= 0) speed = active ? 1 : 0;
        else speed = Mathf.MoveTowards(speed, active ? 1 : 0, Time.deltaTime / fullSpeedAfter);

        anim.SetFloat(parameter, speed * speedMultiplier);
    }

    void OnElectrifyStart()
    {
        active = true;
    }

    void OnElectrifyEnd()
    {
        active = false;
    }

}
