using CS5410;
using LunarLander;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
public class LunarParticleSystem
{
    private ParticleSystem _particleSystem;
    private ParticleSystemRenderer _particleSystemRenderer;
    public LunarParticleSystem(Game1 game)
    {
        _particleSystemRenderer = new ParticleSystemRenderer("particle");
        _particleSystemRenderer.LoadContent(game.Content);
    }
    public void ShipCrash(Vector2 center)
    {
        _particleSystem = new ParticleSystem(center, 10, 5, 0.5f, 0.1f, 1000, 100, 0, 10);
        _particleSystem.Generating = true;
        // System.IO.File.AppendAllLines("crash.txt", new string[] { "Crash at " + center.ToString() });
    }

    public void Thrust(Vector2 center, Vector2 direction)
    {
        if (_particleSystem != null)
            _particleSystem.Generating = true;
        float angle = (float)Math.Atan2(direction.Y, direction.X);
        if (_particleSystem != null)
        {
            _particleSystem.Move(center);
            _particleSystem.Rotate(angle + MathHelper.Pi);
        }
        else
        {
            _particleSystem = new ParticleSystem(center, 10, 5, 0.5f, 0.1f, 1000, 100, angle, 0.1f);
        }
    }

    public void StopThrust()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Generating = false;
            if (!_particleSystem.HasParticles())
            {
                _particleSystem = null;
            }
        }
        Thrusting = false;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        if (_particleSystem != null)
            _particleSystemRenderer.Draw(spriteBatch, _particleSystem, offset);
    }

    public void Update(GameTime gameTime)
    {
        if (_particleSystem != null)
        {
            _particleSystem.Update(gameTime);
        }
    }
    public void Reset()
    {
        _particleSystem = null;
    }
    public bool Thrusting = false;
}