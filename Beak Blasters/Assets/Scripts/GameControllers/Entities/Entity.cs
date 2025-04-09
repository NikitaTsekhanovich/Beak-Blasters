using System;
using GameControllers.Entities.Properties;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.Entities
{
    public abstract class Entity<T> : MonoBehaviourPun, ICanInitialize<T>
    {
        private Action<Entity<T>> _returnAction;
        
        public virtual void SpawnInit(Action<Entity<T>> returnAction)
        {
            _returnAction = returnAction;
        }

        public virtual void ActiveInit(Vector3 startPosition, Quaternion startRotation)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("SyncPosition", RpcTarget.Others, startPosition, startRotation);
        }
        
        protected virtual void ReturnToPool()
        {
            _returnAction?.Invoke(this);
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer && photonView.IsMine)
                photonView.RPC("SendReturnToPool", RpcTarget.Others);
        }

        public void ChangeStateEntity(bool state)
        {
            gameObject.SetActive(state);
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("SyncStateEntity", RpcTarget.Others, state);
        }
    }
}
