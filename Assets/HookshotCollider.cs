using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookshotCollider : MonoBehaviour
{
    [SerializeField] Hookshot _hookshot;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hookshot")
        {
            FindObjectOfType<CameraController>().DisableFirstPerson();
            _hookshot.HookToPosition(transform.position);
        }
        Debug.Log($"Trigger Object {other.gameObject.name}");
    }

}
