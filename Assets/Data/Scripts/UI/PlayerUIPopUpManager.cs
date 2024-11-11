using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("You Died Pop Up")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUpText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;

    public void SendYoudiedPopUp(){
        // 弹出死亡面板
        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;

        StartCoroutine(StretchYouDiedBackgroundTextOverTime(youDiedPopUpBackgroundText, 8.0f, 9.0f));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5.0f));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2.0f, 5.0f));
    }

    private IEnumerator StretchYouDiedBackgroundTextOverTime(TextMeshProUGUI backgroundText, float duration, float stretchAmount){
        if(duration > 0){
            backgroundText.characterSpacing = 0;
            float timer = 0;

            yield return null;

            while(timer < duration){
                timer += Time.deltaTime;
                backgroundText.characterSpacing = Mathf.Lerp(backgroundText.characterSpacing, stretchAmount, duration *(Time.deltaTime/20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration){
        if(duration > 0){
            canvas.alpha = 0;
            float timer = 0;
            yield return null;

            while(timer < duration){
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration*Time.deltaTime);
                yield return null;
            }
        }
    }

    private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay){
        if(duration > 0){
            yield return new WaitForSeconds(delay);

            canvas.alpha = 1;
            float timer = 0;
            

            while(timer < duration){
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration*Time.deltaTime);
                yield return null;
            }
        }
    }


}
