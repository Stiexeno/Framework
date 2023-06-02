using System.Collections;
using UnityEngine;

namespace Framework.Core
{
    public interface ICoroutineManager
    {
        Coroutine Begin(IEnumerator coroutine);
        void Begin(IEnumerator coroutine, ref Coroutine current);
        void Stop(ref Coroutine coroutine);
    }
}
