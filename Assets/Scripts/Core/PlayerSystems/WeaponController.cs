using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Core.PlayerSystems
{
    public class WeaponController : MonoBehaviour
    {
        public Transform firePoint;
        [SerializeField] private SpriteRenderer weaponRender;

        private Camera mainCamera;
        private IWepContrlollerAttackPreset preset;
        private WeaponDrawer weaponDrawer;

        public Vector2 Target { get; private set; }
        public Dictionary<byte, IWepContrlollerAttackPreset> Presets { get; private set; } = new Dictionary<byte, IWepContrlollerAttackPreset>();
        public event Action attack;


        private void Awake()
        {
            mainCamera = Camera.main;
            weaponDrawer = new WeaponDrawer(weaponRender);
            UpdatePresets();
            WeaponSwitchSubscribe();
        }
        private void UpdatePresets()
        {
            Presets.Clear();
            weaponDrawer.Clear();
            var abItems = Game.Player.Inventory.GetActionBarItems();
            for (byte i = 0; i < abItems.Count; i++)
            {
                if (abItems[i] is Weapon wep)
                {
                    Presets.Add(i, GetPreset(wep));
                    weaponDrawer.Add(i, wep.ImageID);
                }
            }
            preset = Presets.First().Value;
            weaponDrawer.SetIcon(Presets.First().Key);
        }
        private void WeaponSwitchSubscribe()
        {
            Game.GameManager.LevelInputListener.numericKey.Subscribe(Index =>
            {
                enabled = Presets.ContainsKey(Index);
                if (enabled)
                {
                    preset = Presets[Index];
                    weaponDrawer.SetIcon(Index);
                }
            }).AddTo(this);
        }
        private void ListenReloadCommand()
        {
            if (Game.GameManager.LevelInputListener.ButtonR && preset is IWeaponReloable reloadable)
                reloadable.Reload();
        }
        private IWepContrlollerAttackPreset GetPreset(Weapon wep)
        {
            if (wep is RangedWeapon)
                return new RangedAttack(this, wep as RangedWeapon, Game.Player.Mods);
            return null;
        }
        private void Update()
        {
            UpdateTarget();
            if (preset.Attack())
            {
                attack?.Invoke();
            }
            ListenReloadCommand();
        }

        private void UpdateTarget()
        {
            Target = mainCamera.ScreenToWorldPoint(Game.GameManager.LevelInputListener.CursorPosition);
        }
    }
}