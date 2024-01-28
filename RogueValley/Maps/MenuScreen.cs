using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RogueValley.enums;
using RogueValley.Entities;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace RogueValley.Maps
{
    internal class MenuScreen
    {
        Texture2D[][][] sprites, projectiles;
        Rectangle[][] buttons;
        int[][][] buttonPos;
        int[] playerPos, screenSize, bgSize, startPos;

        int initalGameState, hoverTimer;
        public Player player;
        Map[] map;
        bool inited;
        bool[][] hover;

        public MenuScreen(int[] screenSize) {

            this.startPos = new int[] { 1280, 750 };

            this.inited = false;
            this.screenSize = screenSize;
            this.bgSize = new int[] {4000, 2000};
            this.hoverTimer = 100;
            this.playerPos = null;
            // 350, 100...... 1825,


        }

        public void LoadConent(Texture2D[][][] sprites, Texture2D[][][] projectiles) {
            this.sprites = sprites;
            this.projectiles = projectiles;
        }
        public int Update(int gameState, int[] movement)
        {
            this.initalGameState = gameState;
            if (!this.inited) 
            {
                this.init();
                this.inited = true;            
            }
            if (this.playerPos == null)
            {
                this.playerPos = this.player.playerPosition;
                this.player.playerPosition = this.startPos;
                this.hoverTimer = 100;
            }


            switch (gameState) 
            {

                case 0:
                    this.player.Movement(movement, this.map[(int)enums.MenuSprite.StartS]);
                    this.player.Update(this.map[(int)enums.MenuSprite.WeaponC]);
                    gameState = UpdateStartScreen(this.player);
                    break;

                case 2:
                    this.player.Movement(movement, this.map[(int)enums.MenuSprite.WeaponC]);
                    this.player.Update(this.map[(int)enums.MenuSprite.WeaponC]);
                    gameState = UpdateWeaponScreen(this.player);
                    break;
                case 3:
                    this.player.Movement(movement, this.map[(int)enums.MenuSprite.Upgrade]);
                    this.player.Update(this.map[(int)enums.MenuSprite.Upgrade]);
                    gameState = UpdateUpgradeScreen(this.player);
                    break;

                default:
                    break;
            }

            if (this.initalGameState != gameState) 
            {
                this.player.playerPosition = this.playerPos;
                this.playerPos = null;
                this.hoverTimer = 100;
            }

            return gameState;
        }
        private void init()
        {
            this.hover = new bool[this.sprites.Length][];
            for (int i = 0; i < this.sprites.Length; i++) {
                this.hover[i] = new bool[this.sprites[i].Length - 1];
                for (int j = 0; j < this.sprites[i].Length - 1; j++) {
                    this.hover[i][j] = false;
                }
            }
            this.map = new Map[this.sprites.Length];
            for (int i = 0; i < this.sprites.Length; i++) {
                this.map[i] = new Map(this.sprites[i][0][0], this.screenSize, this.bgSize);
            }
            this.buttons = new Rectangle[this.sprites.Length][];
            this.buttonPos = new int[this.sprites.Length][][];

            // Start-Screen-Button-Rectangles:

            this.buttons[(int)enums.MenuSprite.StartS] = new Rectangle[this.sprites[(int)enums.MenuSprite.StartS].Length - 1];
            this.buttonPos[(int)enums.MenuSprite.StartS] = new int[this.sprites[(int)enums.MenuSprite.StartS].Length - 1][];

            this.buttons[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Start - 1] = new Rectangle(950, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Start - 1] = new int[] {950, 1300};
            this.buttons[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Quit - 1] = new Rectangle(1750, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Quit - 1] = new int[] { 1750, 1300 };
            this.buttons[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Record - 1] = new Rectangle(2550, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Record - 1] = new int[] { 2550, 1300 };

            // Upgrade-Screen-Button-Rectangles:

            this.buttons[(int)enums.MenuSprite.Upgrade] = new Rectangle[this.sprites[(int)enums.MenuSprite.Upgrade].Length - 1];
            this.buttonPos[(int)enums.MenuSprite.Upgrade] = new int[this.sprites[(int)enums.MenuSprite.Upgrade].Length - 1][];

            this.buttons[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.damage - 1] = new Rectangle(700, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.damage - 1] = new int[] { 700, 1300 };
            this.buttons[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.defence - 1] = new Rectangle(1400, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.defence - 1] = new int[] { 1400, 1300 };
            this.buttons[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.reach - 1] = new Rectangle(2100, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.reach - 1] = new int[] { 2100, 1300 };
            this.buttons[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.speed - 1] = new Rectangle(2800, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.speed - 1] = new int[] { 2800, 1300 };

            // WeaponChoice-Screen-Button-Rectangles:

            this.buttons[(int)enums.MenuSprite.WeaponC] = new Rectangle[this.sprites[(int)enums.MenuSprite.WeaponC].Length - 1];
            this.buttonPos[(int)enums.MenuSprite.WeaponC] = new int[this.sprites[(int)enums.MenuSprite.WeaponC].Length - 1][];

            this.buttons[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Staff - 1] = new Rectangle(1250, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Staff - 1] = new int[] { 1250, 1300 };
            this.buttons[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Sword - 1] = new Rectangle(2250, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Sword - 1] = new int[] { 2250, 1300 };

            // Pause-Screen-Button-Rectangles:

            this.buttons[(int)enums.MenuSprite.Pause] = new Rectangle[this.sprites[(int)enums.MenuSprite.Pause].Length - 1];
            this.buttonPos[(int)enums.MenuSprite.Pause] = new int[this.sprites[(int)enums.MenuSprite.Pause].Length - 1][];

            this.buttons[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Continue - 1] = new Rectangle(950, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Continue - 1] = new int[] { 950, 1300 };
            this.buttons[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Home - 1] = new Rectangle(1750, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Home - 1] = new int[] { 1750, 1300 };
            this.buttons[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Quit - 1] = new Rectangle(2550, 1300, 500, 500);
            this.buttonPos[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Quit - 1] = new int[] { 2550, 1300 };

        }

        public void Draw(SpriteBatch _spriteBatch, int gameState)
        {
            switch (gameState)
            {
                case 0:
                    DrawStartScreen(_spriteBatch);
                    break;

                case 2:
                    DrawWeaponScreen(_spriteBatch);
                    break;

                case 3:
                    DrawUpgradeScreen(_spriteBatch);
                    break;

                default:
                    break;
            }

            _spriteBatch.Draw(this.player.playerSprite, new Rectangle(this.player.drawPosition[0], this.player.drawPosition[1], 100, 100), Color.White);

        }


        private Rectangle[] UpdateRectanglePos(Rectangle[] rec, enums.MenuSprite m, int[][] pos) 
        {
            for (int i = 0; i < rec.Length; i++) {
                rec[i].X = this.map[(int)m].CalcdrawPosX(pos[i][0]);
                rec[i].Y = this.map[(int)m].CalcdrawPosY(pos[i][1]);
            }

            return rec;        
        }

        private bool[] CheckHover(bool[] hover, Rectangle[] rec, Player player)
        {
            Point bottomPoint = new Point();
            bottomPoint.X = player.drawPosition[0] + 50;
            bottomPoint.Y = player.drawPosition[1] + 85;
            int check = hover.Length;

            for (int i = 0; i < rec.Length; i++) 
            {
                if (rec[i].X < bottomPoint.X && rec[i].X + rec[i].Width > bottomPoint.X && rec[i].Y < bottomPoint.Y && rec[i].Y + rec[i].Height > bottomPoint.Y)
                {
                    hover[i] = true;
                }
                else
                {
                    hover[i] = false;
                }
                if (hover[i])
                {
                    if (this.hoverTimer > 0)
                        this.hoverTimer--;
                    else
                        this.hoverTimer = 0;
                }
                else
                    check--;
                if (check <= 0)
                    this.hoverTimer = 100;
            }
            return hover;
        }

        private int UpdateStartScreen(Player player)
        {
            this.buttons[(int)enums.MenuSprite.StartS] = this.UpdateRectanglePos(this.buttons[(int)enums.MenuSprite.StartS], enums.MenuSprite.StartS, this.buttonPos[(int)enums.MenuSprite.StartS]);
            this.map[(int)enums.MenuSprite.StartS].Update(this.player);

            this.hover[(int)enums.MenuSprite.StartS] = this.CheckHover(this.hover[(int)enums.MenuSprite.StartS], this.buttons[(int)enums.MenuSprite.StartS], player);

            if (this.hover[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Start - 1] && this.hoverTimer == 0)
                return 2;

            else if (this.hover[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Quit - 1] && this.hoverTimer == 0)
                return 0;

            else if (this.hover[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Record - 1] && this.hoverTimer == 0)
                return 0;               // TODO: here we need to go to the record page.

            return 0;
        }
        private int UpdateWeaponScreen(Player player)
        {
            this.buttons[(int)enums.MenuSprite.WeaponC] = this.UpdateRectanglePos(this.buttons[(int)enums.MenuSprite.WeaponC], enums.MenuSprite.WeaponC, this.buttonPos[(int)enums.MenuSprite.WeaponC]);
            this.map[(int)enums.MenuSprite.WeaponC].Update(this.player);

            this.hover[(int)enums.MenuSprite.WeaponC] = this.CheckHover(this.hover[(int)enums.MenuSprite.WeaponC], this.buttons[(int)enums.MenuSprite.WeaponC], player);

            if (this.hover[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Staff - 1] && this.hoverTimer == 0)
            {
                player.weapon = new Staff(player);
                player.weapon.LoadContent(player.pAttackSprite, player.sAttackSprite);
                player.weapon.LoadStaffProjs(this.projectiles); ;
                return 1;
            }

            else if (this.hover[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Sword - 1] && this.hoverTimer == 0)
            {
                player.weapon = new StandartSword(player);
                player.weapon.LoadContent(player.pAttackSprite, player.sAttackSprite);
                return 2;
            }

            return 2;
        }
        private int UpdateUpgradeScreen(Player player)
        {
            this.buttons[(int)enums.MenuSprite.Upgrade] = this.UpdateRectanglePos(this.buttons[(int)enums.MenuSprite.Upgrade], enums.MenuSprite.Upgrade, this.buttonPos[(int)enums.MenuSprite.Upgrade]);
            this.map[(int)enums.MenuSprite.Upgrade].Update(this.player);

            return 3;
        }

        private int UpdatePauseScreen(Player player)
        {
            this.buttons[(int)enums.MenuSprite.Pause] = this.UpdateRectanglePos(this.buttons[(int)enums.MenuSprite.Pause], enums.MenuSprite.Pause, this.buttonPos[(int)enums.MenuSprite.Pause]);
            this.map[(int)enums.MenuSprite.Pause].Update(this.player);

            return 4;
        }
        private void DrawStartScreen(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.map[(int)enums.MenuSprite.StartS].get_map(), new Rectangle(this.map[(int)enums.MenuSprite.StartS].map_position[0], this.map[(int)enums.MenuSprite.StartS].map_position[1], 4000, 2000), Color.White);

            for (int i = 1; i < this.sprites[(int)enums.MenuSprite.StartS].Length; i++) {
                int x = 0;
                if (this.hover[(int)enums.MenuSprite.StartS][i - 1])
                    x = 1;
                _spriteBatch.Draw(this.sprites[(int)enums.MenuSprite.StartS][i][x], this.buttons[(int)enums.MenuSprite.StartS][i - 1], Color.White);
            }
            return;
        }

        private void DrawWeaponScreen(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.map[(int)enums.MenuSprite.WeaponC].get_map(), new Rectangle(this.map[(int)enums.MenuSprite.WeaponC].map_position[0], this.map[(int)enums.MenuSprite.WeaponC].map_position[1], 4000, 2000), Color.White);

            for (int i = 1; i < this.sprites[(int)enums.MenuSprite.WeaponC].Length; i++)
            {
                int x = 0;
                if (this.hover[(int)enums.MenuSprite.WeaponC][i - 1])
                    x = 1;
                _spriteBatch.Draw(this.sprites[(int)enums.MenuSprite.WeaponC][i][x], this.buttons[(int)enums.MenuSprite.WeaponC][i - 1], Color.White);
            }
        }

        private void DrawUpgradeScreen(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.map[(int)enums.MenuSprite.Upgrade].get_map(), new Rectangle(this.map[(int)enums.MenuSprite.Upgrade].map_position[0], this.map[(int)enums.MenuSprite.Upgrade].map_position[1], 4000, 2000), Color.White);

            for (int i = 1; i < this.sprites[(int)enums.MenuSprite.Upgrade].Length; i++)
            {
                int x = 0;
                if (this.hover[(int)enums.MenuSprite.Upgrade][i - 1])
                    x = 1;
                _spriteBatch.Draw(this.sprites[(int)enums.MenuSprite.Upgrade][i][x], this.buttons[(int)enums.MenuSprite.Upgrade][i - 1], Color.White);
            }
        }

        private void DrawPauseScreen(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.map[(int)enums.MenuSprite.Pause].get_map(), new Rectangle(this.map[(int)enums.MenuSprite.Pause].map_position[0], this.map[(int)enums.MenuSprite.Pause].map_position[1], 4000, 2000), Color.White);

            for (int i = 1; i < this.sprites[(int)enums.MenuSprite.Pause].Length; i++)
            {
                int x = 0;
                if (this.hover[(int)enums.MenuSprite.Pause][i - 1])
                    x = 1;
                _spriteBatch.Draw(this.sprites[(int)enums.MenuSprite.Pause][i][x], this.buttons[(int)enums.MenuSprite.Pause][i - 1], Color.White);
            }
        }
    }
}
