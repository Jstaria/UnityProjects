using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusController : Singleton<FocusController>
{
    private FocusOn currentFocus;
    private FocusOn prevFocus;

    public CameraController cameraCon;

    public void SetFocus(FocusOn focus)
    {
        currentFocus = focus;
    }

    public void RevertFocus()
    {
        FocusOn tempFocus = currentFocus;
        currentFocus = prevFocus;
        prevFocus = tempFocus;
    }

    private void UpdateCameraFocus()
    {
        cameraCon.SetFocus(currentFocus);
    }
}
