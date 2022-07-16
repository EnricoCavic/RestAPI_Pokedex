using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteController : MonoBehaviour, IUIResourse<Sprite>
{

    Image spriteComponent;

    private void Awake() => GetUIObject();
    public void GetUIObject() => spriteComponent = GetComponent<Image>();
    public void UpdateResourse(Sprite _newContent)
    {
        spriteComponent.color = Color.white;
        spriteComponent.sprite = _newContent;   
    } 
    public void BlankResourse() => spriteComponent.color = Color.clear;

}