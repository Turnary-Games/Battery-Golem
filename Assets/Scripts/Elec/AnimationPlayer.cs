using UnityEngine;
using System.Collections;

public class AnimationPlayer : MonoBehaviour {

    public Animator anim;
    public string parameter = "Speed";
    public float fullSpeedAfter;

    private bool active;
    private float speed = 0;

    void Update()
    {
        if (fullSpeedAfter <= 0) speed = active ? 1 : 0;
        else speed = Mathf.MoveTowards(speed, active ? 1 : 0, Time.deltaTime / fullSpeedAfter);

        anim.SetFloat(parameter, speed);
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
