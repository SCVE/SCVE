using System;
using Engine.EngineCore.Core;
using Engine.EngineCore.Events;
using OpenTK.Mathematics;

namespace Engine.EngineCore.Renderer
{
    public class OrthographicCameraController
    {
        public OrthographicCameraController(float aspectRatio, bool rotation = false)
        {
            _aspectRatio = aspectRatio;
            _camera = new OrthographicCamera(_aspectRatio * _zoomLevel, _aspectRatio * _zoomLevel, _zoomLevel, _zoomLevel);
            _rotation = rotation;
        }

        public void OnUpdate(Timestep ts)
        {
            if (Input.Instance.IsKeyPressed(KeyCode.A))
            {
                _cameraPosition.X -= MathF.Cos(MathHelper.DegreesToRadians(_cameraRotation)) * _cameraTranslationSpeed * ts;
                _cameraPosition.Y -= MathF.Sin(MathHelper.DegreesToRadians(_cameraRotation)) * _cameraTranslationSpeed * ts;
            }
            else if (Input.Instance.IsKeyPressed(KeyCode.D))
            {
                _cameraPosition.X += MathF.Cos(MathHelper.DegreesToRadians(_cameraRotation)) * _cameraTranslationSpeed * ts;
                _cameraPosition.Y += MathF.Sin(MathHelper.DegreesToRadians(_cameraRotation)) * _cameraTranslationSpeed * ts;
            }

            if (Input.Instance.IsKeyPressed(KeyCode.W))
            {
                _cameraPosition.X += -MathF.Sin(MathHelper.DegreesToRadians(_cameraRotation)) * _cameraTranslationSpeed * ts;
                _cameraPosition.Y += MathF.Cos(MathHelper.DegreesToRadians(_cameraRotation)) * _cameraTranslationSpeed * ts;
            }
            else if (Input.Instance.IsKeyPressed(KeyCode.S))
            {
                _cameraPosition.X -= -MathF.Sin(MathHelper.DegreesToRadians(_cameraRotation)) * _cameraTranslationSpeed * ts;
                _cameraPosition.Y -= MathF.Cos(MathHelper.DegreesToRadians(_cameraRotation)) * _cameraTranslationSpeed * ts;
            }

            if (_rotation)
            {
                if (Input.Instance.IsKeyPressed(KeyCode.Q))
                    _cameraRotation += _cameraRotationSpeed * ts;
                if (Input.Instance.IsKeyPressed(KeyCode.E))
                    _cameraRotation -= _cameraRotationSpeed * ts;

                if (_cameraRotation > 180.0f)
                    _cameraRotation -= 360.0f;
                else if (_cameraRotation <= -180.0f)
                    _cameraRotation += 360.0f;

                _camera.SetRotation(_cameraRotation);
            }

            _camera.SetPosition(_cameraPosition);

            _cameraTranslationSpeed = _zoomLevel;
        }

        public void OnEvent(Event e)
        {
            EventDispatcher dispatcher = new(e);
            dispatcher.Dispatch<MouseScrolledEvent>(OnMouseScrolled);
            dispatcher.Dispatch<WindowResizeEvent>(OnWindowResized);
        }

        public void OnResize(float width, float height)
        {
            _aspectRatio = width / height;
            _camera.SetProjection(-_aspectRatio * _zoomLevel, _aspectRatio * _zoomLevel, -_zoomLevel, _zoomLevel);
        }

        public OrthographicCamera GetCamera()
        {
            return _camera;
        }

        public float GetZoomLevel()
        {
            return _zoomLevel;
        }

        public void SetZoomLevel(float level)
        {
            _zoomLevel = level;
        }

        private bool OnMouseScrolled(MouseScrolledEvent e)
        {
            _zoomLevel -= e.GetYOffset() * 0.25f;
            _zoomLevel = Math.Max(_zoomLevel, 0.25f);
            _camera.SetProjection(-_aspectRatio * _zoomLevel, _aspectRatio * _zoomLevel, -_zoomLevel, _zoomLevel);
            return false;
        }

        private bool OnWindowResized(WindowResizeEvent e)
        {
            OnResize(e.GetWidth(), e.GetHeight());
            return false;
        }

        private float _aspectRatio;
        private float _zoomLevel = 1.0f;
        private OrthographicCamera _camera;

        private bool _rotation;

        private Vector3 _cameraPosition = Vector3.Zero;
        private float _cameraRotation = 0.0f; //In degrees, in the anti-clockwise direction
        private float _cameraTranslationSpeed = 5.0f;
        private float _cameraRotationSpeed = 180.0f;
    }
}