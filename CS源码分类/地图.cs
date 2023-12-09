using Engine;
using Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class MapCamera : BasePerspectiveCamera
    {
        public bool IsOrthographic=true;

        public const int MapSize = 100;

        public float ViewRange { get; set; } = (float)SettingsManager.VisibilityRange;

        public override bool UsesMovementControls
        {
            get
            {
                return false;
            }
        }

        public override bool IsEntityControlEnabled
        {
            get
            {
                return true;
            }
        }
        //public Vector2 m_viewportSize = new Vector2(160,160;)
        public Vector3 TargetPosition { get; set; }

        public Vector3 CameraPosition { get; set; }

        public Vector3 CameraDirection { get; set; }

        public Vector3 CameraUp { get; set; }

        public MapCamera(GameWidget gameWidget) : base(gameWidget)
        {
            this.CameraPosition = gameWidget.ActiveCamera.ViewPosition;
            this.CameraDirection = -Vector3.UnitY;
            this.CameraUp = Vector3.UnitZ;
        }

        public override void Update(float dt)
        {
            if (this.IsOrthographic)
            {
                this.CameraPosition = new Vector3(this.CameraPosition.X, 256f, this.CameraPosition.Z);
            }
            if (base.GameWidget.Target == null)
            {
                return;
            }
            float? num = null;
            for (int i = 0; i <= 0; i++)
            {
                for (int j = 0; j <= 0; j++)
                {
                    TerrainRaycastResult? terrainRaycastResult = base.GameWidget.SubsystemGameWidgets.SubsystemTerrain.Raycast(this.TargetPosition, this.CameraPosition, false, true, (int value, float distance) => !BlocksManager.Blocks[Terrain.ExtractContents(value)].IsTransparent_(value) || BlocksManager.Blocks[Terrain.ExtractContents(value)].IsCollidable_(value));
                    if (terrainRaycastResult != null)
                    {
                        num = new float?((num != null) ? MathUtils.Min(num.Value, terrainRaycastResult.Value.Distance) : terrainRaycastResult.Value.Distance);
                        this.CameraPosition = this.TargetPosition + Vector3.Normalize(-this.CameraDirection) * MathUtils.Max(num.Value - 0.5f, base.GameWidget.PlayerData.ComponentPlayer.ComponentBody.BoxSize.Y + 0.4f);
                    }
                }
            }
            if (this.CameraPosition != this.ViewPosition)
            {
                this.CameraPosition = Vector3.Lerp(this.ViewPosition, this.CameraPosition, 5f * dt);
            }
            base.SetupPerspectiveCamera(this.CameraPosition, this.CameraDirection, this.CameraUp);
        }

        public override Matrix ProjectionMatrix
        {
            get
            {
                if (this.IsOrthographic)
                {
                    Viewport viewport = Display.Viewport;
                    ViewWidget viewWidget = base.GameWidget.ViewWidget;
                    float num = MathUtils.Min(viewWidget.ActualSize.X, viewWidget.ActualSize.Y);
                    float num2 = MapSize / 300;
                    float viewRange = this.ViewRange;
                    Matrix matrix = Matrix.CreateOrthographic(viewRange / num2, viewRange, -1f, this.CameraPosition.Y + viewRange);
                    Matrix matrix2 = MapCamera.CreateScaleTranslation(num, 0f - num, viewWidget.ActualSize.X / 2f, viewWidget.ActualSize.Y / 2f) * viewWidget.GlobalTransform * MapCamera.CreateScaleTranslation(2f / (float)viewport.Width, -2f / (float)viewport.Height, -1f, 1f);
                    this.m_projectionMatrix = new Matrix?(matrix * matrix2);
                    return this.m_projectionMatrix.Value;
                }
                if (this.m_projectionMatrix == null)
                {
                    this.m_projectionMatrix = new Matrix?(BasePerspectiveCamera.CalculateBaseProjectionMatrix(base.GameWidget.ViewWidget.ActualSize));
                    ViewWidget viewWidget2 = base.GameWidget.ViewWidget;
                    if (viewWidget2.ScalingRenderTargetSize == null)
                    {
                        this.m_projectionMatrix *= MapCamera.CreateScaleTranslation(0.5f * viewWidget2.ActualSize.X, -0.5f * viewWidget2.ActualSize.Y, viewWidget2.ActualSize.X / 2f, viewWidget2.ActualSize.Y / 2f) * viewWidget2.GlobalTransform * MapCamera.CreateScaleTranslation(2f / (float)Display.Viewport.Width, -2f / (float)Display.Viewport.Height, -1f, 1f);
                    }
                }
                return this.m_projectionMatrix.Value;
            }
        }

        public static Matrix CreateScaleTranslation(float sx, float sy, float tx, float ty)
        {
            return new Matrix(sx, 0f, 0f, 0f, 0f, sy, 0f, 0f, 0f, 0f, 1f, 0f, tx, ty, 0f, 1f);
        }

        public RenderTarget2D ViewTexture = new RenderTarget2D(MapSize, MapSize, 1, ColorFormat.Rgba8888, DepthFormat.Depth24Stencil8);
    }

    public class MapViewWidget : TouchInputWidget, IDragTargetWidget
    {
        public SubsystemDrawing m_subsystemDrawing;

        public RenderTarget2D m_scalingRenderTarget;

        public static RenderTarget2D ScreenTexture = new RenderTarget2D(MapCamera.MapSize, MapCamera.MapSize, 1, ColorFormat.Rgba8888, DepthFormat.Depth24Stencil8);

        public GameWidget GameWidget
        {
            get;
            set;
        }

        public MapCamera MapCamera
        {
            get;
            set;
        }

        public Point2? ScalingRenderTargetSize
        {
            get
            {
                if (m_scalingRenderTarget == null)
                {
                    return null;
                }
                return new Point2(m_scalingRenderTarget.Width, m_scalingRenderTarget.Height);
            }
        }

        public override void ChangeParent(ContainerWidget parentWidget)
        {
            if (parentWidget.ParentWidget is GameWidget)
            {
                GameWidget = (GameWidget)parentWidget.ParentWidget;
                m_subsystemDrawing = GameWidget.SubsystemGameWidgets.Project.FindSubsystem<SubsystemDrawing>(throwOnError: true);
                base.ChangeParent(parentWidget);
                return;
            }
            throw new InvalidOperationException("ViewWidget must be a child of GameWidget.");
        }

        public override void MeasureOverride(Vector2 parentAvailableSize)
        {
            IsDrawRequired = true;
            base.MeasureOverride(parentAvailableSize);
        }

        public override void Draw(DrawContext dc)
        {
            if (GameWidget.PlayerData.ComponentPlayer != null && GameWidget.PlayerData.IsReadyForPlaying)
            {
                DrawToScreen(dc);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            Utilities.Dispose(ref m_scalingRenderTarget);
        }

        public void DragOver(Widget dragWidget, object data)
        {
        }

        public void DragDrop(Widget dragWidget, object data)
        {
            var inventoryDragData = data as InventoryDragData;
            if (inventoryDragData != null && GameManager.Project != null)
            {
                SubsystemPickables subsystemPickables = GameManager.Project.FindSubsystem<SubsystemPickables>(throwOnError: true);
                ComponentPlayer componentPlayer = GameWidget.PlayerData.ComponentPlayer;
                int slotValue = inventoryDragData.Inventory.GetSlotValue(inventoryDragData.SlotIndex);
                int count = (componentPlayer != null && componentPlayer.ComponentInput.SplitSourceInventory == inventoryDragData.Inventory && componentPlayer.ComponentInput.SplitSourceSlotIndex == inventoryDragData.SlotIndex) ? 1 : ((inventoryDragData.DragMode != DragMode.SingleItem) ? inventoryDragData.Inventory.GetSlotCount(inventoryDragData.SlotIndex) : MathUtils.Min(inventoryDragData.Inventory.GetSlotCount(inventoryDragData.SlotIndex), 1));
                int num = inventoryDragData.Inventory.RemoveSlotItems(inventoryDragData.SlotIndex, count);
                if (num > 0)
                {
                    Vector2 vector = dragWidget.WidgetToScreen(dragWidget.ActualSize / 2f);
                    Vector3 value = Vector3.Normalize(GameWidget.ActiveCamera.ScreenToWorld(new Vector3(vector.X, vector.Y, 1f), Matrix.Identity) - GameWidget.ActiveCamera.ViewPosition) * 12f;
                    subsystemPickables.AddPickable(slotValue, num, GameWidget.ActiveCamera.ViewPosition, value, null);
                }
            }
        }

        public void SetupScalingRenderTarget()
        {
            float num = (SettingsManager.ResolutionMode == ResolutionMode.Low) ? 0.5f : ((SettingsManager.ResolutionMode != ResolutionMode.Medium) ? 1f : 0.75f);
            float num2 = GlobalTransform.Right.Length();
            float num3 = GlobalTransform.Up.Length();
            var vector = new Vector2(ActualSize.X * num2, ActualSize.Y * num3);
            Point2 point = default;
            point.X = (int)MathUtils.Round(vector.X * num);
            point.Y = (int)MathUtils.Round(vector.Y * num);
            Point2 point2 = point;
            if ((num < 1f || GlobalColorTransform != Color.White) && point2.X > 0 && point2.Y > 0)
            {
                if (m_scalingRenderTarget == null || m_scalingRenderTarget.Width != point2.X || m_scalingRenderTarget.Height != point2.Y)
                {
                    Utilities.Dispose(ref m_scalingRenderTarget);
                    m_scalingRenderTarget = new RenderTarget2D(point2.X, point2.Y, 1, ColorFormat.Rgba8888, DepthFormat.Depth24Stencil8);
                }
                Display.RenderTarget = m_scalingRenderTarget;
                Display.Clear(Color.Black, 1f, 0);
            }
            else
            {
                Utilities.Dispose(ref m_scalingRenderTarget);
            }
        }

        public void ApplyScalingRenderTarget(DrawContext dc)
        {
            if (m_scalingRenderTarget != null)
            {
                BlendState blendState = (GlobalColorTransform.A < byte.MaxValue) ? BlendState.AlphaBlend : BlendState.Opaque;
                TexturedBatch2D texturedBatch2D = dc.PrimitivesRenderer2D.TexturedBatch(m_scalingRenderTarget, useAlphaTest: false, 0, DepthStencilState.None, RasterizerState.CullNoneScissor, blendState, SamplerState.PointClamp);
                int count = texturedBatch2D.TriangleVertices.Count;
                texturedBatch2D.QueueQuad(Vector2.Zero, ActualSize, 0f, Vector2.Zero, Vector2.One, GlobalColorTransform);
                texturedBatch2D.TransformTriangles(GlobalTransform, count);
                dc.PrimitivesRenderer2D.Flush();
            }
        }

        public void DrawToScreen(DrawContext dc)
        {
            MapCamera = GameWidget.PlayerData.ComponentPlayer.Entity.FindComponent<ComponentPlayerSystem>(true).MapCamera;
            MapCamera.PrepareForDrawing();
            RenderTarget2D renderTarget = Display.RenderTarget;
            SetupScalingRenderTarget();
            try
            {
                m_subsystemDrawing.Draw(GameWidget.PlayerData.ComponentPlayer.Entity.FindComponent<ComponentPlayerSystem>(true).MapCamera);
            }
            finally
            {
                Display.RenderTarget = renderTarget;
            }
            ApplyScalingRenderTarget(dc);
            //ModsManager.HookAction("DrawToScreen", loader => {loader.DrawToScreen(this, dc); return false;});
        }
    }
}
