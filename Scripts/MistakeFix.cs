using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MistakeFix : MonoBehaviour
{
    public string[] words;
    public string[] bad_symbols;
    public Slider ProgressBar;

    string word;
    bool status = false;
    public Text title;
    public InputField InputF;
    // Start is called before the first frame update
    float time;
    float timeAmt = 12;
    public GameObject finish;

    void Start()
    {
        Debug.Log(menu.score);
        word = words[Random.Range(0,29)];
        title.text = title.text + word;
        InputF.text += word.Insert(Random.Range(0, word.Length), bad_symbols[Random.Range(0,9)]);
        time = timeAmt;
    }

    // Update is called once per frame
    public void Check()
    {
        if(InputF.text == word){
            status = true;
            Debug.Log("Success! " + InputF.text);
            //cam.backgroundColor = new Vector4(0,0.5f,0,1);
            finish = Instantiate(finish);
            finish.transform.SetParent(gameObject.transform, false);
            menu.score += (int)time * 10;
            finish.transform.GetChild(1).GetComponent<Text>().text = menu.score.ToString();
            Destroy(InputF.gameObject);
            Destroy(ProgressBar.gameObject);
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
        Destroy(InputF.gameObject);
        Destroy(ProgressBar.gameObject);
    }
    IEnumerator lvl_end()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene ("Level3");
    }   
    void Update()
    {
        if(time>0 && status == false){
            time -= Time.deltaTime;
            ProgressBar.value = time / timeAmt;

        }
        if(time <= 0 && status == false){
            Fail();
        }
    }
}
