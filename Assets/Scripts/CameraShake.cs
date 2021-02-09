using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool isShaking = false;
    public bool testShake = false;
    public float testDuration;
    public float testMagnitude;

    //Test shaking behavior using values in editor
    private void Update()
    {
        if (testShake)
        {
            ShakeCamera(testDuration, testMagnitude);
            testShake = false;
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        if (!isShaking)
            StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        isShaking = true;

        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;

        isShaking = false;
    }
}
