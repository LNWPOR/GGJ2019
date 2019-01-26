using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private readonly float MAX_TEXT_SIZE = 1.5f;
    private const float SCALE_UP_TIME = 1f;
    private const float FADE_TIME = 1f;
    private const float ZOOM_TIME = 1f;

    [SerializeField]
    private GameObject _Camera;
    [SerializeField]
    private Image _FadeImage;
    [SerializeField]
    private Text _GreetText;
    [SerializeField]
    private GameObject _Canvas;
    // Start is called before the first frame update
    void Start()
    {
        _Canvas.SetActive(true);
        StartCoroutine(DoScaleUpText(() =>
        {
            StartCoroutine(DoFade(()=> 
            {}));
            StartCoroutine(DoZoom(() => { Destroy(_Canvas); }));
        }));
    }

    IEnumerator DoScaleUpText(System.Action callback)
    {
        float time = 0f;
        while (time < SCALE_UP_TIME)
        {
            float frac = time / SCALE_UP_TIME;
            _GreetText.transform.localScale = new Vector2(frac * MAX_TEXT_SIZE, frac * MAX_TEXT_SIZE);
            _GreetText.color = new Color(_GreetText.color.r, _GreetText.color.g, _GreetText.color.b, frac);
            time += Time.deltaTime;
            yield return null;
        }
        _GreetText.transform.localScale = new Vector2(MAX_TEXT_SIZE, MAX_TEXT_SIZE);
        _GreetText.color = new Color(_GreetText.color.r, _GreetText.color.g, _GreetText.color.b, 0f);
        callback();
    }

    IEnumerator DoFade(System.Action callback)
    {
        float time = 0f;
        while (time < FADE_TIME)
        {
            float frac = time / FADE_TIME;
            _FadeImage.color = new Color(_FadeImage.color.r, _FadeImage.color.g, _FadeImage.color.b, (1f - frac));
            time += Time.deltaTime;
            yield return null;
        }
        _FadeImage.color = new Color(_FadeImage.color.r, _FadeImage.color.g, _FadeImage.color.b, 0f);
        callback();
    }

    IEnumerator DoZoom(System.Action callback)
    {
        _Camera.GetComponent<CameraController>().enabled = true;
        float time = 0f;
        float originalSize = Camera.main.orthographicSize;
        while (time < ZOOM_TIME)
        {
            float frac = time / ZOOM_TIME;
            Camera.main.orthographicSize =  (1f - frac) * originalSize + frac * 20f;
            time += Time.deltaTime;
            yield return null;
        }
        Camera.main.orthographicSize = 20f;
        callback();
    }
}
