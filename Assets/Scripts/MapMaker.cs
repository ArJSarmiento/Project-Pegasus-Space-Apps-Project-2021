using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    public List<GameObject> Prefabs;
    public LevelCreator levelCreator;
    public GameObject parentMap;
    private GameObject createObject;

    // 맵 정보 이용해서 맵 만듬.
    void Awake()
    {
        float currentX = -9.5f;
        float currentY = 8.5f;
        foreach(ElementTypes et in levelCreator.level)
        {
            if(et.ToString() == "Empty")
            {
                currentX += 1;
                if (levelCreator.level.Count == 400)
                {
                    if(currentX > 9.5f)
                    {
                        currentX = -9.5f;
                        currentY -= 1;
                    }    
                }
                else
                {
                    if(currentX > 19.5f)
                    {
                        currentX = -9.5f;
                        currentY -= 1;
                    }       
                }
             
                continue;
            }
            foreach(GameObject ob in Prefabs)
            {
                if(et.ToString() == ob.name)
                {
                    if (ob.name == "Baba")
                    {
                        GameObject baba = GameObject.Find("Baba");
                        baba.transform.position = new Vector3(currentX, currentY, 0);
                    }
                    else
                    {
                        createObject = Instantiate(ob, new Vector3(currentX, currentY, 0), Quaternion.identity);
                        createObject.transform.parent = parentMap.transform;                        
                    }
                }
            }
            currentX += 1;
            if (levelCreator.level.Count == 400)
            {
                if(currentX > 9.5f)
                {
                    currentX = -9.5f;
                    currentY -= 1;
                }                
            }
            else
            {
                if(currentX > 19.5f)
                {
                    currentX = -9.5f;
                    currentY -= 1;
                }   
            }

        }
    }
}
