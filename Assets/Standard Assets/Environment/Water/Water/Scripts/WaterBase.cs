using UnityEngine;

public enum WaterQuality {
		High = 2,
		Medium = 1,
		Low = 0,
}

[ExecuteInEditMode]
public class WaterBase : MonoBehaviour 
{
	public Material sharedMaterial;
	public WaterQuality waterQuality = WaterQuality.High;
	public bool edgeBlend = true;
    public float BumpFlowSpeed = 0.1f;
    public float FoamFlowSpeed = 0.1f;

    private float _smallBumpPhase0 = 0;
    private float _smallBumpPhase1 = 0.5f;
    private float _largeBumpPhase0 = 0;
    private float _largeBumpPhase1 = 0.5f;
    private float _smallFoamPhase0 = 0;
    private float _smallFoamPhase1 = 0.5f;
    private float _largeFoamPhase0 = 0;
    private float _largeFoamPhase1 = 0.5f;

    private float Wrap(float x, float min, float max)
    {
        return (((x - min) % (max - min)) + (max - min)) % (max - min) + min;
    }

	public void UpdateShader() 
	{
        Vector4 tiling = sharedMaterial.GetVector("_Tiling");
        float bumpTilingRatio = tiling.x / tiling.y;
        float foamTilingRatio = tiling.z / tiling.w;

        float bumpDelta = Time.deltaTime * BumpFlowSpeed;
        _smallBumpPhase0 = Wrap(_smallBumpPhase0 + bumpDelta, 0, 1);
        _smallBumpPhase1 = Wrap(_smallBumpPhase1 + bumpDelta, 0, 1);
        _largeBumpPhase0 = Wrap(_largeBumpPhase0 + bumpDelta * bumpTilingRatio, 0, 1);
        _largeBumpPhase1 = Wrap(_largeBumpPhase1 + bumpDelta * bumpTilingRatio, 0, 1);

        float foamDelta = Time.deltaTime * FoamFlowSpeed;
        _smallFoamPhase0 = Wrap(_smallFoamPhase0 + foamDelta, 0, 1);
        _smallFoamPhase1 = Wrap(_smallFoamPhase1 + foamDelta, 0, 1);
        _largeFoamPhase0 = Wrap(_largeFoamPhase0 + foamDelta * foamTilingRatio, 0, 1);
        _largeFoamPhase1 = Wrap(_largeFoamPhase1 + foamDelta * foamTilingRatio, 0, 1);

	    sharedMaterial.SetVector("_SmallFlowPhases", new Vector4(_smallBumpPhase0, _smallBumpPhase1, _smallFoamPhase0, _smallFoamPhase1));
        sharedMaterial.SetVector("_LargeFlowPhases", new Vector4(_largeBumpPhase0, _largeBumpPhase1, _largeFoamPhase0, _largeFoamPhase1));

		if(waterQuality > WaterQuality.Medium)
			sharedMaterial.shader.maximumLOD = 501;
		else if(waterQuality> WaterQuality.Low)
			sharedMaterial.shader.maximumLOD = 301;
		else 
			sharedMaterial.shader.maximumLOD = 201;	
		
		// If the system does not support depth textures (ie. NaCl), turn off edge bleeding, 
		// as the shader will render everything as transparent if the depth texture is not valid.
		if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			edgeBlend = false;

		if(edgeBlend) 
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_ON");
			Shader.DisableKeyword("WATER_EDGEBLEND_OFF");		
			// just to make sure (some peeps might forget to add a water tile to the patches)
			if (Camera.main)
				Camera.main.depthTextureMode |= DepthTextureMode.Depth;		
		} 
		else 
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
			Shader.DisableKeyword("WATER_EDGEBLEND_ON");	
		}
	}
	
	public void WaterTileBeingRendered (Transform tr, Camera currentCam) 
	{
		if (currentCam && edgeBlend)
			currentCam.depthTextureMode |= DepthTextureMode.Depth;	
	}
	
	public void Update () 
	{				
		if(sharedMaterial)		
			UpdateShader();
	}	
}