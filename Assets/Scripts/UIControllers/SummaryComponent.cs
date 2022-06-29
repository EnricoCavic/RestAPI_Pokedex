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
        txtName.UpdateResourse(_pokeInfo.generalNode["name"]);
        txtDexNumber.UpdateResourse(_pokeInfo.generalNode["id"]);
        imgPokemon.UpdateResourse(_pokeInfo.frontDefaultTexture);

        txtTitle.UpdateResourse(_pokeInfo.genus);

        txtDescription.UpdateResourse(_pokeInfo.latestEnFlavorText); 

        for (int i = 0; i < imgTypes.Count; i++)
        {
            string typeName = _pokeInfo.GetTypeName(i);
            if(typeName == null)
                break;
            imgTypes[i].UpdateResourse(typeDictionary[typeName]);
        }

        string eggGroups = "\n";
        eggGroups += "\n" + _pokeInfo.field1Name;
        if(_pokeInfo.field2Node != null)
            eggGroups += "\n" + _pokeInfo.field2Name;

        txtEggGroup.UpdateResourse(eggGroups);

        string evYield = "\n";
        for (int i = 0; i < 6; i++)
        {
            if(_pokeInfo.GetStatEffort(i) > 0)
            {
                string statName = _pokeInfo.GetStatName(i);
                statName = statName.Substring(0,1).ToUpper() + statName.Substring(1);
                evYield += "\n+" + _pokeInfo.GetStatEffort(i).ToString() + " " + statName;
            }
        }
        txtEVYield.UpdateResourse(evYield);


    }

}
