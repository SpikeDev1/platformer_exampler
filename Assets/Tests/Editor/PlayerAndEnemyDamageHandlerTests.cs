using System;
using NUnit.Framework;
using UniRx;
using UnityEngine;

using Systems.Combat;
using Gameplay.Players;
using Gameplay.Enemies;
using Gameplay.Actors;
using Core.Utilities;
using Model.Configs;

namespace Assets.Tests.EditMode
{
    public class PlayerAndEnemyDamageHandlerTests
    {

        private class FakePlayerView : IPlayerDamageFacade
        {
            public int HurtCalls { get; private set; }
            public int DeathCalls { get; private set; }
            public int DestroyCalls { get; private set; }
            private readonly Subject<Unit> deathFinished = new Subject<Unit>();
            public IObservable<Unit> DeathFinished => deathFinished;
            public Transform Transform { get; } = new GameObject("PlayerViewStub").transform;

            public void Hurt() { HurtCalls++; }
            public void Death() { DeathCalls++; }
            public void Destroy() { DestroyCalls++; }
            public IObservable<Unit> RespawnFinished { get; }
            public void Respawn()
            {
                throw new NotImplementedException();
            }

            public void Respawn(Vector3 newPos)
            {
                throw new NotImplementedException();
            }
        }

        private class FakeAudio : IAudioPlayer
        {
            public AudioClip LastClip;
            public string LastId;
            public void PlayOneShot(AudioClip clip) { LastClip = clip; }
            public void PlayOneShot(string idAudio) { LastId = idAudio; }
        }

        // ---------------- PlayerDamageHandler ----------------

        [Test]
        public void Player_Damage_NotLethal_CallsHurt_NoDeath()
        {
            var health = new HealthAbility(new HealthConfig(){ amount = 1 });
            health.Init(5);
            var view = new FakePlayerView();
            var handler = new PlayerDamageHandler(health, view);

            int diedCount = 0;
            handler.Died.Subscribe(_ => diedCount++);
            handler.Initialize();

            handler.Damage(2);

            Assert.AreEqual(3, health.Current);
            Assert.AreEqual(1, view.HurtCalls);
            Assert.AreEqual(0, view.DeathCalls);
            Assert.AreEqual(0, diedCount);
            Assert.True(handler.IsAlive);
        }

        [Test]
        public void Player_Damage_Lethal_NoHurt_TriggersDeath()
        {
            var health = new HealthAbility(new HealthConfig() { amount = 1 });
            health.Init(2);
            var view = new FakePlayerView();
            var handler = new PlayerDamageHandler(health, view);

            int diedCount = 0;
            handler.Died.Subscribe(_ => diedCount++);
            handler.Initialize();

            handler.Damage(2);

            Assert.AreEqual(0, health.Current);
            Assert.AreEqual(0, view.HurtCalls);
            Assert.AreEqual(1, view.DeathCalls);
            Assert.AreEqual(1, diedCount);
            Assert.False(handler.IsAlive);
        }

        [Test]
        public void Player_Kill_SameAsLethalDamage()
        {
            var health = new HealthAbility(new HealthConfig() { amount = 1 });
            health.Init(4);
            var view = new FakePlayerView();
            var handler = new PlayerDamageHandler(health, view);

            int diedCount = 0;
            handler.Died.Subscribe(_ => diedCount++);
            handler.Initialize();

            handler.Kill();

            Assert.AreEqual(0, health.Current);
            Assert.AreEqual(0, view.HurtCalls);
            Assert.AreEqual(1, view.DeathCalls);
            Assert.AreEqual(1, diedCount);
            Assert.False(handler.IsAlive);
        }

        // ---------------- EnemyDamageHandler ----------------

        [Test]
        public void Enemy_Damage_NotLethal_PlaysOuch_NoDeath()
        {
            var cfg = new HealthConfig { amount = 10, ouchAudio = "enemy_ouch" };
            var health = new HealthAbility(cfg);
            health.Init(cfg.amount);
            var audio = new FakeAudio();
            var handler = new EnemyDamageHandler(health, audio, cfg);

            int diedCount = 0;
            handler.Died.Subscribe(_ => diedCount++);
            handler.Initialize();

            handler.Damage(3);

            Assert.AreEqual(7, health.Current);
            Assert.AreEqual("enemy_ouch", audio.LastId);
            Assert.AreEqual(0, diedCount);
            Assert.True(handler.IsAlive);
        }

        [Test]
        public void Enemy_Damage_Lethal_NoOuch_TriggersDeath()
        {
            var cfg = new HealthConfig { amount = 3, ouchAudio = "enemy_ouch" };
            var health = new HealthAbility(cfg);
            health.Init(cfg.amount);
            var audio = new FakeAudio();
            var handler = new EnemyDamageHandler(health, audio, cfg);

            int diedCount = 0;
            handler.Died.Subscribe(_ => diedCount++);
            handler.Initialize();

            handler.Damage(3);

            Assert.AreEqual(0, health.Current);
            Assert.IsNull(audio.LastId, "Ouch should not play on lethal hit");
            Assert.AreEqual(1, diedCount);
            Assert.False(handler.IsAlive);
        }

        [Test]
        public void Enemy_Kill_SameAsLethalDamage()
        {
            var cfg = new HealthConfig { amount = 5, ouchAudio = "enemy_ouch" };
            var health = new HealthAbility(cfg);
            health.Init(cfg.amount);
            var audio = new FakeAudio();
            var handler = new EnemyDamageHandler(health, audio, cfg);

            int diedCount = 0;
            handler.Died.Subscribe(_ => diedCount++);
            handler.Initialize();

            handler.Kill();

            Assert.AreEqual(0, health.Current);
            Assert.IsNull(audio.LastId, "Ouch should not play on kill");
            Assert.AreEqual(1, diedCount);
            Assert.False(handler.IsAlive);
        }
    }
}

