using OpenTK.Mathematics;

namespace Engine.EngineCore.Renderer
{
    public class OrthographicCamera
    {
        public OrthographicCamera(float left, float right, float bottom, float top)
        {
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
            _viewMatrix = Matrix4.Identity;

            _viewProjectionMatrix = _projectionMatrix * _viewMatrix;
        }

        public void SetProjection(float left, float right, float bottom, float top)
        {
            _projectionMatrix = Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1, 1);
            _viewProjectionMatrix = _projectionMatrix * _viewMatrix;
        }

        public ref Vector3 GetPosition()
        {
            return ref _position;
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
            RecalculateViewMatrix();
        }

        public float GetRotation()
        {
            return _rotation;
        }

        public void SetRotation(float rotation)
        {
            _rotation = rotation;
            RecalculateViewMatrix();
        }

        public ref Matrix4 GetProjectionMatrix()
        {
            return ref _projectionMatrix;
        }

        public ref Matrix4 GetViewMatrix()
        {
            return ref _viewMatrix;
        }

        public ref Matrix4 GetViewProjectionMatrix()
        {
            return ref _viewProjectionMatrix;
        }

        private void RecalculateViewMatrix()
        {
            var transform = Matrix4.CreateTranslation(_position) *
                Matrix4.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(_rotation));

            _viewMatrix = Matrix4.Invert(transform);
            _viewProjectionMatrix = _projectionMatrix * _viewMatrix;
        }

        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;
        private Matrix4 _viewProjectionMatrix;

        private Vector3 _position = Vector3.Zero;
        private float _rotation = 0f;
    }
}