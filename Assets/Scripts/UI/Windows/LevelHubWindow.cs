using Assets.Scripts.Core;
using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.LevelManagment;
using Assets.Scripts.UI.Base;
using Assets.Scripts.UI.Windows.Equip;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Windows
{
    public class LevelHubWindow : Window
    {
        [SerializeField] private ImageTextButtonView levelScrollPf;
        [SerializeField] private ImageTextButtonView enemyScrollPf;
        [SerializeField] private Transform levelContent;
        [SerializeField] private Transform enemyContent;
        [SerializeField] private TMP_Text levelHeader;
        [SerializeField] private TMP_Text desc;
        //[SerializeField] private TMP_Text countDesc;
        //[SerializeField] private Image previewImg;
        [SerializeField] private Button startBtn;
        [SerializeField] private Button equipBtn;

        private uint selectedLevel;
        private UISlotSelector current;
        private Level[] Levels => Game.Library.LevelLib.Levels;
        private void Start()
        {
            startBtn.onClick.AddListener(() =>
            {
                Close();
                Game.GameManager.StartGame(selectedLevel);
            });
            equipBtn.onClick.AddListener(() =>
            {
                Game.WindowController.Open<EquipWindow>();
            });
            InitLevels();
        }

        private async void InitLevels()
        {
            bool firstSelected = false;
            foreach (var lvl in Levels)
            {
                var inst = Instantiate(levelScrollPf, levelContent);
                inst.SetText("Level " + lvl.ID.ToString());
                inst.OnClick(() => SetLevel(lvl));

                InitUISlotSeclector(inst);

                if (!firstSelected)
                {
                    firstSelected = true;
                    inst.InvokeClick();
                }
                var sprite = await lvl.GetPreviewSprite();
                inst.SetImage(sprite.First());
            }
        }

        private void InitUISlotSeclector(ImageTextButtonView inst)
        {
            var selector = inst.GetComponent<UISlotSelector>();
            if (selector != null)
            {
                inst.AddOnClick(() => 
                { 
                    selector.Select(true);
                    current?.Select(false);
                    current = selector;
                });
            }
        }

        private void SetLevel(Level lvl)
        {
            selectedLevel = lvl.ID;
            levelHeader.text = lvl.Name;
            desc.text = lvl.Description;
            //countDesc.text = "Monster count: " + lvl.SpawnPool.ToString();
            SetEnemies(lvl);
            //var sprite = await lvl.GetPreviewSprite();
            //previewImg.sprite = sprite.First();
        }

        private void SetEnemies(Level lvl)
        {
            enemyContent.RemoveAllChilds();
            var enemies = Game.Library.EnemyLib.GetDatas(lvl.EnemyIDs);
            foreach (var enemy in enemies)
            {
                var inst = Instantiate(enemyScrollPf, enemyContent);
                inst.Set(enemy.Icon, enemy.name);
                inst.OnClick(() => ShowEnemyInfo(enemy));
            }
        }

        private void ShowEnemyInfo(EnemyData enemy)
        {
            var wnd = Game.WindowController.Open<EnemyTooltipWindow>(false);
            wnd.Init(enemy);
        }
    }
}