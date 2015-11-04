using UnityEngine;
using System.Collections;

namespace ExtensionMethods {

    public static class RigidbodyExtension {
        /// <summary>
        /// Disables/Reenables the rigidbody.
        /// Deacting will disable the gravity and collision detection, as well as resetting the linear and angular velocity.
        /// Reactivating will do the opposite, except will not affect the velocity.
        /// </summary>
        /// <param name="rbody"></param>
        /// <param name="state">True to activate, False to deactivate</param>
        public static void SetEnabled(this Rigidbody rbody, bool state) {
            rbody.useGravity = state;
            rbody.detectCollisions = state;
            if (state == false) {
                rbody.velocity = Vector3.zero;
                rbody.angularVelocity = Vector3.zero;
            }
        }

        public static bool IsEnabled(this Rigidbody rbody) {
            return rbody.detectCollisions && rbody.useGravity;
        }
    }

}
