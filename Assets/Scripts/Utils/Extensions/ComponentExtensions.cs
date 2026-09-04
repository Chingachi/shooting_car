using System.Threading;
using ModestTree;
using UnityEngine;

namespace Utils.Extensions
{
    public static class ComponentExtensions
    {
        public static void TrySerialize<T>(this GameObject source, ref T result) where T : Component
        {
            if(result != null)
            {
                return;
            }

            if(source.TryGetComponent(out T component))
            {
                result = component;
            } else
            {
                Log.Error($"{source} - {typeof(T).Name} has not been serialized");
            }
        }

        public static void Reset(this Transform source)
        {
            source.localPosition = Vector3.zero;
            source.localRotation = Quaternion.identity;
            source.localScale = Vector3.one;
        }

        public static void DisposeSource(this CancellationTokenSource source)
        {
            if(source == null)
            {
                return;
            }

            if(!source.IsCancellationRequested)
            {
                source.Cancel();
            }

            source.Dispose();
        }
    }
}