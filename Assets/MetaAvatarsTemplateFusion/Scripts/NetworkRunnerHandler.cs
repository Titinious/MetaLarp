using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Chiligames.MetaAvatarsFusion
{
    public class NetworkRunnerHandler : MonoBehaviour
    {
        NetworkRunner networkRunner;
        public event Action OnNetworkRunnerInitialized;

        private void Awake()
        {
            networkRunner = GetComponent<NetworkRunner>();
        }

        // Start is called before the first frame update
        void Start()
        {
            var client = InitializeNetworkRunner(networkRunner, GameMode.Shared, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, NetworkRunnerInitialized);
        }

        public void NetworkRunnerInitialized(NetworkRunner runner)
        {
            OnNetworkRunnerInitialized?.Invoke();
        }

        protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
        {
            var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

            if (sceneManager == null)
            {
                sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            }

            return runner.StartGame(new StartGameArgs
            {
                GameMode = gameMode,
                Address = address,
                Scene = scene,
                SessionName = "VRTest",
                Initialized = initialized,
                SceneManager = sceneManager
            });
        }

    }
}
