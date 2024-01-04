using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Donuts
{
    public partial class GameEvent
    {
        public Action<MeshRenderer> onUpdateInstancingDeltaTime;
        public Action<MeshRenderer[], int> onPlayInstancingAnimation;
    }

    public class InstancingAnimationSystem : AGameSystem
    {

        private static int shaderDeltaTimeHash = Shader.PropertyToID("_DT");
        private MaterialPropertyBlock materialBlock;
        public InstancingAnimationSystem()
        {
            materialBlock = new MaterialPropertyBlock();
            
        }

        public override void SetupEvents()
        {
            gameEvent.onUpdateInstancingDeltaTime += OnUpdateInstancingAnimation;
            gameEvent.onPlayInstancingAnimation += OnPlayInstancingAnimation;
        }

        private void OnUpdateInstancingAnimation(MeshRenderer mesh)
        {
            mesh.GetPropertyBlock(materialBlock);
            materialBlock.SetFloat(shaderDeltaTimeHash, Time.timeSinceLevelLoad);
            mesh.SetPropertyBlock(materialBlock);
        }

        private void OnPlayInstancingAnimation(MeshRenderer[] meshRenderers, int index)
        {
            int length = meshRenderers.Length;
            for (int i = 0; i < length; i++)
            {
                bool istarget = i == index;
                meshRenderers[i].gameObject.SetActive(istarget);
                if(istarget)
                {
                    OnUpdateInstancingAnimation(meshRenderers[i]);
                }
            }
        }
    }
}