using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_TargetLockon : MonoBehaviour
{
    [SerializeField] private Transform _target;
    public static Transform Target;

    public static bool HasTarget = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Target = _target;
            HasTarget = true;
        }
        else
        {
            Target = null;
            HasTarget = false;
        }
    }
}
