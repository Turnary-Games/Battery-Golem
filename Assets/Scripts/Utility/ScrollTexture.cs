using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour {

	public MeshRenderer mesh;
	public Vector2 motion;

	void Update() {
		Material mat = mesh.material;
		Vector2 offset = mat.mainTextureOffset;

		offset += motion * Time.deltaTime;
		offset.x %= 1;
		offset.y %= 1;

		mat.mainTextureOffset = offset;
	}

}
