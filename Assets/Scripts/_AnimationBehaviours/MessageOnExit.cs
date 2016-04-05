using UnityEngine;
using System.Collections;

public class MessageOnExit : StateMachineBehaviour {

	public string messageName = "AnimationExit";
	public bool requireReceiver;
	public Option sendTo = Option.selfOnly;

	private SendMessageOptions _require { get { return requireReceiver ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver; } }
	
	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (sendTo == Option.selfOnly)
			animator.SendMessage(messageName, _require);
		else if (sendTo == Option.selfAndParent) {
			animator.SendMessage(messageName, _require);
			if (animator.transform.parent)
				animator.transform.parent.SendMessage(messageName, _require);
		} else if (sendTo == Option.parentOnly) {
			if (animator.transform.parent)
				animator.transform.parent.SendMessage(messageName, _require);
		} else if (sendTo == Option.upwards)
			animator.SendMessageUpwards(messageName, _require);
	}

	public enum Option {
		selfOnly, selfAndParent, parentOnly, upwards
	}
}
