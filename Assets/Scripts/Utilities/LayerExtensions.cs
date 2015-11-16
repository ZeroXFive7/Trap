using UnityEngine;

namespace UnityEngine
{
    public static class LayerExtensions
    {
        public static bool LayerIsInLayerMask(this GameObject gameObject, LayerMask layerMask)
        {
            return layerMask.ContainsLayer(gameObject.layer);
        }

        public static bool ContainsLayer(this LayerMask layerMask, int layerValue)
        {
            return (layerMask.value & (1 << layerValue)) == (1 << layerValue);
        }
    }
}