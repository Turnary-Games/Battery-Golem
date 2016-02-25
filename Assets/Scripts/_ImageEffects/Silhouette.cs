using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Silhouette : MonoBehaviour {

	public Color silhouetteColor = new Color(1,1,1,0.5f);
	private Material material;
	RenderTexture buffer;

	void Awake() {
		material = new Material(Shader.Find("Hidden/SilhouetteImageEffect"));
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		material.SetColor("_Silhouette", silhouetteColor);

		if (buffer == null || buffer.width != source.width || buffer.height != source.height) {
			buffer = RenderTexture.GetTemporary(source.width, source.height, 0);
		}

		Graphics.SetRenderTarget(buffer.colorBuffer, source.depthBuffer);
		Graphics.Blit(source, material);
		Graphics.Blit(buffer, destination);
		//Graphics.Blit(source, destination, material);
	}

}
