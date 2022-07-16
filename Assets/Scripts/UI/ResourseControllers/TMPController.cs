using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPController : MonoBehaviour, IUIResourse<string>
{
    TextMeshProUGUI textComponent;
    [SerializeField] string baseText;

    private void Awake() => GetUIObject();
    
    public void GetUIObject() => textComponent = GetComponent<TextMeshProUGUI>(); 
    public void UpdateResourse(string _newContent) => textComponent.text = baseText + _newContent;
    public void BlankResourse() => textComponent.text = "";
}
