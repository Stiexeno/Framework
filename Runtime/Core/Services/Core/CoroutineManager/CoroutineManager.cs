using System.Collections;
using UnityEngine;

namespace Framework.Core
{
    public class CoroutineManager : MonoBehaviour, ICoroutineManager
    {
        public Coroutine Begin(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void Begin(IEnumerator coroutine, ref Coroutine current)
        {
            current = StartCoroutine(coroutine);
        }

        public void Stop(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
