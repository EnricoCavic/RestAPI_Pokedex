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
    [SerializeField] TMPController txtEVYield;
    [SerializeField] TMPController txtEggGroup;  
    List<IUIResourse<string>> txtList;

    [Header("Image Objects")]
    [SerializeField] ImgController imgPokemon;
    [SerializeField] List<SpriteController> imgTypes;

    [Header("Type Dictionary")]
    Dictionary<string, Sprite> typeDictionary;
    [SerializeField] List<Sprite> typeSprites;
    [SerializeField] List<string> typeStrings;

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
        txtList.Add(txtEVYield);
        txtList.Add(txtEggGroup);

        typeDictionary = new Dictionary<string, Sprite>();
        for (int i = 0; i < typeSprites.Count; i++)
        {
            typeDictionary.Add(typeStrings[i],typeSprites[i]);
        }
    }

    public void BlankAllResourses()
    {
        for (int i = 0; i < txtList.Count; i++) 
            txtList[i].BlankResourse();

        for (int i = 0; i < imgTypes.Count; i++) 
            imgTypes[i].BlankResourse();

        imgPokemon.BlankResourse();
        
    }

    public void UpdatePkmnUI(PokemonInfo _pokeInfo)
    {
        txtName.UpdateResourse(_pokeInfo.general["name"]);
        txtDexNumber.UpdateResourse(_pokeInfo.general["id"]);
        imgPokemon.UpdateResourse(_pokeInfo.frontDefaultTexture);

        txtTitle.UpdateResourse(_pokeInfo.species["genera"][7]["genus"]);

        txtDescription.UpdateResourse(FindLastestEnFlavorText(_pokeInfo.species["flavor_text_entries"])); 

        for (int i = 0; i < imgTypes.Count; i++)
        {
            string typeName = _pokeInfo.general["types"][i]["type"]["name"];
            if(typeName == null)
                break;
            imgTypes[i].UpdateResourse(typeDictionary[typeName]);
        }

        string eggGroups = "";
        eggGroups += "\n" + _pokeInfo.field1["names"][6]["name"];
        if(_pokeInfo.field2 != null)
            eggGroups += "\n" + _pokeInfo.field2["names"][6]["name"];

        txtEggGroup.UpdateResourse(eggGroups);


    }

    string FindLastestEnFlavorText(JSONNode _flavorTexts)
    {
        for (int i = _flavorTexts.Count; i > 0; i--)
        {
            if(_flavorTexts[i]["language"]["name"] == "en")
            {
                string txt = _flavorTexts[i]["flavor_text"];
                txt = txt.Replace("\n", " ");
                return txt;           
            }
        }      

        return "";  
    }

}
