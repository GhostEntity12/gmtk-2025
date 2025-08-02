using UnityEngine;

/// <summary>
/// Basic camera tracker
/// </summary>
public class CameraTrack : MonoBehaviour
{
    [SerializeField] Transform trackObject;
    [SerializeField] Transform tracker;

    // Update is called once per frame
    void Update()
    {
        tracker.position = trackObject.position;
    }
}
