using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Manager
{
    public class ParticleManager : MonoBehaviour
    {
        #region Singleton
        public static ParticleManager Instance { get; private set; }
        #endregion

        [SerializeField] private List<ParticleStruct> particles = new List<ParticleStruct>();

        private readonly List<DesiredParticle> particlePairs = new List<DesiredParticle>();
        private readonly List<DesiredPooledParticle> particlePooledPairs = new List<DesiredPooledParticle>();

        [System.Serializable]
        public struct ParticleStruct
        {
            public int count;
            public Particles type;
            public ParticleSystem particle;
        }

        [System.Serializable]
        public struct DesiredParticle
        {
            public Particles type;
            public ParticleSystem particle;
        }

        [System.Serializable]
        private struct DesiredPooledParticle
        {
            public Particles type;
            public Queue<ParticleSystem> ParticleSystems;
        }

        private void Awake()
        {
            Instance = this;
            Init();
        }

        public void Init()
        {
            SetParticlesPool();
        }

        private void SetParticlesPool()
        {
            foreach (ParticleStruct currentParticleStruct in particles)
            {
                switch (currentParticleStruct.count)
                {
                    case 0: return;
                    case 1:
                        {
                            ParticleSystem clone = Instantiate(currentParticleStruct.particle, transform);
                            clone.Stop();
                            ParticleSystem[] particleSystems = clone.GetComponentsInChildren<ParticleSystem>();
                            for (int i = particleSystems.Length - 1; i >= 0; i--)
                            {
                                particleSystems[i].Stop();
                            }
                            particlePairs.Add(new DesiredParticle
                            {
                                type = currentParticleStruct.type,
                                particle = clone
                            });
                            break;
                        }
                    default:
                        {
                            if (currentParticleStruct.count > 1)
                            {
                                var particleSystems = new Queue<ParticleSystem>();

                                for (var j = 0; j < currentParticleStruct.count; j++)
                                {
                                    particleSystems.Enqueue(Instantiate(currentParticleStruct.particle, transform));
                                }

                                particlePooledPairs.Add(new DesiredPooledParticle
                                {
                                    type = currentParticleStruct.type,
                                    ParticleSystems = particleSystems
                                });
                            }
                            break;
                        }
                }
            }
        }

        public ParticleSystem GetParticle(Particles particleId)
        {
            for (int i = particlePairs.Count - 1; i >= 0; i--)
            {
                if (particlePairs[i].type != particleId)
                    continue;

                return particlePairs[i].particle;
            }

            for (int i = particlePooledPairs.Count - 1; i >= 0; i--)
            {
                if (particlePooledPairs[i].type != particleId)
                    continue;

                ParticleSystem particleSystem1 = particlePooledPairs[i].ParticleSystems.Dequeue();
                particlePooledPairs[i].ParticleSystems.Enqueue(particleSystem1);
                return particleSystem1;
            }

            return null;
        }

        public ParticleSystem GetParticle(int particleId)
        {
            for (int i = particlePairs.Count - 1; i >= 0; i--)
            {
                if (((int)particlePairs[i].type) != particleId)
                {
                    continue;
                }

                return particlePairs[i].particle;
            }

            for (int i = particlePooledPairs.Count - 1; i >= 0; i--)
            {
                if (((int)particlePooledPairs[i].type) != particleId)
                    continue;

                var particle = particlePooledPairs[i].ParticleSystems.Dequeue();
                particlePooledPairs[i].ParticleSystems.Enqueue(particle);
                return particle;
            }

            return null;
        }

        public void InstantiateParticle(Vector3 pos, ParticleSystem particle)
        {
            particle.transform.position = pos;
            particle.Play();
        }

        public void InstantiateParticle(Vector3 pos, Particles particleId)
        {
            ParticleSystem particle = GetParticle(particleId);
            particle.transform.position = pos;
            particle.Play();
        }

        public void InstantiateParticle(Vector3 pos, Quaternion rot, Particles particleId)
        {
            ParticleSystem particle = GetParticle(particleId);
            particle.transform.position = pos;
            particle.transform.rotation = rot;
            particle.Play();
        }

        public void InstantiateParticle(Transform stack, ParticleSystem particle, Vector3 offset)
        {
            Transform particleTransform;
            (particleTransform = particle.transform).SetParent(stack);
            particleTransform.localPosition = Vector3.zero + offset;
            particle.Play();
        }

        public void InstantiateParticle(Transform stack, Particles particleId, Vector3 offset)
        {
            ParticleSystem particle = GetParticle(particleId);
            Transform particleTransform;
            (particleTransform = particle.transform).SetParent(stack);
            particleTransform.localPosition = Vector3.zero + offset;
            particle.Play();
        }

        public void InstantiateParticle(Transform pTransform, ParticleSystem particle)
        {
            Transform particleTransform;
            (particleTransform = particle.transform).SetParent(pTransform);
            particleTransform.localPosition = Vector3.zero;
            particle.Play();
        }

        public void InstantiateParticle(Transform pTransform, Particles particleId)
        {
            ParticleSystem particle = GetParticle(particleId);
            Transform particleTransform;
            (particleTransform = particle.transform).SetParent(pTransform);
            particleTransform.localPosition = Vector3.zero;
            particle.Play();
        }

        public void InstantiateParticle(Transform pTransform, ParticleSystem particle, Space space)
        {
            particle.transform.SetParent(pTransform);

            switch (space)
            {
                case Space.World:
                    particle.transform.position = Vector3.zero;
                    break;
                case Space.Self:
                    particle.transform.localPosition = Vector3.zero;
                    break;
                default:
                    Debug.LogWarning("Out of range", gameObject);
                    break;
            }
            particle.Play();
        }

        public void InstantiateParticle(Transform pTransform, Particles particleId, Space space)
        {
            ParticleSystem particle = GetParticle(particleId);

            particle.transform.SetParent(pTransform);

            switch (space)
            {
                case Space.World:
                    particle.transform.position = Vector3.zero;
                    break;
                case Space.Self:
                    particle.transform.localPosition = Vector3.zero;
                    break;
                default:
                    Debug.LogWarning("Out of range", gameObject);
                    break;
            }

            particle.Play();
        }

        public void InstantiateParticle(Transform pTransform, ParticleSystem particle, Vector3 offset, Space space)
        {
            particle.transform.SetParent(pTransform);

            switch (space)
            {
                case Space.World:
                    particle.transform.position = Vector3.zero + offset;
                    break;
                case Space.Self:
                    particle.transform.localPosition = Vector3.zero + offset;
                    break;
                default:
                    Debug.LogWarning("Out of range", gameObject);
                    break;
            }
            particle.Play();
        }

        public void InstantiateParticle(Transform pTransform, Particles particleId, Vector3 offset, Space space)
        {
            ParticleSystem particle = GetParticle(particleId);

            particle.transform.SetParent(pTransform);

            switch (space)
            {
                case Space.World:
                    particle.transform.position = Vector3.zero + offset;
                    break;
                case Space.Self:
                    particle.transform.localPosition = Vector3.zero + offset;
                    break;
                default:
                    Debug.LogWarning("Out of range", gameObject);
                    break;
            }
            particle.Play();
        }

        public void SetSimulationSpeed(Particles particleId, float speed)
        {
            ParticleSystem particle = GetParticle(particleId);
            ParticleSystem.MainModule main = particle.main;
            main.simulationSpeed = speed;
        }

        public void SetSimulationSpeed(ParticleSystem particle, float speed)
        {
            ParticleSystem.MainModule main = particle.main;
            main.simulationSpeed = speed;
        }

        public void StopParticle(ParticleSystem particle)
        {
            particle.Stop();
        }

        public void PlayParticle(ParticleSystem particle)
        {
            particle.Play();
        }

        /// <summary>
        /// searches among particles in the scene
        /// </summary>
        /// <param name="particleName"></param>
        /// <param name="value"></param>
        public void PlayParticle(Particles particleId)
        {
            ParticleSystem particle = GetParticle(particleId);
        }

        /// <summary>
        /// searches among particles in the scene
        /// </summary>
        /// <param name="particleName"></param>
        /// <param name="value"></param>
        public void StopParticle(Particles particleId)
        {
            ParticleSystem particle = GetParticle(particleId);
        }
    }
}