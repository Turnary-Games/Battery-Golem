using UnityEngine;
using System.Collections;

public class PlayerAnimation : PlayerSubClass {

	public Animator ac;
	
	void LateUpdate() {
		if (pushing.movement.pushing.anim.pushing.interaction.pushing.hud.inventory.pushing.point) {
			ac.SetBool("ArmsUp", true);
			ac.SetBool("CoreItem", true);
		} else if (interaction.electrifying) {
			ac.SetBool("ArmsUp", true);
			ac.SetBool("CoreItem", true);
		} else {
			ac.SetBool("ArmsUp", inventory.equipped);
			ac.SetBool("CoreItem", inventory.equipped && inventory.equipped.isCore);
		}


		// Velocity, relative to the current platform (if any)
		Vector3 motion = movement.platform ? movement.body.velocity - movement.platform.body.velocity : movement.body.velocity;
		float magn = new Vector2(motion.x, motion.z).magnitude;

		// Tell animator
		ac.SetBool("Walking", magn > 0);
		ac.SetFloat("MoveSpeed", magn > 0 ? magn / 5 : 1);
		ac.SetBool("Grounded", movement.grounded);
		ac.SetFloat("VertSpeed", motion.y);
	}
}
