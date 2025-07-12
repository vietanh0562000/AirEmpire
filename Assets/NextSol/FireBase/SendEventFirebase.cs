using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SendEventFirebase
{

    public static bool loadingFull = false;
    //level_start
    // public static void SendEvent_level_start(int idLevel)
    // {
    //     Firebase.Analytics.FirebaseAnalytics.LogEvent("level_start", "id", idLevel);
    // }
    // //level_win
    // public static void SendEvent_level_win(int idLevel, int coin)
    // {
    //     Firebase.Analytics.FirebaseAnalytics.LogEvent("level_win", new Firebase.Analytics.Parameter[] {
    //         new Firebase.Analytics.Parameter("id",idLevel),
    //           new Firebase.Analytics.Parameter("coin",coin)
    //     });
    // }
    // //level_fail
    // public static void SendEvent_level_fail(int idLevel, int position, int coin)
    // {
    //     Firebase.Analytics.FirebaseAnalytics.LogEvent("level_fail", new Firebase.Analytics.Parameter[] {
    //         new Firebase.Analytics.Parameter("id",idLevel),
    //          new Firebase.Analytics.Parameter("position",position),
    //           new Firebase.Analytics.Parameter("coin",coin)
    //     });
    // }

    // //reward_getx5
    // public static void SendEvent_reward_getx5(int idLevel)
    // {
    //     Firebase.Analytics.FirebaseAnalytics.LogEvent("reward_getx5", "id", idLevel);
    // }

    // //reward_upgrade_speed
    // public static void SendEvent_reward_upgrade_speed(int idLevel)
    // {
    //     Firebase.Analytics.FirebaseAnalytics.LogEvent("reward_upgrade_speed", "id", idLevel);
    // }
    // //reward_upgrade_booster
    // public static void SendEvent_reward_upgrade_booster(int idLevel)
    // {
    //     Firebase.Analytics.FirebaseAnalytics.LogEvent("reward_upgrade_booster", "id", idLevel);
    // }
    // //booster_use
    // public static void SendEvent_booster_use(int idLevel)
    // {
    //     Firebase.Analytics.FirebaseAnalytics.LogEvent("booster_use", "id", idLevel);
    // }
    //speed_update
    public static void SendEvent_speed_update(/*int idLevel*/)
    {
        if (!SendEventFirebase.loadingFull) return;
        //Firebase.Analytics.FirebaseAnalytics.LogEvent("speed_update" /*"id"*//*, idLevel*/);
    } //worker_update
    public static void SendEvent_worker_update(/*int idLevel*/)
    {
        if (!SendEventFirebase.loadingFull) return;
       // Firebase.Analytics.FirebaseAnalytics.LogEvent("worker_update"/*, "id", idLevel*/);
    }
    //earning_update
    public static void SendEvent_earning_update(/*int idLevel*/)
    {
        if (!SendEventFirebase.loadingFull) return;
       // Firebase.Analytics.FirebaseAnalytics.LogEvent("earning_update"/*, "id", idLevel*/);
    }

    // ---- Event In Game ----- 
    public static void GameStart()
    {
        if (!SendEventFirebase.loadingFull) return;
      //  Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.game_start);
    }

    public static void GameExit()
    {
        if (!SendEventFirebase.loadingFull) return;
    //    Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.game_exit);
    }

    /// <summary>
    ///  Use :To understand users' habit and strategy
    ///  Trigger: When users upgrade workshop/trading house
    /// </summary>
    /// <param name="map_no">Map hien tai | default 0 </param>
    /// <param name="dockType">loai : Workshop, Trading House | default: Workshop</param>
    /// <param name="dockNo">default:0</param>
    /// <param name="upgrade_type">loai upgrade: Speed,Worker,Income |default:None</param>
    /// <param name="upgrade_level">default:1</param>
    public static void UpgradeDocksEvent(int map_no, string dockType, int dockNo, string upgrade_type, int upgrade_level)
    {
        /*if (!SendEventFirebase.loadingFull) return;
        Firebase.Analytics.Parameter[] parameters ={
            new Firebase.Analytics.Parameter(
              "map_no", map_no),
            new Firebase.Analytics.Parameter(
              "dockType", " "+dockType),
            new Firebase.Analytics.Parameter(
              "dockNo", dockNo),
            new Firebase.Analytics.Parameter(
              "upgrade_type", " "+upgrade_type),
            new Firebase.Analytics.Parameter(
              "upgrade_level", upgrade_level)
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.upgrade_docks, parameters);*/
    }

    /// <summary>
    /// First launch	With permission popup
    /// Normal launch	Without any popup, age screen
    /// </summary>
    /// <param name="action_type">First launch, Normal launch</param>
    /// <param name="time_spent">lauch game - play scene</param>
    public static void LoadingTimeEvent(string action_type, int time_spent)
    {
        /*if (!SendEventFirebase.loadingFull) return;
        Firebase.Analytics.Parameter[] parameters ={
            new Firebase.Analytics.Parameter(
              "action_type", action_type),
            new Firebase.Analytics.Parameter(
              "time_spent", ""+time_spent)
          };
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.loading_times, parameters);*/
    }

    public static void GachaPlayedEvent()
    {
        if (!SendEventFirebase.loadingFull) return;
      //  Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.upgrade_docks);
    }

    /// <summary>
    /// progression_type:
    /// Unlock new dock
    /// Unlock new map
    /// 
    /// Workshop
    /// Trading House
    /// </summary>
    /// <param name="progression_type"></param>
    /// <param name="map_no"></param>
    /// <param name="dock_type"></param>
    /// <param name="dock_no"></param>
    public static void ProgressionEvent(string progression_type, int map_no, string dock_type, int dock_no)
    {
        /*if (!SendEventFirebase.loadingFull) return;
        Firebase.Analytics.Parameter[] parameters ={
            new Firebase.Analytics.Parameter(
              "progression_type", ""+progression_type),
            new Firebase.Analytics.Parameter(
              "map_no", map_no),
            new Firebase.Analytics.Parameter(
              "dock_type", ""+dock_type),
            new Firebase.Analytics.Parameter(
              "dock_no", dock_no)
          };
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.progression, parameters);*/
    }

    public static void SessionRewardReceiveEvent()
    {
        if (!SendEventFirebase.loadingFull) return;
       // Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.session_reward_receive);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ads_type">ads_reward_received_money,ads_reward_double_speed,ads_money_offlinex2, ads_reward_double_income</param>
    /// <param name="ads_location">Offline earning popup, Main Screen, Setting, Shop</param>
    public static void ClickOnAdsEvent(string ads_type, string ads_location)
    {
        if (!SendEventFirebase.loadingFull) return;
        /*Firebase.Analytics.Parameter[] parameters ={
            new Firebase.Analytics.Parameter(
              "ads_type", ""+ads_type),
            new Firebase.Analytics.Parameter(
              "ads_location", ""+ads_location)
          };
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.click_on_ads, parameters);*/
    }

    // ---- Event Ads -----
    // Full Ads
    public static void EventFullAdsRequest()
    {
        if (!SendEventFirebase.loadingFull) return;
      //  Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.fullads_request);
    }
    public static void EventFullAdsRequestFailed()
    {
        if (!SendEventFirebase.loadingFull) return;
       // Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.fullads_request_failed);
    }
    public static void EventFullAdsRequestSuccess()
    {
        if (!SendEventFirebase.loadingFull) return;
       // Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.fullads_request_success);
    }
    public static void EventFullAdsShowReady()
    {
        if (!SendEventFirebase.loadingFull) return;
       // Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.fullads_show_ready);
    }
    public static void EventFullAdsShowNotReady()
    {
        if (!SendEventFirebase.loadingFull) return;
        //Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.fullads_show_notready);
    }
    public static void EventFullAdsShow()
    {
        if (!SendEventFirebase.loadingFull) return;
       // Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.fullads_show);
    }
    public static void EventFullAdsShowFailed()
    {
        if (!SendEventFirebase.loadingFull) return;
       // Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.fullads_show_failed);
    }
    public static void EventFullAdsFinish()
    {
        if (!SendEventFirebase.loadingFull) return;
       /// Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.fullads_finish);
    }

    // Reward Ads
    public static void EventVideoAdsShowReady()
    {
        if (!SendEventFirebase.loadingFull) return;
     //   Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.videoads_show_ready);
    }
    public static void EventVideoAdsShowNotReady()
    {
        if (!SendEventFirebase.loadingFull) return;
        //Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.videoads_show_notready);
    }
    public static void EventVideoAdsShow()
    {
        if (!SendEventFirebase.loadingFull) return;
        //Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.videoads_show);
    }
    public static void EventVideoAdsShowFailed()
    {
        if (!SendEventFirebase.loadingFull) return;
       // Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.videoads_show_failed);
    }
    public static void EventVideoAdsFinish()
    {
        if (!SendEventFirebase.loadingFull) return;
        //Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.videoads_finish);
    }
    public static void EventVideoAdsClick()
    {
        if (!SendEventFirebase.loadingFull) return;
        //Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.videoads_click);
    }
    public static void EventVideoAdsRewardReceived()
    {
        if (!SendEventFirebase.loadingFull) return;
        //Firebase.Analytics.FirebaseAnalytics.LogEvent(Event.videoads_rewardReceived);
    }
}

public class Event
{
    // ---- Event Ads : Full Ads -----
    public static readonly string fullads_request = "fullads_request";
    public static readonly string fullads_request_failed = "fullads_request_failed";
    public static readonly string fullads_request_success = "fullads_request_success";
    public static readonly string fullads_show_ready = "fullads_show_ready";
    public static readonly string fullads_show_notready = "fullads_show_notready";
    public static readonly string fullads_show = "fullads_show";
    public static readonly string fullads_show_failed = "fullads_show_failed";
    public static readonly string fullads_finish = "fullads_finish";

    // ---- Event Ads : Reward Video -----
    public static readonly string videoads_show_ready = "videoads_show_ready";
    public static readonly string videoads_show_notready = "videoads_show_notready";
    public static readonly string videoads_show = "videoads_show";
    public static readonly string videoads_show_failed = "videoads_show_failed";
    public static readonly string videoads_finish = "videoads_finish";
    public static readonly string videoads_click = "videoads_click";
    public static readonly string videoads_rewardReceived = "videoads_rewardReceived";

    // ---- Event In Game ----- 
    public static readonly string game_start = "game_start";
    public static readonly string game_exit = "game_exit";
    public static readonly string upgrade_docks = "upgrade_docks";
    public static readonly string loading_times = "loading_times";
    public static readonly string gacha_Played = "gacha_Played";
    public static readonly string progression = "progression";
    public static readonly string session_reward_receive = "session_reward_receive";
    public static readonly string click_on_ads = "click_on_ads";
}
