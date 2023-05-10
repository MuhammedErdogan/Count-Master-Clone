using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static void DelayedAction(this MonoBehaviour monoBehaviour, float delay, Action action, out Coroutine coroutine)
    {
        coroutine = monoBehaviour.StartCoroutine(DelayedCoroutine(delay, action));
    }

    private static IEnumerator DelayedCoroutine(float delay, Action action)
    {
        yield return BetterWaitForSeconds.Wait(delay);
        action();
    }
}
