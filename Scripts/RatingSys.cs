using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
public class RatingSys : MonoBehaviour
{
    float start_time;
    float time;
    bool signedAcc = false;
    public Text myName;
    public Text AllScore;
    DatabaseReference reference;
    // Start is called before the first frame update
    void Awake(){
        StartCoroutine(CheckAndFixDependencies());
    }
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    // Handle initialization of the necessary firebase modules:
    

    // Track state changes of the auth object.
    private IEnumerator CheckAndFixDependencies()
    {
        var checkAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDependenciesTask.IsCompleted);

        var dependencyResult = checkAndFixDependenciesTask.Result;
        
        if(dependencyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyResult}");
        }
    }
    void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(CheckAutoLogin());
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();
        if(user != null){
            var reloadUserTask = user.ReloadAsync();
            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);
            //AutoLogin();
            if(user != null){
                StartCoroutine(LoadUserData());
            }
            
        }
        else {
            signedAcc = false;
        }
    }

    private void AutoLogin()
    {
        if(user != null)
        {
            signedAcc = true;
        }
        else {
            signedAcc = false;
        }
        Debug.Log(signedAcc);
    }
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId + "; Name " + user.DisplayName);
            }
        }
    }
    private IEnumerator LoadUserData()
    {
        if(user != null){
            //Get the currently logged in user data
            var DBTask = reference.Child("Users").Child(user.DisplayName).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                //No data exists yet
                AllScore.text = "0";
            }
            else
            {
                //Data has been retrieved
                DataSnapshot snapshot = DBTask.Result;
                AllScore.text = snapshot.Child("AllScore").Value.ToString();
                myName.text = user.DisplayName;
                start_time = Time.time;

            }
        }
        
    }

}
