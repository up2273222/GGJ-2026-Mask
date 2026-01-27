using System;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostprocessingController : MonoBehaviour
{
 public Shader postProcessShader;
 public Shader fogShader;
 Material postProcessMaterial;
 Material fogMaterial;

 private new Camera camera;

 private Vector3[] frustumCorners;
 private Vector4[] vectorArray;

 public float vignetteRadius;
 public float vignetteFeather;
 public float temperature;
 public float saturationMult;
 public float fogStart, fogEnd;

 public Transform cameraTransform;

 private void Awake()
 {
     
 }


[ImageEffectOpaque]
 void OnRenderImage(RenderTexture src, RenderTexture dest)
 {
     if (postProcessMaterial == null)
     {
         postProcessMaterial = new Material(postProcessShader);
     }

     if (fogMaterial == null)
     {
         camera = GetComponent<Camera>();
         fogMaterial = new Material(fogShader);
     }
     
     
    

     
     RenderTexture renderTexture = RenderTexture.GetTemporary(
   src.width, //Width
   src.height,  //Height
   16,  //Depth buffer
   RenderTextureFormat.ARGBHalf  //Format
   );
   
     RenderTexture renderTexture2 = RenderTexture.GetTemporary(
         src.width, //Width
         src.height,  //Height
         16,  //Depth buffer
         RenderTextureFormat.ARGBHalf  //Format
     );
     
     RenderTexture renderTexture3 = RenderTexture.GetTemporary(
         src.width, //Width
         src.height,  //Height
         16,  //Depth buffer
         RenderTextureFormat.ARGBHalf  //Format
     );
   
   postProcessMaterial.SetFloat("_vignetteRadius", vignetteRadius);
   postProcessMaterial.SetFloat("_vignetteFeather", vignetteFeather);
   postProcessMaterial.SetFloat("_temperature", temperature);
   postProcessMaterial.SetFloat("_saturationMult", saturationMult);
   
   fogMaterial.SetFloat("_fogStart", fogStart);
   fogMaterial.SetFloat("_fogEnd", fogEnd);
   fogMaterial.SetVector("_camPos", cameraTransform.position);
   
   
   
   Graphics.Blit(src, renderTexture, postProcessMaterial, 0);
   Graphics.Blit(renderTexture, renderTexture2, postProcessMaterial, 1);
   Graphics.Blit(renderTexture2, renderTexture3, fogMaterial);
   Graphics.Blit(renderTexture3, dest, postProcessMaterial, 2);
   
   
   
   
   //Graphics.Blit(src, dest, fogMaterial, 0);
   //Graphics.Blit(renderTexture, dest);
   
   
   RenderTexture.ReleaseTemporary(renderTexture);
   RenderTexture.ReleaseTemporary(renderTexture2);
   RenderTexture.ReleaseTemporary(renderTexture3);
 }
 
}
