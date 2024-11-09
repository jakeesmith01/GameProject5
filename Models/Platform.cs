using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject5.Models
{
    public class Platform
    {
        private Game game;
        private Vector3 position;
        private float width;
        private float height;
        private float depth;
        private Texture2D texture;
        private BasicEffect effect;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        /// <summary>
        /// Creates a new instance of a platform with a specified width
        /// </summary>
        /// <param name="game">The game the platform belongs to</param>
        /// <param name="width">The width of the platform</param>
        public Platform(Game game, float width)
        {
            this.game = game;
            this.position = new Vector3(0, -5f, 0);
            this.width = width;
            this.height = 0.5f;
            this.depth = 10.0f;
            this.texture = game.Content.Load<Texture2D>("crate2_diffuse");

            InitializeVertices();
            InitializeEffect();
        }

        /// <summary>
        /// Initialies the vertices and the indexes for the platform
        /// </summary>
        private void InitializeVertices()
        {
            float halfWidth = width / 2;
            float halfHeight = height / 2;
            float halfDepth = depth / 2;

            
            var vertices = new VertexPositionNormalTexture[]
            {
            // Bottom Face
            new VertexPositionNormalTexture(new Vector3(-halfWidth, -halfHeight, -halfDepth), Vector3.Down, new Vector2(0, 1)),
            new VertexPositionNormalTexture(new Vector3( halfWidth, -halfHeight, -halfDepth), Vector3.Down, new Vector2(1, 1)),
            new VertexPositionNormalTexture(new Vector3( halfWidth, -halfHeight,  halfDepth), Vector3.Down, new Vector2(1, 0)),
            new VertexPositionNormalTexture(new Vector3(-halfWidth, -halfHeight,  halfDepth), Vector3.Down, new Vector2(0, 0)),

            // Top Face
            new VertexPositionNormalTexture(new Vector3(-halfWidth, halfHeight, -halfDepth), Vector3.Up, new Vector2(0, 1)),
            new VertexPositionNormalTexture(new Vector3( halfWidth, halfHeight, -halfDepth), Vector3.Up, new Vector2(1, 1)),
            new VertexPositionNormalTexture(new Vector3( halfWidth, halfHeight,  halfDepth), Vector3.Up, new Vector2(1, 0)),
            new VertexPositionNormalTexture(new Vector3(-halfWidth, halfHeight,  halfDepth), Vector3.Up, new Vector2(0, 0)),

            // Front Face
            new VertexPositionNormalTexture(new Vector3(-halfWidth, -halfHeight, -halfDepth), Vector3.Forward, new Vector2(0, 1)),
            new VertexPositionNormalTexture(new Vector3(-halfWidth,  halfHeight, -halfDepth), Vector3.Forward, new Vector2(0, 0)),
            new VertexPositionNormalTexture(new Vector3( halfWidth,  halfHeight, -halfDepth), Vector3.Forward, new Vector2(1, 0)),
            new VertexPositionNormalTexture(new Vector3( halfWidth, -halfHeight, -halfDepth), Vector3.Forward, new Vector2(1, 1)),

            // Back Face
            new VertexPositionNormalTexture(new Vector3(-halfWidth, -halfHeight, halfDepth), Vector3.Backward, new Vector2(1, 1)),
            new VertexPositionNormalTexture(new Vector3( halfWidth, -halfHeight, halfDepth), Vector3.Backward, new Vector2(0, 1)),
            new VertexPositionNormalTexture(new Vector3( halfWidth,  halfHeight, halfDepth), Vector3.Backward, new Vector2(0, 0)),
            new VertexPositionNormalTexture(new Vector3(-halfWidth,  halfHeight, halfDepth), Vector3.Backward, new Vector2(1, 0)),

            // Left Face
            new VertexPositionNormalTexture(new Vector3(-halfWidth, -halfHeight,  halfDepth), Vector3.Left, new Vector2(0, 1)),
            new VertexPositionNormalTexture(new Vector3(-halfWidth,  halfHeight,  halfDepth), Vector3.Left, new Vector2(0, 0)),
            new VertexPositionNormalTexture(new Vector3(-halfWidth,  halfHeight, -halfDepth), Vector3.Left, new Vector2(1, 0)),
            new VertexPositionNormalTexture(new Vector3(-halfWidth, -halfHeight, -halfDepth), Vector3.Left, new Vector2(1, 1)),

            // Right Face
            new VertexPositionNormalTexture(new Vector3(halfWidth, -halfHeight, -halfDepth), Vector3.Right, new Vector2(0, 1)),
            new VertexPositionNormalTexture(new Vector3(halfWidth,  halfHeight, -halfDepth), Vector3.Right, new Vector2(0, 0)),
            new VertexPositionNormalTexture(new Vector3(halfWidth,  halfHeight,  halfDepth), Vector3.Right, new Vector2(1, 0)),
            new VertexPositionNormalTexture(new Vector3(halfWidth, -halfHeight,  halfDepth), Vector3.Right, new Vector2(1, 1)),
            };

            
            var indices = new short[]
            {
            // Bottom Face
            0, 1, 2, 0, 2, 3,

            // Top Face
            4, 5, 6, 4, 6, 7,

            // Front Face
            8, 9, 10, 8, 10, 11,

            // Back Face
            12, 13, 14, 12, 14, 15,

            // Left Face
            16, 17, 18, 16, 18, 19,

            // Right Face
            20, 21, 22, 20, 22, 23
            };

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            indexBuffer = new IndexBuffer(game.GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }

        /// <summary>
        /// Initializes the effect for the platform
        /// </summary>
        private void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice)
            {
                TextureEnabled = true,
                Texture = texture,
                LightingEnabled = true,
                AmbientLightColor = new Vector3(0.6f),
                View = Matrix.CreateLookAt(new Vector3(0, 10, -20), Vector3.Zero, Vector3.Up),
                Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1f, 100f)
            };
        }

        /// <summary>
        /// Draws the platform in to the world
        /// </summary>
        /// <param name="view">The view matrix</param>
        /// <param name="projection">The projection matrix</param>
        public void Draw(Matrix view, Matrix projection)
        {
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            game.GraphicsDevice.Indices = indexBuffer;

            effect.View = view;
            effect.Projection = projection;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexBuffer.IndexCount / 3);
            }
        }
    }

}
