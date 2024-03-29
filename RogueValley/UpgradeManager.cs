﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using RogueValley.Entities;
using RogueValley.Maps;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Microsoft.Xna.Framework.Input;

namespace RogueValley
{
    class UpgradeManager
    {

        protected Texture2D[][] sprites;
        protected Texture2D[][][] projectiles;
        protected UpgradeUI upgradeUI;
        public UpgradeManager() {
            this.upgradeUI = new UpgradeUI();

        }
        public void LoadContent(Texture2D[][] sprites, Texture2D[][][] projectiles) {
            this.sprites = sprites;
            this.projectiles = projectiles;
            this.upgradeUI.LoadContent(this.sprites[(int)enums.UpgradeManager.UpgradeScreen]);
        }

        public virtual SpriteBatch Draw(SpriteBatch _spriteBatch)
        {
            this.upgradeUI.Draw(_spriteBatch);

            return _spriteBatch;
        }
        public virtual Player Update(Player player, bool clicked)
        {


            return player;
        }

        public bool AfterWaveUpgrade(Player player, Game1 g1) {
            return this.upgradeUI.Update(player, g1);
        }

    }
    class WeaponSelecScreen : UpgradeManager {
        protected Rectangle[] choices;
        public WeaponSelecScreen() {

            choices = new Rectangle[2];
            choices[0] = new Rectangle(500, 500, 323, 267); 
            choices[1] = new Rectangle(1000, 500, 323, 267);

        }
        public override SpriteBatch Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(base.sprites[(int)enums.UpgradeManager.WeaponSelect][0], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(base.sprites[(int)enums.UpgradeManager.WeaponSelect][1], this.choices[0], Color.White);
            _spriteBatch.Draw(base.sprites[(int)enums.UpgradeManager.WeaponSelect][2], this.choices[1], Color.White);

            return _spriteBatch;
        }
        public override Player Update(Player player, bool clicked)
        {
            var mouseState = Mouse.GetState();
            if (clicked)
            {
                Point mousePos = new Point(mouseState.X, mouseState.Y);

                if (this.choices[(int)enums.Weapon.StandartSword].Contains(mousePos))
                {
                    player.weapon = new StandartSword(player);
                    player.weapon.LoadContent(player.pAttackSprite, player.sAttackSprite);
                }
                else if (this.choices[(int)enums.Weapon.Staff].Contains(mousePos)) {
                    player.weapon = new Staff(player);
                    player.weapon.LoadContent(player.pAttackSprite, player.sAttackSprite);
                    player.weapon.LoadStaffProjs(base.projectiles);
                }
            }
            return player;
        }

    }
}
