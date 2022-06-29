using UnityEngine;
using SimpleJSON;

public struct PokemonInfo
{
    public JSONNode generalNode;
    public JSONNode speciesNode;    
    public JSONNode field1Node;
    public JSONNode field2Node;
    public Texture frontDefaultTexture;
    public string frontDefaultURL => generalNode["sprites"]["front_default"];
    public string field1URL => speciesNode["egg_groups"][0]["url"];
    public string field2URL => speciesNode["egg_groups"][1]["url"];

    public string genus => speciesNode["genera"][7]["genus"];
    public string latestEnFlavorText => FindLastestEnFlavorText(speciesNode["flavor_text_entries"]);
    public string field1Name => field1Node["names"][6]["name"];
    public string field2Name => field2Node["names"][6]["name"];
    public string GetTypeName(int i) => generalNode["types"][i]["type"]["name"];
    public int GetStatEffort(int i) => generalNode["stats"][i]["effort"];
    public string GetStatName(int i) => generalNode["stats"][i]["stat"]["name"];

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
