using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class TapByOrder : MonoBehaviour, IPointerDownHandler
{
    public Camera cam;
    public GameObject content;
    public Slider ProgressBar;
    public GameObject ScrBar;
    public GameObject folder;
    public GameObject finish;
    public Text title;
    string[] folders = menu.folders;
    bool doubletap = false;
    bool status = false;
    
    float time;
    float timeAmt = 20;
    float ButtonCooler = 0.5f; // Half a second before reset
    int ButtonCount = 0;
    int start_folder;
    int started_folder;
    int last_folder;
    int need_folder;
    int prev_last_folder;
    int cnt = 3;
    // Start is called before the first frame update
    void mixFolders()
    {
        for (int t = 0; t < folders.Length; t++ )
        {
            string tmp = folders[t];
            int r = Random.Range(t, folders.Length);
            folders[t] = folders[r];
            folders[r] = tmp;
        }
    }
    void Folder_Create()
    {
        start_folder = 0;
        last_folder = Random.Range(6,12);
        mixFolders();
        need_folder = Random.Range(start_folder, last_folder);
        title.text = "Откройте папку " + folders[need_folder];
        
        for(var i = start_folder; i < last_folder; i++){
            folder = Instantiate(folder);
            folder.transform.GetChild(1).GetComponent<Text>().text = folders[i];
            folder.transform.SetParent(content.transform, false);
            folder.name = "Folder("+i.ToString()+")";
        }
        cnt -= 1;
    }
    void Start()
    {

        time = timeAmt;
        mixFolders();
        
        Folder_Create();
        prev_last_folder = last_folder;
    }
    public void Check(int num_obj){
        Debug.Log(need_folder);
        Debug.Log(num_obj);
        if(cnt <= 0){
            status = true;
            finish = Instantiate(finish);
            finish.transform.SetParent(gameObject.transform, false);
            menu.score += (int)(ProgressBar.value * 175);
            finish.transform.GetChild(1).GetComponent<Text>().text = menu.score.ToString();
            StartCoroutine(lvl_end());
        }
        else if(need_folder == num_obj){
            prev_last_folder = last_folder;
            Folder_Create();
            for(var i = 0; i < prev_last_folder; i++){
                GameObject.Destroy(content.transform.GetChild(i).gameObject);
            }

        }
        else {
            Fail();
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
        SceneManager.LoadScene ("Level1");
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
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.tag);
        if(eventData.pointerCurrentRaycast.gameObject.tag == "FileSys_Obj" && Input.GetKey(KeyCode.LeftControl)){
            eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>().color = new Vector4(0.7882f, 0.9529f, 1f, 0.7f);
        }
        else if(eventData.pointerCurrentRaycast.gameObject.tag == "FileSys_Obj"){
            eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>().color = new Vector4(0.7882f, 0.9529f, 1f, 0.7f);
            if ( ButtonCooler > 0 && ButtonCount == 1/*Number of Taps you want Minus One*/){
                //Has double tapped
                Debug.Log("Double Tapped");
                Check(eventData.pointerCurrentRaycast.gameObject.transform.GetSiblingIndex());
                
                // foreach (Transform child in content.transform) {
                //     GameObject.Destroy(child.gameObject);
                // }
                
                doubletap = true;
                
            } else{
                ButtonCooler = 0.5f ; 
                ButtonCount += 1 ;
                doubletap = false;
            }
            Unselect(eventData.pointerCurrentRaycast.gameObject.transform.GetSiblingIndex());
        }
        else if(eventData.pointerCurrentRaycast.gameObject.tag == "FileSys_Child" && Input.GetKey(KeyCode.LeftControl)){
            eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.GetComponent<Image>().color = new Vector4(0.7882f, 0.9529f, 1f, 0.7f);
            
        }
        else if(eventData.pointerCurrentRaycast.gameObject.tag == "FileSys_Child"){
            if ( ButtonCooler > 0 && ButtonCount == 1/*Number of Taps you want Minus One*/){
                //Has double tapped
                Debug.Log("Double Tapped");
                Check(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.transform.GetSiblingIndex());
                
                // foreach (Transform child in content.transform) {
                //     GameObject.Destroy(child.gameObject);
                // }
                
                doubletap = true;
                
            } else{
                ButtonCooler = 0.5f ; 
                ButtonCount += 1 ;
                doubletap = false;
            }
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
        // if(Input.GetMouseButtonDown(0)){
            
        // }
    
        if ( ButtonCooler > 0 ){
            ButtonCooler -= 1 * Time.deltaTime ;
        } else{
            ButtonCount = 0;
        }

        if(time>0 && status == false){
            time -= Time.deltaTime;
            ProgressBar.value = time / timeAmt;

        }
        if(time <= 0 && status == false){
            Fail();
        }
    }
}
