using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineManager {
    private static CoroutineHandler _handler;


    public static Coroutine StartCoroutine(IEnumerator coroutine){
        return Handler.StartCoroutine(coroutine);
    }


    private static CoroutineHandler Handler{
        get{
            if (_handler == null){
                var gameObject = new GameObject("CoroutineHandler");
                _handler = gameObject.AddComponent<CoroutineHandler>();
            }
            return _handler;
        }
    }
}

public class CoroutineHandler : MonoBehaviour{ }


