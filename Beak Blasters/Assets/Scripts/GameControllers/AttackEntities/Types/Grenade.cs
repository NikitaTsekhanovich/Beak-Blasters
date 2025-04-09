using System;
using DG.Tweening;
using GameControllers.AttackEntities.Behaviors.ChangeStateBehaviour;
using GameControllers.AttackEntities.Behaviors.FlyBehavior;
using GameControllers.Entities;
using Photon.Pun;
using StartSceneControllers;
using UnityEngine;

namespace GameControllers.AttackEntities.Types
{
    public class Grenade : AttackEntity<Grenade>
    {
        [SerializeField] private RadiusChecker radiusChecker;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _explsionparticle;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private AudioSource _exploundSound;
        
        private const float Speed = 0.2f;
        
        private bool _isDead;
        
        private void SetStartValues()
        {
            _spriteRenderer.enabled = true;
            _collider.enabled = true;
            _changeStateBehavior.ChangeStateTimer(true);
            _isDead = false;
        }
        
        protected override void DoDestroy()
        {
            if (_isDead) return;
            _isDead = true;
            
            DestroyAnimation();
            radiusChecker.Explode(DealDamage);
            radiusChecker.ClearColliders();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("DestroyAnimation", RpcTarget.Others);
            
            DOTween.Sequence()
                .AppendInterval(2f)
                .AppendCallback(base.DoDestroy);
        }
        
        public override void SpawnInit(Action<Entity<Grenade>> returnAction)
        {
            InitGrenade();
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("InitGrenade", RpcTarget.Others);
            
            base.SpawnInit(returnAction);
        }
        
        public override void ActiveInit(Vector3 startPosition, Quaternion startRotation)
        {
            base.ActiveInit(startPosition, startRotation);
            SetStartValues();
        }

        public void InitDirectionShoot(Vector3 directionShoot)
        {
            SyncDirectionShoot(directionShoot);
            
            if (GameModeData.ModeGame == ModeGame.Multiplayer)
                photonView.RPC("SyncDirectionShoot", RpcTarget.Others, directionShoot);
        }
        
        [PunRPC]
        private void InitGrenade()
        {
            _flyBehavior = new FlyBallisticBehaviour(transform);
            _changeStateBehavior = new LifeTimeBehaviour(DoDestroy, _lifeTime);
        }

        [PunRPC]
        private void DestroyAnimation()
        {
            _exploundSound.Play();
            _collider.enabled = false;
            _changeStateBehavior.ChangeStateTimer(false);
            _spriteRenderer.enabled = false;
            _explsionparticle.Play();
        }

        [PunRPC]
        private void SyncDirectionShoot(Vector3 directionShoot)
        {
            _flyBehavior.InitDirectionShoot(directionShoot * Speed);
        }
        
        [PunRPC]
        private void SyncPosition(Vector3 startPosition, Quaternion startRotation)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
            SetStartValues();
        }

        [PunRPC]
        private void SendReturnToPool()
        {
            ReturnToPool();
        }
        
        [PunRPC]
        private void SyncStateEntity(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
