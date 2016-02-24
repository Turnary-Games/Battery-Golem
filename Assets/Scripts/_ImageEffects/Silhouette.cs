using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Silhouette : MonoBehaviour {

	public Color silhouetteColor = new Color(1,1,1,0.5f);
	private Material material;

	void Awake() {
		material = new Material(Shader.Find("Hidden/SilhouetteImageEffect"));
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		material.SetColor("_Silhouette", silhouetteColor);
		Graphics.Blit(source, destination, material);
	}

}
