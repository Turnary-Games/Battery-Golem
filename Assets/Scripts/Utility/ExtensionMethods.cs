using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public static class ListExtension {

		/// <summary>
		/// Get the index of the first empty slot.
		/// If the value is null the slot is counted as empty.
		/// If no size is declared it will use the /list.Length/ value.
		/// </summary>
		public static int EmptySlot<Item>(this Item[] list) where Item : _Item {
			return list.EmptySlot(list.Length);
		}

		/// <summary>
		/// Get the index of the first empty slot.
		/// If the value is null the slot is counted as empty.
		/// If no size is declared it will use the /list.Length/ value.
		/// </summary>
		/// <param name="size">Max size of the list</param>
		public static int EmptySlot<Item>(this Item[] list, int size) where Item : _Item {
			if (list == null || list.Length == 0)
				return -1;

			// Basic algorithm to find an empty slot
			for (int i = 0; i < size; i++) {
				if (list[i] == null) {
					// Empty slot
					return i;
				}
			}

			// List is full
			return -1;
		}

		public static int IndexOf<Item>(this Item[] list, Item item) where Item : _Item {
			for (int index = 0; index < list.Length; index++) {
				if ((item==null && list[index]==null) || (item!=null && item.Equals(list[index]))) {
					return index;
				} 
			}
			return -1;
		}

		public static string ToFancyString<T>(this T[] list) {
			string s = "";
			for (int index = 0; index < list.Length; index++) {
				s += "[" + index + "]=" + (list[index] == null ? "null" : list[index].ToString()) + ";";
			}

			return list.ToString() + "{ " + s + " }";
		}

		public static T Get<T>(this T[] list, int index) where T : Object {
			return list.Length > index && index >= 0 ? list[index] : null;
		}
	}

	public static class ParticleExtensions {
		public static void Play(this ParticleSystem[] list) {
			foreach(var system in list) {
				system.Play();
			}
		}

		public static void Stop(this ParticleSystem[] list) {
			foreach (var system in list) {
				system.Stop();
			}
		}

		public static void Pause(this ParticleSystem[] list) {
			foreach (var system in list) {
				system.Pause();
			}
		}
	}


	public static class EffectItemExtension {

		// Array extension
		public static void OnPickup(this PickupAction[] array) {
			foreach (PickupAction effect in array) {
				if (effect != null)
					effect.OnPickup();
			}
		}

		public static void OnDrop(this PickupAction[] array) {
			foreach (PickupAction effect in array) {
				if (effect != null)
					effect.OnDrop();
			}
		}

		// List extension
		public static void OnPickup(this List<PickupAction> list) {
			foreach (PickupAction effect in list) {
				if (effect != null)
					effect.OnPickup();
			}
		}

		public static void OnDrop(this List<PickupAction> list) {
			foreach (PickupAction effect in list) {
				if (effect != null)
					effect.OnDrop();
			}
		}
	}

	public static class VectorExtensions {
		/// <summary>
		/// Converts a vector3 to a vector2. The Z value gets lost.
		/// </summary>
		public static Vector2 ToVector2(this Vector3 vector) {
			return new Vector2(vector.x, vector.y);
		}

		/// <summary>
		/// Converts a vector2 to  a vector3. The Z value gets set to 0.
		/// </summary>
		public static Vector3 ToVector3(this Vector2 vector) {
			return new Vector3(vector.x, vector.y);
		}
	}
}
