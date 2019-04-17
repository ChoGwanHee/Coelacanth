using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIRespawnCounter : MonoBehaviour {

    public bool alive = false;
    public Sprite[] chrSprites;
    private Image counterBg;
    private Text counterText;

    
	void Awake () {
        counterBg = GetComponentInChildren<Image>();
        counterText = GetComponentInChildren<Text>();
	}

    public void SetCount(float remainTime, Vector3 pos, int chrNum, bool isMine)
    {
        gameObject.SetActive(true);
        alive = true;
        transform.position = pos;
        counterBg.sprite = chrSprites[chrNum];

        if(gameObject.activeSelf)
            StartCoroutine(Count(remainTime));
    }

    public void Disable()
    {
        StopAllCoroutines();
        alive = false;
        transform.position = new Vector3(0f, 0f, -20f);
        gameObject.SetActive(false);
    }
	
	private IEnumerator Count(float time)
    {
        do
        {
            time -= Time.deltaTime;
            counterText.text = Mathf.CeilToInt(time).ToString();
            yield return null;
        } while (time > 0);

        alive = false;
        transform.position = new Vector3(0f, 0f, -20f);
        gameObject.SetActive(false);
    }


}
