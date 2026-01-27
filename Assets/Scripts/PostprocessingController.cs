using System;
using System.Collections;
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

 
 

 private bool isFading;

 public WorldState currentState;
 
 

 public Transform cameraTransform;


 private void Awake()
 {
     
 }

 private void Update()
 {
     if(Input.GetKeyDown(KeyCode.E))
     {
         StartCoroutine(ChangeValue(fogEnd,0,1,v => fogEnd = v));
         
     }
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
         frustumCorners = new Vector3[4];
         vectorArray = new Vector4[4];
         fogMaterial = new Material(fogShader);
     }

     camera.CalculateFrustumCorners(
         new Rect(0f, 0f, 1f, 1f),
         camera.farClipPlane,
         camera.stereoActiveEye,
         frustumCorners
     );
     
     vectorArray[0] = frustumCorners[0];
     vectorArray[1] = frustumCorners[3];
     vectorArray[2] = frustumCorners[1];
     vectorArray[3] = frustumCorners[2];
     fogMaterial.SetVectorArray("_FrustumCorners", vectorArray);
     
     
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
 
 private IEnumerator ChangeValue(float start, float end, float duration, Action<float> value)
 {
     float timeElapsed = 0f;

     while (timeElapsed < duration)
     {
         timeElapsed += Time.deltaTime;
         float t = timeElapsed / duration;
         value(Mathf.Lerp(start, end, t));
         yield return null;
     }
     value(end);
     
     yield return new WaitForSeconds(1);
     
     timeElapsed = 0f;
     
     while (timeElapsed < duration)
     {
         timeElapsed += Time.deltaTime;
         float t = timeElapsed / duration;
         value(Mathf.Lerp(end, start, t));
         yield return null;
     }
     value(start);

    
 }
 
}
