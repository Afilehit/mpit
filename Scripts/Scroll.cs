using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class Scroll : MonoBehaviour, IPointerDownHandler
{
    public Camera cam;
    public GameObject content;
    public Slider ProgressBar;
    public GameObject ScrBar;
    public GameObject folder;
    public GameObject finish;
    string[] folders = menu.folders;
    bool status = false;
    float time;
    float timeAmt = 4;
    // Start is called before the first frame update
    void Start()
    {
        time = timeAmt;
        for (int t = 0; t < folders.Length; t++ )
        {
            string tmp = folders[t];
            int r = Random.Range(t, folders.Length);
            folders[t] = folders[r];
            folders[r] = tmp;
        }
        for(var i = 0; i < Random.Range(30,40); i++){
            folder = Instantiate(folder);
            folder.transform.GetChild(1).GetComponent<Text>().text = folders[i];
            folder.transform.SetParent(content.transform, false);
        }

    }
    public void Check(){
        if(ScrBar.GetComponent<Scrollbar>().value <= 0.03 && status == false){
            status = true;
            finish = Instantiate(finish);
            finish.transform.SetParent(gameObject.transform, false);
            menu.score += (int)(ProgressBar.value * 100);
            finish.transform.GetChild(1).GetComponent<Text>().text = menu.score.ToString();
            //Destroy(ProgressBar.gameObject);
            //Destroy(content.gameObject);
            StartCoroutine(lvl_end());
        }
    }
    void Fail()
    {
        status = true;
        finish = Instantiate(finish);
        finish.transform.SetParent(gameObject.transform, false);
        finish.transform.GetChild(0).GetComponent<Text>().text = "FAIL";
        finish.transform.GetChild(1).GetComponent<Text>().text = menu.score.ToString();
        //Destroy(ProgressBar.gameObject);
        //Destroy(content.gameObject);
    }
    IEnumerator lvl_end()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene ("Level4");
    } 
    // public void Select(int numObj){
    //     content.transform.GetChild(numObj).GetComponent<Image>().color = new Vector4(0.7882f, 0.9529f, 1f, 0.7f);
    // }
    public void Unselect(int numObj){
        for(var i = 0; i < content.transform.childCount; i++){
            if(i != numObj){
                content.transform.GetChild(i).GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 0f);
            }
            
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.tag == "FileSys_Obj" && Input.GetKey(KeyCode.LeftControl)){
            eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>().color = new Vector4(0.7882f, 0.9529f, 1f, 0.7f);
        }
        else if(eventData.pointerCurrentRaycast.gameObject.tag == "FileSys_Obj"){
            eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>().color = new Vector4(0.7882f, 0.9529f, 1f, 0.7f);
            Unselect(eventData.pointerCurrentRaycast.gameObject.transform.GetSiblingIndex());
        }
        else if(eventData.pointerCurrentRaycast.gameObject.tag == "FileSys_Child" && Input.GetKey(KeyCode.LeftControl)){
            eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.GetComponent<Image>().color = new Vector4(0.7882f, 0.9529f, 1f, 0.7f);
            
        }
        else if(eventData.pointerCurrentRaycast.gameObject.tag == "FileSys_Child"){
            eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.GetComponent<Image>().color = new Vector4(0.7882f, 0.9529f, 1f, 0.7f);
            Unselect(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.transform.GetSiblingIndex());
        }
        else{
            Debug.Log(true);
            for(var i = 0; i < content.transform.childCount; i++){
                content.transform.GetChild(i).GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 0f);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(ProgressBar.value);
        if(time>0 && status == false){
            time -= Time.deltaTime;
            ProgressBar.value = time / timeAmt;

        }
        if(time <= 0 && status == false){
            Fail();
        }
    }
}
