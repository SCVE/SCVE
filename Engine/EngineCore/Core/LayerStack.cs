using System.Collections.Generic;

namespace Engine.EngineCore.Core
{
    public class LayerStack
    {
        public LayerStack()
        {
        }

        public void PushLayer(Layer layer)
        {
            _layers.Insert(_layerInsertIndex, layer);
        }

        public void PushOverlay(Layer overlay)
        {
            _layers.Add(overlay);
        }

        public void PopLayer(Layer layer)
        {
            var layerIndex = _layers.IndexOf(layer);

            if (layerIndex != -1)
            {
                layer.OnDetach();
                _layers.RemoveAt(layerIndex);
                _layerInsertIndex--;
            }
        }

        public void PopOverlay(Layer overlay)
        {
            var layerIndex = _layers.IndexOf(overlay);

            if (layerIndex != -1)
            {
                overlay.OnDetach();
                _layers.RemoveAt(layerIndex);
            }
        }

        public IEnumerator<Layer> GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        ~LayerStack()
        {
            foreach (var layer in _layers)
            {
                layer.OnDetach();
            }
        }

        private IList<Layer> _layers;
        private int _layerInsertIndex = 0;
    }
}