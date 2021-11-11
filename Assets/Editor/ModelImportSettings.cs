using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Timeline;

public class ModelImportSettings : AssetPostprocessor
{
	//void OnPreprocessAnimation()
	//{

	//	if (assetPath.Contains("Animations"))
	//	{
	//		ModelImporter modelImporter = assetImporter as ModelImporter;
	//		var animations = modelImporter.defaultClipAnimations;

	//		for (int i = 0; i < animations.Length; i++)
	//		{
	//			Debug.Log(animations[i].firstFrame);
	//			Debug.Log(animations[i].lastFrame);
	//		}

	//		modelImporter.clipAnimations = animations;

	//		Debug.Log("Imported animation with custom settings.");
	//	}
	//}


	//void OnPreprocessAnimation()
	//{
	//	ModelImporter modelImporter = assetImporter as ModelImporter;
	//	modelImporter.clipAnimations = modelImporter.defaultClipAnimations;
	//}

    private string SourceModelPath = "Assets/Character.fbx";
    void OnPreprocessAnimation()
    {
        if (assetPath.Contains("Animations"))
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
            var animations = modelImporter.defaultClipAnimations;

            ConfigureRig(modelImporter);
            ProcessAllAnimations(animations);
            ProcessLoopingAnimations(animations);
            ProcessOneTimeAnimations(animations);
            modelImporter.clipAnimations = animations;
            Debug.Log("Imported animation with custom settings.");
        }
    }
    private void ConfigureRig(ModelImporter model)
    {
        var fbxAsset = AssetDatabase.LoadAssetAtPath<GameObject>(SourceModelPath);
        model.animationType = ModelImporterAnimationType.Human;
        var avatar = fbxAsset.GetComponent<Animator>().avatar;
        model.sourceAvatar = avatar;
    }
    private void ProcessAllAnimations(ModelImporterClipAnimation[] animations)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].lockRootRotation = true;
            animations[i].lockRootHeightY = true;
            animations[i].keepOriginalOrientation = true;
            animations[i].keepOriginalPositionXZ = true;
            animations[i].keepOriginalPositionY = true;
        }
    }
    private void ProcessOneTimeAnimations(ModelImporterClipAnimation[] animations)
    {
        if (assetPath.Contains("OneTime"))
        {
            for (int i = 0; i < animations.Length; i++)
            {
                animations[i].loopTime = false;
                animations[i].loopPose = false;
            }
        }
    }
    private void ProcessLoopingAnimations(ModelImporterClipAnimation[] animations)
    {
        if (assetPath.Contains("Looping"))
        {
            for (int i = 0; i < animations.Length; i++)
            {
                animations[i].loopTime = true;
                animations[i].loopPose = true;
            }
        }
    }
}

