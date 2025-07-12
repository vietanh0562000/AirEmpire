//using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ControlFirebase : MonoBehaviour
{
    public static ControlFirebase Instance { get; private set; }
   // Firebase.FirebaseApp app;

    // public bool _fireBaseActived = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        CheckFirebase();
    }

    void CheckFirebase()
    {
        SendEventFirebase.loadingFull = false;
        /*Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                // _fireBaseActived = true;
                SendEventFirebase.loadingFull = true;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });*/
    }

    //demo
    public void LogEvent()
    {
        /*// Log an event with no parameters.
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);

        // Log an event with a float parameter
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent("progress", "percent", 0.4f);

        // Log an event with an int parameter.
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventPostScore,
            Firebase.Analytics.FirebaseAnalytics.ParameterScore,
            42
          );

        // Log an event with a string parameter.
        Firebase.Analytics.FirebaseAnalytics
          .LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
            Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
            "spoon_welders"
          );

        // Log an event with multiple parameters, passed as a struct:
        Firebase.Analytics.Parameter[] LevelUpParameters = {
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterLevel, 5),
            new Firebase.Analytics.Parameter(
              Firebase.Analytics.FirebaseAnalytics.ParameterCharacter, "mrspoon"),
            new Firebase.Analytics.Parameter(
              "hit_accuracy", 3.14f)
          };

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
          Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
          LevelUpParameters);*/
    }
    // static class SendEventFirebase 
}

