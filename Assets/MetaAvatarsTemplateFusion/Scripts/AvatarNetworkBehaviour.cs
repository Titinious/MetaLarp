using Fusion;
using UnityEngine;

namespace Chiligames.MetaAvatarsFusion
{
    public class AvatarNetworkBehaviour : NetworkBehaviour
    {
        [SerializeField] FusionMetaAvatar avatar;

        //The [Networked] attribute allows us to easily share the state of a variable across the network just by setting it.
        [Networked] public ulong oculusID { get; set; }

        //RPCs in Fusion must be called from a NetworkBehaviour
        [Rpc(RpcSources.InputAuthority, RpcTargets.Proxies, InvokeLocal = false)]
        public void RPC_RecieveStreamData(byte[] bytes)
        {
            avatar.AddToStreamDataList(bytes);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All, InvokeLocal = false)]
        public void RPC_LoadNewAvatar(string assetPath)
        {
            avatar.LoadAvatarFromRPC(assetPath);
        }
    }
}
