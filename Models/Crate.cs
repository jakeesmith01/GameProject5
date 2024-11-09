using System;
using GameProject5.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace GameProject5.Models
{
    public class Crate
    {
        private Game game;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private BasicEffect effect;
        private Texture2D texture;

        private Random rand = new Random();

        private float rotationAngle;

        private Vector3 rotationAxis;

        public Matrix World { get; set; }

        public bool IsFalling { get; set; }

        /// <summary>
        /// Creates a new crate instance at a specified position
        /// </summary>
        /// <param name="game">The game this crate belongs to</param>
        /// <param name="position">The position of the crate in the world</param>
        public Crate(Game game, Vector3 position)
        {
            this.game = game;
            this.texture = game.Content.Load<Texture2D>("crate0_diffuse");
           
            InitializeVertices();
            InitializeIndices();
            InitializeEffect();
            effect.World = Matrix.CreateTranslation(position);
            World = effect.World;

            this.rotationAxis = new Vector3(
                (float)rand.NextDouble(),
                (float)rand.NextDouble(),
                (float)rand.NextDouble()
            );
            this.rotationAxis.Normalize();

        }

        /// <summary>
        /// Updates the rotation for a falling crate based off the time
        /// </summary>
        /// <param name="deltaTime">The amount of time that has passed</param>
        public void UpdateRotation(float deltaTime)
        {
            if (IsFalling)
            {
                rotationAngle += MathHelper.ToRadians(90) * deltaTime;
            }
        }

        /// <summary>
        /// Initializes the vertices of the crate (cube)
        /// </summary>
        public void InitializeVertices()
        {
            var vertexData = new VertexPositionNormalTexture[] { 
                // Front Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Forward },

                // Back Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, 1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Backward },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, 1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f, 1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f, 1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Forward },

                // Top Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, 1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Up },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, 1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Up },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, 1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Up },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, 1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Up },

                // Bottom Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Down },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Down },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Down },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Down },

                // Left Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Left },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Left },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Left },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Left },

                // Right Face
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Right },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Right },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Right },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Right },
            };

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), vertexData.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionNormalTexture>(vertexData);
        }

        /// <summary>
        /// Initializes the Index Buffer
        /// </summary>
        public void InitializeIndices()
        {
            var indexData = new short[]
            {
                // Front face
                0, 2, 1,
                0, 3, 2,

                // Back face 
                4, 6, 5,
                4, 7, 6,

                // Top face
                8, 10, 9,
                8, 11, 10,

                // Bottom face 
                12, 14, 13,
                12, 15, 14,

                // Left face 
                16, 18, 17,
                16, 19, 18,

                // Right face 
                20, 22, 21,
                20, 23, 22
            };

            indexBuffer = new IndexBuffer(game.GraphicsDevice, IndexElementSize.SixteenBits, indexData.Length, BufferUsage.None);
            indexBuffer.SetData<short>(indexData);
        }

        /// <summary>
        /// Initializes the BasicEffect to render our crate
        /// </summary>
        void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.World = Matrix.CreateScale(2.0f);

            effect.View = Matrix.CreateLookAt(
                new Vector3(8, 9, 12), // The camera position
                new Vector3(0, 0, 0), // The camera target,
                Vector3.Up            // The camera up vector
            );

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,                         // The field-of-view 
                game.GraphicsDevice.Viewport.AspectRatio,   // The aspect ratio
                0.1f, // The near plane distance 
                100.0f // The far plane distance
            );


            effect.EnableDefaultLighting();


            effect.TextureEnabled = true;
            effect.Texture = texture;
        }

        /// <summary>
        /// Draws the crate
        /// </summary>
        /// <param name="camera">The camera used for view and projection matrices</param>
        public void Draw(ICamera camera)
        {
            Matrix worldMatrix = World;
            if (IsFalling)
            {
                Matrix rotation = Matrix.CreateFromAxisAngle(rotationAxis, rotationAngle);
                worldMatrix = rotation * World;
            }

            effect.World = worldMatrix;
            effect.View = camera.View;
            effect.Projection = camera.Projection;

            effect.CurrentTechnique.Passes[0].Apply();

            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            game.GraphicsDevice.Indices = indexBuffer;

            game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
        }

        /// <summary>
        /// Sets the position of the crate, allowing it to stack on the previous one.
        /// </summary>
        /// <param name="position">The new position for the crate</param>
        public void SetPosition(Vector3 position)
        {
            World = Matrix.CreateTranslation(position);
            effect.World = World;
        }
    }
}
