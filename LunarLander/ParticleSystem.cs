using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CS5410
{
    public class ParticleSystem
    {
        private Dictionary<long, Particle> m_particles = new Dictionary<long, Particle>();
        public Dictionary<long, Particle>.ValueCollection particles { get { return m_particles.Values; } }
        private MyRandom m_random = new MyRandom();

        private Vector2 m_center;
        private int m_sizeMean; // pixels
        private int m_sizeStdDev;   // pixels
        private float m_speedMean;  // pixels per millisecond
        private float m_speedStDev; // pixles per millisecond
        private float m_lifetimeMean; // milliseconds
        private float m_lifetimeStdDev; // milliseconds
        private float m_directionMean; // degrees
        private float m_directionStdDev; // degrees

        public ParticleSystem(Vector2 center, int sizeMean, int sizeStdDev, float speedMean, float speedStdDev, int lifetimeMean, int lifetimeStdDev, float directionMean, float directionStdDev)
        {
            m_center = center;
            m_sizeMean = sizeMean;
            m_sizeStdDev = sizeStdDev;
            m_speedMean = speedMean;
            m_speedStDev = speedStdDev;
            m_lifetimeMean = lifetimeMean;
            m_lifetimeStdDev = lifetimeStdDev;
            m_directionMean = directionMean;
            m_directionStdDev = directionStdDev;
        }

        private Particle create()
        {
            float size = (float)m_random.nextGaussian(m_sizeMean, m_sizeStdDev);
            float direction = (float)m_random.nextGaussian(m_directionMean, m_directionStdDev);
            Vector2 velocity = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));
            velocity.Normalize();
            velocity *= (float)m_random.nextGaussian(m_speedMean, m_speedStDev);

            // System.IO.File.AppendAllText("particle.txt", "size: " + size + " direction: " + direction + " velocity: " + velocity + " speed: " + velocity.Length() + " lifetime: " + (int)m_random.nextGaussian(m_lifetimeMean, m_lifetimeStdDev) + "\n");

            var p = new Particle(
                    m_center,
                    velocity,
                    velocity.Length(),
                    new Vector2(size, size),
                    new TimeSpan(0, 0, 0, 0, (int)m_random.nextGaussian(m_lifetimeMean, m_lifetimeStdDev)));
            return p;
        }
        public void Move(Vector2 center)
        {
            m_center = center;
        }

        public void Rotate(float angle)
        {
            m_directionMean = angle;
        }

        public void Update(GameTime gameTime)
        {
            // Update existing particles
            List<long> removeMe = new List<long>();
            foreach (Particle p in m_particles.Values)
            {
                if (!p.update(gameTime))
                {
                    removeMe.Add(p.name);
                }
            }
            // Remove dead particles
            foreach (long key in removeMe)
            {
                m_particles.Remove(key);
            }

            // Generate some new particles
            if (Generating)
            {
                for (int i = 0; i < 8; i++)
                {
                    var particle = create();
                    m_particles.Add(particle.name, particle);
                }
            }
        }
        public bool Generating = true;

        public bool HasParticles()
        {
            return m_particles.Count > 0;
        }
    }
}