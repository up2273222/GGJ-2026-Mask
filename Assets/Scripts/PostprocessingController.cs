using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostprocessingController : MonoBehaviour
{
 public Shader postProcessShader;
 Material postProcessMaterial;
 
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
   src.format  //Format
   );
   
   Graphics.Blit(src, renderTexture, postProcessMaterial,0);
   Graphics.Blit(src, renderTexture, postProcessMaterial,1);
   Graphics.Blit(renderTexture, dest);
   
   RenderTexture.ReleaseTemporary(renderTexture);
 }
 
}
