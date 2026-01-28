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

 public float vignetteFeather;

 private float currentVignetteRadius; 
 private float currentTemperature;
 private float currentSaturationMult;
 private float currentFogStart, currentFogEnd;

 [Header("Tragedy vals")] 
 public float tragedyFogStart;
 public float tragedyFogEnd;
 public float tragedyTemperature;
 public float tragedySaturationMult;
 public float tragedyRadius;

 [Header("Comedy vals")] 
 public float comedyFogStart;
 public float comedyFogEnd;
 public float comedyTemperature;
 public float comedySaturationMult;
 public float comedyRadius;
 
 
 
 public WorldState currentState;

 private void OnEnable()
 {
     LevelSwapManager.SwapCheckOutcomeSuccessful += SwitchPostProcessState;
     
 }

 private void OnDisable()
 {
     LevelSwapManager.SwapCheckOutcomeSuccessful -= SwitchPostProcessState;
 }


 private void Awake()
 {
     currentState = WorldState.Tragedy;
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
   
   postProcessMaterial.SetFloat("_vignetteRadius", currentVignetteRadius);
   postProcessMaterial.SetFloat("_vignetteFeather", vignetteFeather);
   postProcessMaterial.SetFloat("_temperature", currentTemperature);
   postProcessMaterial.SetFloat("_saturationMult", currentSaturationMult);
   
   fogMaterial.SetFloat("_fogStart", currentFogStart);
   fogMaterial.SetFloat("_fogEnd", currentFogEnd);
   
   
   
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


 private void SwitchPostProcessState(WorldState state)
 {
     switch (state)
     {
         case WorldState.Tragedy:
         {
             //Change to tragedy
             StartCoroutine(ChangeToTragedy(1));
             currentState = WorldState.Tragedy;
             break;
         }
         case WorldState.Comedy:
         {
             StartCoroutine(ChangeToComedy(1));
             currentState = WorldState.Comedy;
             break;
         }
     }
     
 }
 
 private IEnumerator ChangeToTragedy(float duration)
 {
     float timeElapsed = 0f;
     float initialFogEnd = currentFogEnd;
     
     while (timeElapsed < duration)
     {
         timeElapsed += Time.deltaTime;
         float t = timeElapsed / duration;
         currentFogEnd = Mathf.Lerp(initialFogEnd, 0, t);
         yield return null;
     }
     currentFogEnd = 0;
     
     yield return new WaitForSeconds(0.5f);
     //move player
     currentTemperature = tragedyTemperature;
     currentTemperature = tragedyTemperature;
     currentSaturationMult = tragedySaturationMult;
     yield return new WaitForSeconds(0.5f);
     
     timeElapsed = 0f;
     
     while (timeElapsed < duration)
     {
         timeElapsed += Time.deltaTime;
         float t = timeElapsed / duration;
         currentFogEnd = Mathf.Lerp(0, tragedyFogEnd, t);
         currentVignetteRadius = Mathf.Lerp(comedyRadius,tragedyRadius, t);
         yield return null;
     }
     currentFogEnd = tragedyFogEnd;
 }

 private IEnumerator ChangeToComedy(float duration)
 {
     float timeElapsed = 0f;
     float initialFogEnd = currentFogEnd;
     
     while (timeElapsed < duration)
     {
         timeElapsed += Time.deltaTime;
         float t = timeElapsed / duration;
         currentFogEnd = Mathf.Lerp(initialFogEnd, 0, t);
         yield return null;
     }
     currentFogEnd = 0;
     
     yield return new WaitForSeconds(0.5f);
     //move player
     currentTemperature = tragedyTemperature;
     currentTemperature = comedyTemperature;
     currentSaturationMult = comedySaturationMult;
     yield return new WaitForSeconds(0.5f);
     
     timeElapsed = 0f;
     
     while (timeElapsed < duration)
     {
         timeElapsed += Time.deltaTime;
         float t = timeElapsed / duration;
         currentFogEnd = Mathf.Lerp(0, comedyFogEnd, t);
         currentVignetteRadius = Mathf.Lerp(tragedyRadius, comedyRadius, t);
         yield return null;
     }
     currentFogEnd = comedyFogEnd;
 }
 
}
