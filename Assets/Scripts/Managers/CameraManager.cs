using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Transform mainCamera;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ShakeCamera(float duration, float size)
    {
        StartCoroutine(Shake(duration, size));
    }

    private IEnumerator Shake(float duration, float size)
    {
        float timeElapsed = 0f;
        Vector3 startPosition = mainCamera.localPosition;

        while(timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * size;
            float y = Random.Range(1f, 1f) * size;

            Vector3 position = new Vector3 (x, y, startPosition.z);
            mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, position, Time.deltaTime * 5f);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //while(Vector3.Distance(mainCamera.position, startPosition) > 0.01f)
        //{
        //    mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, startPosition, Time.deltaTime * 2f);
        //    yield return null;
        //}

        mainCamera.localPosition = startPosition;
    }
}
