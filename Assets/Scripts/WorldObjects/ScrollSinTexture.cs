using UnityEngine;
using System.Collections;

public class ScrollSinTexture : MonoBehaviour
{

    public MeshRenderer mesh;
    public Vector2 period = Vector2.one;
    public Vector2 amp = Vector2.one;

    void OnValidate()
    {
        period.x = Mathf.Clamp(period.x, 0.0001f, 1000000f);
        period.y = Mathf.Clamp(period.y, 0.0001f, 1000000f);

        amp.x = Mathf.Clamp(amp.x, -1, 1);
        amp.y = Mathf.Clamp(amp.y, -1, 1);
    }

    void Update()
    {
        Material mat = mesh.material;
        Vector2 offset = mat.mainTextureOffset;

        Vector2 B = new Vector2(2 * Mathf.PI / period.x, 2 * Mathf.PI / period.y);

        offset.x = Mathf.Sin(Time.time * B.x) * amp.x;
        offset.y = Mathf.Sin(Time.time * B.y) * amp.y;

        mat.mainTextureOffset = offset;
    }

}
