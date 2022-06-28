using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class SummaryComponent : MonoBehaviour
{
    [Header("Text Objects")]
    [SerializeField] TMPController txtName;
    [SerializeField] TMPController txtDexNumber;
    [SerializeField] TMPController txtTitle;
    [SerializeField] TMPController txtDescription;    
    List<IUIResourse<string>> txtList;

    [Header("Image Objects")]
    [SerializeField] ImgController imgPokemon;
    [SerializeField] List<ImgController> imgTypes;
    List<IUIResourse<Texture>> imgList;

    private void Awake() 
    {
        Initialize();
    }

    private void Start() 
    {
        BlankAllResourses();    
    }

    private void Initialize()
    {
        txtList = new List<IUIResourse<string>>();
        txtList.Add(txtName);
        txtList.Add(txtDescription);
        txtList.Add(txtTitle);
        txtList.Add(txtDexNumber);

        imgList =  new List<IUIResourse<Texture>>(imgTypes);
        imgList.Add(imgPokemon);
    }

    public void BlankAllResourses()
    {
        for (int i = 0; i < txtList.Count; i++) 
            txtList[i].BlankResourse();

        for (int i = 0; i < imgList.Count; i++) 
            imgList[i].BlankResourse();
    }

    public void UpdatePkmnUI(PokemonInfo _pokeInfo)
    {
        txtName.UpdateResourse(_pokeInfo.general["name"]);
        txtDexNumber.UpdateResourse(_pokeInfo.general["id"]);
        imgPokemon.UpdateResourse(_pokeInfo.frontDefaultTexture);

        txtTitle.UpdateResourse(_pokeInfo.species["genera"][7]["genus"]);

        txtDescription.UpdateResourse(FindLastestEnFlavorText(_pokeInfo.species["flavor_text_entries"])); 
    }

    string FindLastestEnFlavorText(JSONNode _flavorTexts)
    {
        for (int i = _flavorTexts.Count; i > 0; i--)
        {
            if(_flavorTexts[i]["language"]["name"] == "en")
            {
                string txt = _flavorTexts[i]["flavor_text"];
                txt = txt.Replace("\n", "");
                return txt;           
            }
        }      

        return "";  
    }

}
