using UnityEngine;

public class ScreenOrientationController : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;
    }
}
