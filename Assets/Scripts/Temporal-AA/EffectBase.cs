// Copyright (c) <2015> <Playdead>
// This file is subject to the MIT License as seen in the root of this folder structure (LICENSE.TXT)
// AUTHOR: Lasse Jon Fuglsang Pedersen <lasse@playdead.com>

using UnityEngine;

public abstract class EffectBase : MonoBehaviour
{
    public void EnsureMaterial(ref Material material, Shader shader)
    {
        if (shader != null)
        {
            if (material == null || material.shader != shader)
                material = new Material(shader);
            if (material != null)
                material.hideFlags = HideFlags.DontSave;
        }
        else
        {
            Debug.LogWarning("missing shader", this);
        }
    }

    public void EnsureKeyword(Material material, string name, bool enabled)
    {
        if (enabled != material.IsKeywordEnabled(name))
        {
            if (enabled)
                material.EnableKeyword(name);
            else
                material.DisableKeyword(name);
        }
    }

    public void EnsureRenderTarget(ref RenderTexture rt, int width, int height, RenderTextureFormat format, FilterMode filterMode, int depthBits = 0)
    {
        if (rt != null && (rt.width != width || rt.height != height || rt.format != format || rt.filterMode != filterMode))
        {
            RenderTexture.ReleaseTemporary(rt);
            rt = null;
        }
        if (rt == null)
        {
            rt = RenderTexture.GetTemporary(width, height, depthBits, format);
            rt.filterMode = filterMode;
            rt.wrapMode = TextureWrapMode.Clamp;
        }
    }

    public void FullScreenQuad()
    {
        GL.PushMatrix();
        GL.LoadOrtho();

        GL.Begin(GL.QUADS);
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 0.0f); // BL

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 0.0f); // BR

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 0.0f); // TR

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f); // TL

        GL.End();
        GL.PopMatrix();
    }
}
