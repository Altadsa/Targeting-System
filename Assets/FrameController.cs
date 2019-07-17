using System.Collections;
using UnityEngine;

public class FrameController : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(FixFramerate());
    }

    IEnumerator FixFramerate()
    {
        yield return new WaitForSeconds(1);
        Application.targetFrameRate = 30;
    }
}