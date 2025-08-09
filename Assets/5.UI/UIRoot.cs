using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{
    [SerializeField]
    private Camera _uiCamera;

    [SerializeField]
    private Transform _rootTransform;

    private Canvas _rootCanvas;
    private CanvasScaler _canvasScaler;

    [SerializeField]
    private int _standardWidth;

    [SerializeField]
    private int _standardHeight;

    private Rect _safeArea;


    private void Awake()
    {
        _rootCanvas = _rootTransform.GetComponent<Canvas>();
        _canvasScaler = _rootTransform.GetComponent<CanvasScaler>();
        Init();
    }

    private void Init()
    {
        _safeArea = Screen.safeArea;
        _canvasScaler.referenceResolution = new Vector2(_standardWidth, _standardHeight);
        var standardVerticalRatio = 1f * _standardHeight / _standardWidth;
        var screenSafeRatio = _safeArea.height / _safeArea.width;
        _canvasScaler.matchWidthOrHeight = screenSafeRatio > standardVerticalRatio ? 0 : 1;
    }
}