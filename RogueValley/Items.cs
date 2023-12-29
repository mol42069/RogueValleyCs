using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueValley.Entities;

namespace RogueValley
{

    class Item {
    
    
    }
    
    
    
    
    class Weapon:Item
    {
        public int damage, reach, AttackCooldown, pAttackTimer, pAttackTimerMax, sAttackTimer, maxTarget;
        protected int animationCount, animationTimer, animationTimerMax, AttackCooldownMax, sAttackTimerMax;
        protected float secondaryMulti, piercing;

        protected Texture2D[][] pAttackSprite, sAttackSprite;


        public Weapon() {
        }

        public void LoadContent(Texture2D[][] pAttackSprite, Texture2D[][] sAttackSprite) {
            this.pAttackSprite = pAttackSprite;
            this.sAttackSprite = sAttackSprite;
        }

        public void Animation() { 

        }
        public virtual void PrimaryAttack(List<Enemies> ene, Player player)
        {

        }
        public virtual void SecondaryAttack(List<Enemies> ene, Player player) {
        
        }
        public void ResetAnimation() {

            this.pAttackTimer = 0;
            this.sAttackTimer = 0;
            this.AttackCooldown = 0;

        }


    }
    class StandartSword:Weapon {
        public StandartSword() {
            
            base.damage = 100;
            base.piercing = 5.0f;
            base.reach = 200;
            base.maxTarget = 5;
            base.secondaryMulti = 2.5f;

            // Animation Variables

            base.animationCount = 0;
            base.animationTimer = 0;
            base.animationTimerMax = 5;

            base.AttackCooldown = 0;
            base.AttackCooldownMax = 10;

            base.pAttackTimer = 0;
            base.pAttackTimerMax = 3;

            base.sAttackTimer = 0;
            base.sAttackTimerMax = 5;



        }

        public override void PrimaryAttack(List<Enemies> ene, Player player)
        {
            if (base.AttackCooldown == 0)
            {
                base.pAttackTimer++;
                if (base.pAttackTimer >= base.pAttackTimerMax * (base.pAttackSprite[player.playerDirection].Length - 1))
                {
                    base.pAttackTimer = 0;
                    // we want to attack all enemies in the list e:
                    // but maximum this.maxtarget ammount.
                    if (ene.Count < base.maxTarget)
                    {
                        for (int i = 0; i < ene.Count; i++)
                        {
                            ene[i].TakeDamage(base.damage, base.piercing);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < base.maxTarget; i++)
                        {
                            ene[i].TakeDamage(base.damage, base.piercing);
                        }
                    }
                    base.AttackCooldown = base.AttackCooldownMax;
                }
                if (base.pAttackTimer % base.pAttackTimerMax == 0)
                {
                    player.playerSprite = base.pAttackSprite[player.playerDirection][(int)(base.pAttackTimer / base.pAttackTimerMax)];
                }
                return;
            }
            base.AttackCooldown--;
        }

        public override void SecondaryAttack(List<Enemies> e, Player player)
        {
            if (base.AttackCooldown == 0)
            {
                base.sAttackTimer++;
                if (base.sAttackTimer >= base.sAttackTimerMax * (base.sAttackSprite[player.playerDirection].Length - 1))
                {
                    base.sAttackTimer = 0;
                    // we want to attack all enemies in the list e:
                    // but maximum this.maxtarget ammount.
                    if (e.Count < base.maxTarget)
                    {
                        for (int i = 0; i < e.Count; i++)
                        {
                            e[i].TakeDamage((int)((float)base.damage * base.secondaryMulti), this.piercing);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < base.maxTarget; i++)
                        {
                            e[i].TakeDamage((int)((float)base.damage * base.secondaryMulti), base.piercing);
                        }
                    }
                    base.AttackCooldown = base.AttackCooldownMax;
                }
                if (base.sAttackTimer % base.sAttackTimerMax == 0)
                {
                    player.playerSprite = base.sAttackSprite[player.playerDirection][(int)(base.sAttackTimer / base.sAttackTimerMax)];
                }
                return;
            }
        }
    }

    class Staff : Weapon
    {
        public Staff()
        {

            base.damage = 100;
            base.piercing = 5.0f;
            base.reach = 200;
            base.maxTarget = 5;
            base.secondaryMulti = 2.5f;

            // Animation Variables

            base.animationCount = 0;
            base.animationTimer = 0;
            base.animationTimerMax = 5;

            base.AttackCooldown = 0;
            base.AttackCooldownMax = 10;

            base.pAttackTimer = 0;
            base.pAttackTimerMax = 3;

            base.sAttackTimer = 0;
            base.sAttackTimerMax = 5;



        }

        // INSTEAD OF THESE ATTACKS WE DO THE SAME AS WITH MAGES BUT ON SECONDARY ATTACK WE SEND AN EXPLOSTION TO THE CURSOR OTHERWISE WE CAST FLAME-
        // BALLS TO A RANDOM ENEMY
        public override void PrimaryAttack(List<Enemies> ene, Player player)
        {
            if (base.AttackCooldown == 0)
            {
                base.pAttackTimer++;
                if (base.pAttackTimer >= base.pAttackTimerMax * (base.pAttackSprite[player.playerDirection].Length - 1))
                {
                    base.pAttackTimer = 0;
                    // we want to attack all enemies in the list e:
                    // but maximum this.maxtarget ammount.
                    if (ene.Count < base.maxTarget)
                    {
                        for (int i = 0; i < ene.Count; i++)
                        {
                            ene[i].TakeDamage(base.damage, base.piercing);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < base.maxTarget; i++)
                        {
                            ene[i].TakeDamage(base.damage, base.piercing);
                        }
                    }
                    base.AttackCooldown = base.AttackCooldownMax;
                }
                if (base.pAttackTimer % base.pAttackTimerMax == 0)
                {
                    player.playerSprite = base.pAttackSprite[player.playerDirection][(int)(base.pAttackTimer / base.pAttackTimerMax)];
                }
                return;
            }
            base.AttackCooldown--;
        }

        public override void SecondaryAttack(List<Enemies> e, Player player)
        {
            if (base.AttackCooldown == 0)
            {
                base.sAttackTimer++;
                if (base.sAttackTimer >= base.sAttackTimerMax * (base.sAttackSprite[player.playerDirection].Length - 1))
                {
                    base.sAttackTimer = 0;
                    // we want to attack all enemies in the list e:
                    // but maximum this.maxtarget ammount.
                    if (e.Count < base.maxTarget)
                    {
                        for (int i = 0; i < e.Count; i++)
                        {
                            e[i].TakeDamage((int)((float)base.damage * base.secondaryMulti), this.piercing);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < base.maxTarget; i++)
                        {
                            e[i].TakeDamage((int)((float)base.damage * base.secondaryMulti), base.piercing);
                        }
                    }
                    base.AttackCooldown = base.AttackCooldownMax;
                }
                if (base.sAttackTimer % base.sAttackTimerMax == 0)
                {
                    player.playerSprite = base.sAttackSprite[player.playerDirection][(int)(base.sAttackTimer / base.sAttackTimerMax)];
                }
                return;
            }
        }
    }
}
