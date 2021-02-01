using UnityEngine;
using KennethDevelops.ProLibrary.Managers;
using KennethDevelops.ProLibrary.Util;
using KennethDevelops.ProLibrary.DataStructures.Pool;

public class CrowdPerson : MonoBehaviour, IPoolObject
{
    public float directionMin = 0;
    public float directionMax = 0;
    public float speed = 5f;
    public float secondsToDestroy = 2f;

    private Rigidbody rb;
    private Coroutine disposeCoroutine;
    private Quaternion rotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotation = Quaternion.Euler(new Vector3(0, Random.Range(directionMin, directionMax), 0));
    }

    private void FixedUpdate()
    {
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        rb.MoveRotation(rotation);
        rb.MovePosition(rb.position + movement);
    }

    public void OnAcquire()
    {
        disposeCoroutine = StartCoroutine(new WaitForSecondsAndDo(secondsToDestroy, DisposePerson));
    }

    private void DisposePerson()
    {
        PoolManager.GetInstance("CrowdPool").Dispose(this);
    }

    public void OnDispose()
    {
        if (disposeCoroutine != null) StopCoroutine(disposeCoroutine);
    }
}
