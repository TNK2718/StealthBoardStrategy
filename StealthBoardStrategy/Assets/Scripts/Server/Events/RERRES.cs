using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// RaiseEventのラッパー.
///
/// イベントの足し方
/// ① enumを追加
/// ② Actionを追加
/// ③ RaiseEvent受信時のイベントをenumに従って振り分け
/// ④ RESに送信メソッドを追加
/// </summary>

// RaiseEventReceiver
public class RER : SingletonMonoBehaviour<RER>
{
    #region lifecycle

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    #endregion


    // ①
    // eventCode. 0~199。0は特殊な扱いのため1から始める
    public enum RaiseEventType : byte
    {
        SampleEvent = 1,
        Connected,
        Action,
        TrunEnd,
        Disconnected
    }

    // ②
    public Action<string> OnSampleEvent;

    // ③
    public void OnEvent(EventData photonEvent)
    {
        var type = (RaiseEventType) Enum.ToObject(typeof(RaiseEventType), photonEvent.Code);
        Debug.Log("RaiseEvent Received. Type = " + type);
        switch (type)
        {
            case RaiseEventType.SampleEvent:
                OnSampleEvent?.Invoke(photonEvent.CustomData as string);
                break;
            default:
                return;
        }
    }

    public void SetSubscriber(){
        
    }
}


// RaiseEventSender
public static class RES
{
    // ④
    public static void SendSampleEvent(string message)
    {
        var raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache,
        };
        PhotonNetwork.RaiseEvent((byte) RER.RaiseEventType.SampleEvent, message, raiseEventOptions, SendOptions.SendReliable);
    }
}