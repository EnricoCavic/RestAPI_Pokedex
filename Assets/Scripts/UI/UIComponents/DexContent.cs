using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DexContent : MonoBehaviour
{
    RectTransform contentTransform;
    public GameObject dexObjectPrefab;
    public Vector2 dexObjectSize;
    public int totalDexOptions = 10;
    public float spacing = 5f;

    float maxInRow;
    float finalHeight;

    private void Awake() 
    {
        contentTransform = GetComponent<RectTransform>();
    }

    private void Start() 
    {
        StartCoroutine(SetupCoroutine());    
    }

    IEnumerator SetupCoroutine()
    {
        yield return new WaitForEndOfFrame();
        maxInRow = Mathf.FloorToInt(contentTransform.rect.size.x / (dexObjectSize.x + spacing));
        finalHeight = Mathf.CeilToInt(totalDexOptions/maxInRow) * (dexObjectSize.y + spacing) + spacing;
        Debug.Log((totalDexOptions+1)/maxInRow);
        Vector2 novoTamanho = contentTransform.sizeDelta;
        novoTamanho.y = finalHeight;
        contentTransform.sizeDelta = novoTamanho;

        Vector2 currentPosition = new Vector2(spacing, -spacing);

        for (int i = 1; i <= totalDexOptions; i++)
        {
            GameObject obj = Instantiate(dexObjectPrefab, transform);
            RectTransform rt = obj.GetComponent<RectTransform>();
            DexObjectComponent dexObj = obj.GetComponent<DexObjectComponent>();
            dexObj.SetNumber(i);
            rt.localPosition = currentPosition;

            if(i % maxInRow == 0)
            {
                currentPosition.x = spacing;
                currentPosition.y -= spacing + dexObjectSize.y; 
            }
            else
            {
                currentPosition.x += spacing + dexObjectSize.x;
            }

            yield return new WaitForEndOfFrame();

        }


        
    }



}
