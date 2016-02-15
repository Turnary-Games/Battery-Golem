using UnityEngine;
using System.Collections;

public class MessageOnEnter : StateMachineBehaviour {

	public string messageName = "AnimationStart";
	public bool requireReceiver;
	public Option sendTo = Option.selfOnly;

	private SendMessageOptions _require { get { return requireReceiver ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver; } }

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
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
