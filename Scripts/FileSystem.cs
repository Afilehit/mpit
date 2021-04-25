using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class FileSystem : MonoBehaviour
    , IPointerClickHandler
    , IPointerEnterHandler
    , IPointerExitHandler
{
    private bool selected = false;
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        //selected = true;
        //gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Scroll>().Select(gameObject.transform.GetSiblingIndex());
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(GetComponent<Image>().color != new Color(0.7882f, 0.9529f, 1f, 0.7f)){
            GetComponent<Image>().color = new Vector4(0.7882f, 0.9529f, 1f, 0.3921f);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(GetComponent<Image>().color != new Color(0.7882f, 0.9529f, 1f, 0.7f)){
            GetComponent<Image>().color = new Vector4(1f,1f,1f,0f);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
