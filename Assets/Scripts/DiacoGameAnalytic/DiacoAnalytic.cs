using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
namespace Diaco.GameAnalytic
{
    public class DiacoAnalytic : MonoBehaviour, IGameAnalyticsATTListener
    {
        public static DiacoAnalytic instance;
        void Start()
        {
            if (!instance)
                instance = this;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GameAnalytics.RequestTrackingAuthorization(this);
            }
            else
            {
                GameAnalytics.Initialize();
            }

        }
        public void GameAnalyticsATTListenerAuthorized()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerDenied()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerNotDetermined()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerRestricted()
        {
            GameAnalytics.Initialize();
        }

        public void GA_TotalFlowPlayed(float record)
        {
            GameAnalytics.NewDesignEvent("ShopFruit:TotalFlowPlayed", record);

        }
        public void GA_TotalRecord(float record)
        {
            GameAnalytics.NewDesignEvent("ShopFruit:TotalRecord", record);

        }
        public void GA_LoseLevel()
        {
            GameAnalytics.NewDesignEvent("ShopFruit:Lose");

        }
        public void GA_HealthFinish()
        {
            GameAnalytics.NewDesignEvent("ShopFruit:HealthFinish");

        }
        public void GA_StartLevel()
        {
            GameAnalytics.NewDesignEvent("ShopFruit:Start");

        }

    }
}