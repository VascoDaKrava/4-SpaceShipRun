using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace SpaceShipRun.CustomRenderPipeline
{
    public sealed class CameraRenderer
    {
        private const string BUFFER_NAME = "Camera Render";
        private CommandBuffer _commandBuffer = new CommandBuffer { name = BUFFER_NAME };

        private ScriptableRenderContext _context;
        private Camera _camera;
        private CullingResults _cullingResult;

        private static readonly List<ShaderTagId> _drawingShaderTagIds =
            new List<ShaderTagId> {
                new ShaderTagId("SRPDefaultUnlit"),
                //new ShaderTagId("SRPDefaultLit"),
                //new ShaderTagId("SRPDefaultLit"),
                //new ShaderTagId("URPDefaultLit"),
                //new ShaderTagId("PlanetShader"),
                //new ShaderTagId("MyShaderBlaBlaUnlit"),
            };

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            _camera = camera;
            _context = context;
            //RenderGUI();
            if (!Cull(out var parameters))
            {
                return;
            }

            Settings(parameters);
            DrawVisible();
            DrawUnsupportedShaders();
            DrawGizmos();
            Submit();
        }

        private void RenderGUI()
        {
            if (_camera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(_camera);
            }
        }

        private void Settings(ScriptableCullingParameters parameters)
        {
            //_commandBuffer = new CommandBuffer { name = _camera.name };
            _cullingResult = _context.Cull(ref parameters);
            _context.SetupCameraProperties(_camera);
            _commandBuffer.ClearRenderTarget(true, true, Color.clear);
            _commandBuffer.BeginSample(BUFFER_NAME);
            ExecuteCommandBuffer();
        }

        private void Submit()
        {
            _commandBuffer.EndSample(BUFFER_NAME);
            ExecuteCommandBuffer();
            _context.Submit();
        }

        private void ExecuteCommandBuffer()
        {
            _context.ExecuteCommandBuffer(_commandBuffer);
            _commandBuffer.Clear();
        }

        private DrawingSettings CreateDrawingSettings(List<ShaderTagId> shaderTags, SortingCriteria sortingCriteria, out SortingSettings sortingSettings)
        {
            sortingSettings = new SortingSettings(_camera)
            {
                criteria = sortingCriteria,
            };

            var drawingSettings = new DrawingSettings(shaderTags[0], sortingSettings);

            for (var i = 1; i < shaderTags.Count; i++)
            {
                drawingSettings.SetShaderPassName(i, shaderTags[i]);
            }

            return drawingSettings;
        }

        private bool Cull(out ScriptableCullingParameters parameters)
        {
            return _camera.TryGetCullingParameters(out parameters);
        }

        private void DrawVisible()
        {
            var drawingSettings = CreateDrawingSettings(_drawingShaderTagIds, SortingCriteria.CommonOpaque, out var sortingSettings);
            var filteringSettings = new FilteringSettings(RenderQueueRange.all);

            _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
            _context.DrawSkybox(_camera);

            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSettings.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;

            _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
        }

        void DrawGizmos()
        {
            if (!Handles.ShouldRenderGizmos())
            {
                return;
            }
            _context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
            _context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
        }

#if UNITY_EDITOR
        private static readonly ShaderTagId[] _legacyShaderTagIds =
        {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };

        private static Material _errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));

        private void DrawUnsupportedShaders()
        {
            var drawingSettings =
                new DrawingSettings(_legacyShaderTagIds[0], new SortingSettings(_camera))
                {
                    overrideMaterial = _errorMaterial,
                };

            for (var i = 1; i < _legacyShaderTagIds.Length; i++)
            {
                drawingSettings.SetShaderPassName(i, _legacyShaderTagIds[i]);
            }

            var filteringSettings = FilteringSettings.defaultValue;

            _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
        }
#endif

    }
}