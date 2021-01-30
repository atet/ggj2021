using UnityEngine;
using KennethDevelops.ProLibrary.Managers;
using KennethDevelops.ProLibrary.Util;
using KennethDevelops.ProLibrary.DataStructures.Pool;

public class CrowdPerson : MonoBehaviour, IPoolObject
{
    public float speed = 5f;
    public float secondsToDestroy = 2f;

    private Rigidbody rb;
    private Coroutine _disposeCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    public void OnAcquire()
    {
        _disposeCoroutine = StartCoroutine(new WaitForSecondsAndDo(secondsToDestroy, DisposePerson));
    }

    private void DisposePerson()
    {
        PoolManager.GetInstance("CrowdPool").Dispose(this);
    }

    public void OnDispose()
    {
        if (_disposeCoroutine != null) StopCoroutine(_disposeCoroutine);
    }
}
