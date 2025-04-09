using System;
using GameControllers.AttackEntities.Behaviors;
using GameControllers.AttackEntities.Behaviors.ChangeStateBehaviour;
using GameControllers.AttackEntities.Behaviors.FlyBehavior;
using GameControllers.Entities;
using GameControllers.Entities.Properties;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.AttackEntities.Types
{
    public class Bullet : AttackEntity<Bullet>
    {
        [SerializeField] private Transform _movePoint;
        [SerializeField] private float _speed;

        public override void SpawnInit(Action<Entity<Bullet>> returnAction)
        {
            InitBullet();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("InitBullet", RpcTarget.Others);
            
            base.SpawnInit(returnAction);
        }
        
        public override void ActiveInit(Vector3 startPosition, Quaternion startRotation)
        {
            base.ActiveInit(startPosition, startRotation);
            _changeStateBehavior.ChangeStateTimer(true);
        }
        
        protected override void DoDestroy()
        {
            _changeStateBehavior.ChangeStateTimer(false);
            base.DoDestroy();
        }

        [PunRPC]
        private void InitBullet()
        {
            _flyBehavior = new FlyPointBehaviour(transform, _movePoint, _speed);
            _changeStateBehavior = new LifeTimeBehaviour(DoDestroy, _lifeTime);
        }
        
        [PunRPC]
        private void SyncPosition(Vector3 startPosition, Quaternion startRotation)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }

        [PunRPC]
        private void SendReturnToPool()
        {
            DoDestroy();
        }
        
        [PunRPC]
        private void SyncStateEntity(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
