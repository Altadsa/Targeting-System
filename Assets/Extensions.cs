using UnityEngine;

public static class Extensions
{
    public static Vector3 ScaledForward(this Camera camera)
    {
        var camForward = camera.transform.forward;
        return new Vector3(camForward.x,0,camForward.z).normalized;
    }

    public static Vector3 ScaledRight(this Camera camera)
    {
        var camRight = camera.transform.right;
        return new Vector3(camRight.x, 0, camRight.z).normalized;
    }
}