using UnityEngine;
using VolumetricFogAndMist;

public class FeedbackScriptExample : MonoBehaviour
{
    //private MainSceneScript mainSceneScript;
    //private VolumetricFog fog;
    //private float maxDistance;
    //private float minDistance;

    //public float MaxDensity = .95f;
    //public float MinDensity = .25f;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    mainSceneScript = FindObjectsOfType<MainSceneScript>()[0];
    //    fog = FindObjectsOfType<VolumetricFog>()[0];

    //    minDistance = mainSceneScript.MIN_DISTANCE_TO_PARENTS_FOR_WIN;
    //    maxDistance = mainSceneScript.MAX_DISTANCE_FROM_PARENTS_FOR_LOSE;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    float distance = mainSceneScript.DistanceFromDad;

    //    //var distance = (transform.position - Camera.main.transform.position).magnitude;
    //    var norm = (distance - minDistance) / (maxDistance - minDistance);
    //    norm = Mathf.Clamp01(norm);

    //    fog.density = Mathf.Lerp(MinDensity, MaxDensity, norm);
    //}
}
