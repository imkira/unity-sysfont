using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectResizer : MonoBehaviour
{
  public Camera cam;

  private Transform _transform;
  private float _lastOrthographicSize = 0f;
  private float _lastPixelWidth = 0f;
  private float _lastPixelHeight = 0f;

  void Awake()
  {
    _transform = transform;
  }

  void Update()
  {
    if (cam == null)
    {
      cam = Camera.main;
    }
    if (cam != null)
    {
      if ((cam.orthographicSize != _lastOrthographicSize) ||
          (cam.pixelWidth != _lastPixelWidth) ||
          (cam.pixelHeight != _lastPixelHeight))
      {
        _transform.localScale = new Vector3((int)(cam.orthographicSize *
              2000f * cam.aspect / cam.pixelWidth) / 1000f,
            (int)(cam.orthographicSize * 2000f / cam.pixelHeight) / 1000f,
            _transform.localScale.z);
      }
    }
  }
}
