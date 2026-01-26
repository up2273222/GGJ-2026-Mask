using System;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostprocessingController : MonoBehaviour
{
 public Shader postProcessShader;
 Material postProcessMaterial;

 public float vignetteRadius;
 public float vignetteFeather;
 public float temperature;

 
 void OnRenderImage(RenderTexture src, RenderTexture dest)
 {
     if (postProcessMaterial == null)
     {
         postProcessMaterial = new Material(postProcessShader);
     }
     
     RenderTexture renderTexture = RenderTexture.GetTemporary(
   src.width, //Width
   src.height,  //Height
   0,  //Depth buffer
   RenderTextureFormat.ARGBHalf  //Format
   );
   
   postProcessMaterial.SetFloat("_vignetteRadius", vignetteRadius);
   postProcessMaterial.SetFloat("_vignetteFeather", vignetteFeather);
   postProcessMaterial.SetFloat("_temperature",(float) temperature);
   //Graphics.Blit(src, renderTexture, postProcessMaterial, 0);
   //Graphics.Blit(renderTexture, dest, postProcessMaterial, 1);
   
   Graphics.Blit(src, renderTexture, postProcessMaterial, 0);
   Graphics.Blit(renderTexture, dest);
   
   RenderTexture.ReleaseTemporary(renderTexture);
 }
 
}
