using UnityEngine;
using UnityEngine.UI;

public class ImgController : MonoBehaviour, IUIResourse<Texture>
{
    RawImage imageComponent;

    private void Awake() => GetUIObject();   

    public void GetUIObject() => imageComponent = GetComponent<RawImage>();
    public void UpdateResourse(Texture _newContent) => imageComponent.texture = _newContent;
    public void BlankResourse() => imageComponent.texture = Texture2D.blackTexture;
}
