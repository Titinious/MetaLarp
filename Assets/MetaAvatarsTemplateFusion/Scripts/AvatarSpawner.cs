using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Oculus.Platform;
using Oculus.Avatar2;

namespace Chiligames.MetaAvatarsFusion
{
    public class AvatarSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        public AvatarNetworkBehaviour avatarPrefab;
        public AvatarNetworkBehaviour avatarTestPrefab;
        public NetworkObject speakerPrefab;

        [SerializeField] Transform[] spawnPoints;

        private ulong userID = 0;
        private bool userIsEntitled = false;

        [HideInInspector] public NetworkRunner _runner;
        [SerializeField] NetworkRunnerHandler runnerHandler;
        [SerializeField] Transform cameraRig;
        [SerializeField] Transform centerEyeAnchor;

        public event Action<OvrAvatarEntity> OnLocalAvatarLoaded;

        private bool runnerInitialized = false;
        private bool sceneLoaded = false;

        [Tooltip("Enable this to use predefined avatars, doesn't require Oculus authentication")]
        [SerializeField] bool noOculusCredentials = false;

        void Awake()
        {
            //For testing without credentials in editor.
            if (noOculusCredentials)
            {
                CreateAvatarEntity();
                return;
            }
            //Initialize the oculus platform
            try
            {
                Core.AsyncInitialize();
                Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
            }
            catch (UnityException e)
            {           
                Debug.LogError("Platform failed to initialize due to exception.");
                Debug.LogException(e);
            }
        }

        private void Start()
        {
            runnerHandler.OnNetworkRunnerInitialized += () => runnerInitialized = true;
        }

        void EntitlementCallback(Message msg)
        {
            if (msg.IsError)
            {
                Debug.LogError("You are NOT entitled to use this app. Please check if you added the correct ID's and credentials in Oculus>Platform");                
                //UnityEngine.Application.Quit();
            }
            else
            {
                Debug.Log("You are entitled to use this app.");
                GetToken();
            }
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            _runner = runner;
            Debug.Log("OnConnectedToServer");
            SetPositionFromPlayerNumner(runner);
        }

        private void SetPositionFromPlayerNumner(NetworkRunner runner)
        {
            if (runner.LocalPlayer.PlayerId < spawnPoints.Length)
            {
                cameraRig.position = spawnPoints[runner.LocalPlayer.PlayerId].position;
                cameraRig.rotation = spawnPoints[runner.LocalPlayer.PlayerId].rotation;
            }
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        //Get Access token and user ID from Oculus Platform
        private void GetToken()
        {
            Users.GetAccessToken().OnComplete(message =>
            {
                if (!message.IsError)
                {
                    OvrAvatarEntitlement.SetAccessToken(message.Data);
                    Users.GetLoggedInUser().OnComplete(message =>
                    {
                        if (!message.IsError)
                        {
                            userID = message.Data.ID;
                            userIsEntitled = true;
                            CreateAvatarEntity();
                        }
                        else
                        {
                            var e = message.GetError();
                        }
                    });
                }
                else
                {
                    var e = message.GetError();
                }
            });
        }

        public void CreateAvatarEntity()
        {
            StartCoroutine(WaitForEntitlementCheckAndSpawn());
        }

        //Wait for all the entitlements and the runner to be ready to spawn
        IEnumerator WaitForEntitlementCheckAndSpawn()
        {
            AvatarNetworkBehaviour avatar;
            if (noOculusCredentials)
            {
                while (!runnerInitialized || !sceneLoaded)
                {
                    yield return null;
                }
                avatar = _runner.Spawn(avatarTestPrefab, cameraRig.position, cameraRig.rotation, _runner.LocalPlayer);
            }
            else
            {
                //Oculus entitlement
                while (!userIsEntitled || !OvrAvatarEntitlement.AccessTokenIsValid() || _runner == null || !runnerInitialized || !sceneLoaded)
                {
                    yield return null;
                }
                avatar = _runner.Spawn(avatarPrefab, cameraRig.position, cameraRig.rotation, _runner.LocalPlayer);
                var obj = _runner.Spawn(speakerPrefab, centerEyeAnchor.position, centerEyeAnchor.rotation, _runner.LocalPlayer);
                obj.transform.SetParent(centerEyeAnchor.transform);
                var lipSync = FindObjectOfType<OvrAvatarLipSyncContext>();
                lipSync.CaptureAudio = true;
                avatar.GetComponent<FusionMetaAvatar>().SetLipSync(lipSync);
            }

            //Avatar spawning
            FusionMetaAvatar avatarEntity = avatar.GetComponentInChildren<FusionMetaAvatar>();
            //Set avatar position and parent
            avatar.transform.SetParent(cameraRig);
            avatar.transform.localRotation = Quaternion.identity;
            avatar.transform.localPosition = Vector3.zero;

            avatarEntity.OnUserAvatarLoadedEvent.AddListener(avatarEntity => OnLocalAvatarLoaded?.Invoke(avatarEntity));

            //If not using credentials, avoid setting userID in the avatar's network behaviour
            if (noOculusCredentials)
            {
                yield break;
            }
            //Set the oculusID in the networkBehaviour so other users can access it to load our avatar
            avatarEntity.networkBehaviour.oculusID = (ulong)userID;
        }

        public void OnInput(NetworkRunner runner, NetworkInput input){}

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            Debug.Log("OnConnectFailed");
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            Debug.Log("OnConnectRequest");
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            Debug.Log("OnDisconnectedFromServer");
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            Debug.Log("OnSceneLoadDone");
            sceneLoaded = true;
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log("OnShutdown");
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }
    }
}
