
using UnityEngine;

public class UiHookshot : MonoBehaviour
{
    [SerializeField] Hookshot _hookshot;
    [SerializeField] Camera _main;
    [SerializeField] Canvas _Canvas;

    public void ShowUi()
    {
        gameObject.SetActive(true);
    }
    RaycastHit hit;
    private void Update()
    {
        var camWorldPoint = _main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _hookshot.Distance));
        if (Physics.Linecast(_main.transform.position, camWorldPoint))
        {
            _Canvas.enabled = true;
        }
        else
        {
            _Canvas.enabled = false;
        }
    }

    public void HideUi()
    {
        gameObject.SetActive(false);
    }
}
