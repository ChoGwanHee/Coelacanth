using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffInfo : MonoBehaviour {

    public float displayTime = 1.2f;
    public float fadeTime = 0.5f;
    public Vector3 offset;
    public Sprite[] sprites;

    private Image infoImage;
    private Transform target;


    private void Awake()
    {
        infoImage = GetComponent<Image>();
    }

    public void DisplayBuffInfo(int num, Transform newTarget)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        infoImage.sprite = sprites[num];
        infoImage.SetNativeSize();
        target = newTarget;
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset) ;
        StartCoroutine(DisplayProcess());
    }

    private IEnumerator DisplayProcess()
    {
        float elapsedTime = 0.0f;
        float ratio = 0.0f;
        Color color = infoImage.color;
        Vector3 pos = target.position;

        // fade in
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            ratio = elapsedTime / fadeTime;

            color.a = ratio;
            infoImage.color = color;

            pos = Camera.main.WorldToScreenPoint(target.position + offset);
            transform.position = pos;

            yield return null;
        }

        // stay
        while (elapsedTime < displayTime)
        {
            elapsedTime += Time.deltaTime;
            
            pos = Camera.main.WorldToScreenPoint(target.position + offset);
            transform.position = pos;

            yield return null;
        }

        elapsedTime = 0.0f;
        int up = 0;

        // fade out
        while(elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            ratio = 1f - (elapsedTime / fadeTime);

            color.a = ratio;
            infoImage.color = color;

            up += 2;
            pos = Camera.main.WorldToScreenPoint(target.position + offset);
            pos.y += up;
            transform.position = pos;

            yield return null;
        }

        color.a = 1.0f;
        infoImage.color = color;

        gameObject.SetActive(false);
    }
}
