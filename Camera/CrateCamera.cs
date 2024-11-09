using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace GameProject5.Camera
{
    public class CrateCamera : ICamera
    {
        Vector3 target;

        Vector3 position;

        Game game;

        Matrix view;

        Matrix projection;

        public Matrix View => view;

        public Matrix Projection => projection;

        private float verticalOffset = 10f;
        private float distanceBehindTarget = 20f;

        public CrateCamera(Game game, Vector3 position)
        {
            this.game = game;
            this.position = position;
            this.target = Vector3.Zero;

            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1, 1000);

            UpdateViewMatrix();
        }

        public void SetTarget(Vector3 newTarget)
        {
            target = newTarget;
            UpdateViewMatrix();
        }

        public void Update(GameTime gameTime)
        {
            Vector3 desiredPosition = new Vector3(
                target.X,
                target.Y + verticalOffset,
                target.Z - distanceBehindTarget
            );

            position = Vector3.Lerp(position, desiredPosition, 0.1f);

            UpdateViewMatrix();
        }

        public void UpdateViewMatrix()
        {
            view = Matrix.CreateLookAt(position, target, Vector3.Up);
        }
    }
}
