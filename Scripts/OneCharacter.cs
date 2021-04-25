using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OneCharacter : MonoBehaviour
{
    public Slider ProgressBar;
    public Text letter_obj;
    public Camera cam;
    string abc = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
    public GameObject finish;
    float time;
    float timeAmt = 7;

    float time_now;
    float start_time;
    int react_ms;

    int letter;
    bool status = false;
    public void Start()
    {
        letter = Random.Range(0,32);
        letter_obj.text = abc.Substring(letter, 1);
        time = timeAmt;
        start_time = Time.time;
    }
    public InputField InputF;
    public void Check()
    {
        if(InputF.text != ""){
            status = true;
            if(InputF.text.ToLower() == letter_obj.text.ToLower()){
                Debug.Log("Success! " + InputF.text);
                //cam.backgroundColor = new Vector4(0,0.5f,0,1);
                finish = Instantiate(finish);
                if(react_ms == null){
                    react_ms = 7000;
                }
                Finish.reaction = react_ms;
                finish.transform.SetParent(gameObject.transform, false);
                menu.score += (int)time * 10;
                finish.transform.GetChild(1).GetComponent<Text>().text = menu.score.ToString();
                Destroy(InputF.gameObject);
                Destroy(letter_obj);
                Destroy(ProgressBar.gameObject);
                StartCoroutine(lvl_end());
            }
            else
            {
                Fail();
            }
        }
    }
    IEnumerator lvl_end()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene ("Level2");
    }   
    void Fail()
    {
        status = true;
        finish = Instantiate(finish);
        finish.transform.SetParent(gameObject.transform, false);
        finish.transform.GetChild(0).GetComponent<Text>().text = "FAIL";
        finish.transform.GetChild(1).GetComponent<Text>().text = menu.score.ToString();
        Destroy(InputF.gameObject);
        Destroy(letter_obj);
        Destroy(ProgressBar.gameObject);
    }
    public void Update()
    {
        // if(Input.GetMouseButtonDown(0)){
        //     time_now = Time.time;
        //     react_ms = Mathf.Round((time_now - start_time)*1000f);
        // }
        if(time>0 && status == false){
            time -= Time.deltaTime;
            ProgressBar.value = time / timeAmt;

        }
        if(time <= 0 && status == false){
            Fail();
        }
    }

}
