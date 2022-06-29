using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexObjectComponent : MonoBehaviour, IUIComponent
{
    [SerializeField] TMPController txtNumber;
    [SerializeField] ImgController imgMiniature;

    public void BlankAllResourses()
    {
        txtNumber.BlankResourse();
        imgMiniature.BlankResourse();
    }

    public void UpdateUI(PokemonInfo _pokeInfo)
    {
        txtNumber.UpdateResourse(_pokeInfo.generalNode["id"]);
        imgMiniature.UpdateResourse(_pokeInfo.miniatureTexture);
    }
}
