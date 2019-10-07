﻿using System;
using UnityEngine;
using UnityEngine.AI;

namespace Geekbrains
{
    public sealed class Bot : BaseObjectScene
    {
        public float Hp = 100;
        public Vision Vision;
        public Weapon Weapon; 
        public Transform Target { get; set; }
        public NavMeshAgent Agent { get; private set; }
        private float _waitTime = 3;
        private StateBot _stateBot;
        private Vector3 _point;

        public event Action<Bot> OnDieChange;

        private StateBot StateBot
        {
            get => _stateBot;
            set
            {
                _stateBot = value;
                switch (value)
                {
                    case StateBot.Non:
                        Color = Color.white;
                        break;
                    case StateBot.Patrol:
                        Color = Color.green;
                        break;
                    case StateBot.Inspection:
                        Color = Color.yellow;
                        break;
                    case StateBot.Detected:
                        Color = Color.red;
                        break;
                    case StateBot.Died:
                        Color = Color.gray;
                        break;
                    default:
                        Color = Color.white;
                        break;
                }

            }
        }

        protected override void Awake()
        {
            base.Awake();
            Agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            var bodyBot = GetComponentInChildren<BodyBot>();
            if (bodyBot != null) bodyBot.OnApplyDamageChange += SetDamage;

            var headBot = GetComponentInChildren<HeadBot>();
            if (headBot != null) headBot.OnApplyDamageChange += SetDamage;
        }

        private void OnDisable()
        {
            var bodyBot = GetComponentInChildren<BodyBot>();
            if (bodyBot != null) bodyBot.OnApplyDamageChange -= SetDamage;

            var headBot = GetComponentInChildren<HeadBot>();
            if (headBot != null) headBot.OnApplyDamageChange -= SetDamage;
        }
        

        public void Tick()
		{
			if (StateBot == StateBot.Died) return;

			if (StateBot != StateBot.Detected)
			{
				if (!Agent.hasPath)
				{
					if (StateBot != StateBot.Inspection)
					{
						if (StateBot != StateBot.Patrol)
						{
                            BPatrol();
                        }
						else
						{
							if (Vector3.Distance(_point, transform.position) <= 1)
							{
                                BInspection();

                            }
						}
					}
				}

				if (Vision.VisionM(transform, Target))
				{
					StateBot = StateBot.Detected;
				}
			}
			else
			{
                MovePoint(Target.position);
				Agent.stoppingDistance = 2;
				if (Vision.VisionM(transform, Target))
				{
                    Weapon.Fire();
				}
                else
                {
                    if(Vector3.Distance(_point, transform.position) <= 1)

                    {
                        BInspection();
                    }
                    else
                    {
                        BPatrol();
                    }

                }
            }
        }

        public void BPatrol()
        {
            StateBot = StateBot.Patrol;
            _point = Patrol.GenericPoint(transform);
            MovePoint(_point);
            Agent.stoppingDistance = 0;
        }

        public void BInspection()
        {
            StateBot = StateBot.Inspection;
            Invoke(nameof(ReadyPatrol), _waitTime);
        }

        public void SetDamage(InfoCollision info)
		{
			if (Hp > 0)
			{
				Hp -= info.Damage;
			}

			if (Hp <= 0)
			{
				StateBot = StateBot.Died;
				Agent.enabled = false;
				foreach (var child in GetComponentsInChildren<Transform>())
				{
					child.parent = null;
					var tempRbChild = child.GetComponent<Rigidbody>();
					if (!tempRbChild)
					{
						tempRbChild = child.gameObject.AddComponent<Rigidbody>();
					}
					
					Destroy(child.gameObject, 10);
				}

                OnDieChange?.Invoke(this);
            }
		}

		private void ReadyPatrol()
		{
			StateBot = StateBot.Non;
		}

		public void MovePoint(Vector3 point)
		{
			Agent.SetDestination(point);
		}
	}
}