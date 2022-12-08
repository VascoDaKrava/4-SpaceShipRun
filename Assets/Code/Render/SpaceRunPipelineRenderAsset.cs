using UnityEngine;
using UnityEngine.Rendering;

namespace SpaceShipRun.CustomRenderPipeline
{
    [CreateAssetMenu(menuName = "SpaceShipRun/Rendering/SpaceRunPipelineRenderAsset")]
    public sealed class SpaceRunPipelineRenderAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new SpaceRunPipelineRender();
        }
    }
}