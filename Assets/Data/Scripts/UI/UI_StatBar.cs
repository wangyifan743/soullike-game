using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    private Slider slider;
    private RectTransform rectTransform;

    [Header("Bar Options")]
    [SerializeField] bool scaleBarWidthWithStats = true;
    [SerializeField] float widthScaleMultiplier;

    protected virtual void Awake() {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetSta(float newValue){
        slider.value = newValue;
    } 

    public void SetMaxStat(float maxValue){
        slider.maxValue = maxValue;
        slider.value = maxValue;

        if(scaleBarWidthWithStats){
            rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
            PlayerUIManager.instance.playerUIHUDManager.RefreshHUD();
        }
    }



}
