using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo_v0._2
{
    class Creature
    {
        #region Entity Properties
        private static int populationCap = 100;
        private Vector2 pPosition;
        public Vector2 Position
        {
            get { return pPosition; }
        }
        
        private double pAngle;
        public double Angle
        {
            get { return Math.Round(pAngle, 5); }

        }
        public float AngleAsFloat
        {
            get { return Convert.ToSingle(this.Angle); }
        }

        private int pSize = 8;
        public int Size
        {
            get { return pSize; }
        }
        public Vector2 Origin
        {
            get { return new Vector2(pPosition.X + (pSize / 2), pPosition.Y + (pSize / 2)); }
        }

        private int pSpeed;
        public int Speed
        {
            get { return pSpeed; }
        }

        private Vector2 pVelocity;
        private Vector2 pDirection;

        private Texture2D pTexture;
        public Texture2D Texture
        {
            get { return pTexture; }
        }
        private Color pColor;
        public Color Color
        {
            get { return pColor; }
        }

        private bool pCarnivore;
        private Food pFoodTarget;
        public Food FoodTarget
        {
            get { return pFoodTarget; }
        }
        private Creature pCreatureTarget;
        public Creature CreatureTarget
        {
            get { return pCreatureTarget; }
        }

        private Worker.Circle pSenses;
        private Rectangle pCollider;
        private double pStomach;
        #endregion
        #region Creature Properties
        private string pName;

        private double pHunger;
        private int pSenseRange;
        private int pChaseLimit = 2;
        private double pChaseTimer = 0;

        //Reproduction
        private int pGestationLength;
        private int pFertilityPeriod;
        private double pEggInterval = 1.5;
        private double pEggCountdown = 0;
        private double pCurrentFertility = 0;

        //Age
        private double pAge = 0;
        private int pLongevity;
        private int pGeneration = 0;
        public int Generation
        {
            get { return pGeneration; }
        }
        #endregion

        #region Constructors
        public Creature(Point gameSpaceLimit, GraphicsDevice graphicsDevice)
        {
            pPosition  = new Vector2(Worker.StaticRandom.Instance.Next(pSize, gameSpaceLimit.X - pSize), Worker.StaticRandom.Instance.Next(pSize, gameSpaceLimit.Y - pSize));
            pSpeed = Worker.StaticRandom.Instance.Next(10, 50);
            pAngle = Math.Asin(pPosition.X / pPosition.Length()) * (180 / Math.PI);
            pVelocity = new Vector2(pSpeed, pSpeed);

            pSenseRange = Worker.StaticRandom.Instance.Next(pSize, 20);
            pSenses = new Worker.Circle(Origin, pSenseRange);
            pCollider = new Rectangle(Position.ToPoint(), new Point(pSize));
            pHunger = Worker.StaticRandom.Instance.Next(50, 150);
            pStomach = pHunger;
            pGestationLength = Worker.StaticRandom.Instance.Next(20, 70);
            
            pLongevity = Worker.StaticRandom.Instance.Next(70, 140);
            pFertilityPeriod = Worker.StaticRandom.Instance.Next(10,20)*10;
            if (Worker.StaticRandom.Instance.Next(10) < 3)
            {
                pCarnivore = true;
                pColor = new Color(255, pSpeed,0);
                pName += "C" + pColor.G.ToString();
            }
            else
            {
                pCarnivore = false;
                pColor = new Color(0,pSpeed, 255);
                pName += "H" + pColor.G.ToString();
            }

            Color[] colorData = new Color[pSize * pSize];

            for (int i = 0; i < pSize * pSize; i++)
            {
                colorData[i] = pColor;
            }
            pTexture = new Texture2D(graphicsDevice, pSize, pSize);
            pTexture.SetData<Color>(colorData);
        } //initial creation

        public Creature(Vector2 Pos, Point gameSpaceLimit, GraphicsDevice graphicsDevice, string[] parentData)
        {
            int random;
            pPosition = Pos;
            random = Worker.StaticRandom.Instance.Next(100);
            if (random > 2)
            {
                pSpeed = int.Parse(parentData[0]);
            }
            else
            {
                int modifier = Worker.StaticRandom.Instance.Next(1,3);
                if (modifier == 2)
                {
                    modifier = -1;
                }
                pSpeed = int.Parse(parentData[0])+(modifier*10);
                if (pSpeed < 0) pSpeed = 0;
            }
            
            pAngle = Math.Asin(pPosition.X / pPosition.Length()) * (180 / Math.PI);
            pVelocity = new Vector2(pSpeed, pSpeed);

            random = Worker.StaticRandom.Instance.Next(100);
            if (random > 2)
            {
                pHunger = int.Parse(parentData[4]);
            }
            else
            {
                pHunger = int.Parse(parentData[0]) + (Worker.StaticRandom.Instance.Next(-1, 1) * 10);
                if (pHunger < 0) pHunger = 0;
            }
            pStomach = pHunger;

            pSenseRange = int.Parse(parentData[5]);
            random = Worker.StaticRandom.Instance.Next(100);
            if (random > 2)
            {
                pSenseRange = int.Parse(parentData[4]);
            }
            else
            {
                pSenseRange = int.Parse(parentData[0]) + (Worker.StaticRandom.Instance.Next(-1, 1) * 10);
                if (pSenseRange < 0) pSenseRange = 0;
            }
            pSenses = new Worker.Circle(Origin, pSenseRange);
            pCollider = new Rectangle(Position.ToPoint(), new Point(pSize));

            random = Worker.StaticRandom.Instance.Next(100);
            if (random > 2)
            {
                pGestationLength = int.Parse(parentData[6]);
            }
            else
            {
                pGestationLength = int.Parse(parentData[0]) + (Worker.StaticRandom.Instance.Next(-1, 1) * 10);
                if (pGestationLength < 0) pGestationLength = 0;
            }

            random = Worker.StaticRandom.Instance.Next(100);
            if (random > 2)
            {
                pFertilityPeriod = int.Parse(parentData[7]);
            }
            else
            {
                pFertilityPeriod = int.Parse(parentData[0]) + (Worker.StaticRandom.Instance.Next(-1, 1) * 10);
                if (pFertilityPeriod < 0) pFertilityPeriod = 0;
            }

            random = Worker.StaticRandom.Instance.Next(100);
            if (random > 2)
            {
                pLongevity = int.Parse(parentData[8]);
            }
            else
            {
                pLongevity = int.Parse(parentData[0]) + (Worker.StaticRandom.Instance.Next(-1, 1) * 10);
                if (pLongevity < 0) pLongevity = 0;
            }
            pLongevity = int.Parse(parentData[8]);

            random = Worker.StaticRandom.Instance.Next(100);
            if (random > 2)
            {
                pCarnivore = bool.Parse(parentData[10]);
            }
            else
            {
                pCarnivore = !bool.Parse(parentData[10]);
            }

            if (pCarnivore == true)
            {
                pColor = new Color(255, pSpeed, 0);
                pName += "C" + pSpeed;
            }
            else
            {
                pColor = new Color(0, pSpeed, 255);
                pName += "H" + pSpeed;
            }

            Color[] colorData = new Color[pSize * pSize];

            for (int i = 0; i < pSize * pSize; i++)
            {
                colorData[i] = pColor;
            }
            pTexture = new Texture2D(graphicsDevice, pSize, pSize);
            pTexture.SetData<Color>(colorData);
            pGeneration = int.Parse(parentData[9]) + 1;
            pName += "-" + pGeneration;
        } //subsequent creation
        #endregion
        public void UpdateCreature(GameTime Time, Point gameSpaceLimit, GraphicsDevice graphicsDevice,List<Food> Foods, List<Creature> Creatures, List<Egg> Eggs)
        {
            #region Age
            pAge += Time.ElapsedGameTime.TotalSeconds;
            if (pAge >= pLongevity)
            {
                this.Death(Creatures, "Age");
            }
            #endregion
            #region Movement
            float elapsed = (float)Time.ElapsedGameTime.TotalSeconds;


            #region Herbivore Movement

            if (pCarnivore == false)
            {
                Creature closestCarnivore = null;
                foreach (Creature c in Creatures)
                {
                    if (c.pCarnivore == true)
                    {
                        if (pSenses.Contains(c.Position))
                        {
                            if (closestCarnivore == null)
                            {
                                closestCarnivore = c;
                            }
                            else
                            {
                                if (Vector2.Distance(pPosition, c.Position) < Vector2.Distance(pPosition, closestCarnivore.Position))
                                {
                                    closestCarnivore = c;
                                }
                            }
                        }
                    }
                }
                if (closestCarnivore != null)
                {
                    pFoodTarget = null;
                    Vector2 start = pPosition;
                    Vector2 end = closestCarnivore.Position;
                    pDirection = Vector2.Normalize(end - start);
                    pAngle = Math.Asin(pDirection.X / pDirection.Length()) * (180 / Math.PI);

                    pPosition.X += pDirection.X * -Math.Abs(pVelocity.X) * elapsed;
                    pPosition.Y += pDirection.Y * -Math.Abs(pVelocity.Y) * elapsed;
                }
                else
                {
                    if (!Foods.Contains(pFoodTarget))
                    {
                        pFoodTarget = null;
                    }

                    if (pFoodTarget == null)
                    {
                        pFoodTarget = findFoodTarget(Foods);
                    }

                    if (pFoodTarget != null)
                    {
                        Vector2 start = pPosition;
                        Vector2 end = pFoodTarget.Position;
                        pDirection = Vector2.Normalize(end - start);
                        pAngle = Math.Asin(pDirection.X / pDirection.Length()) * (180 / Math.PI);
                        float distance = Vector2.Distance(start, end);

                        pPosition.X += pDirection.X * Math.Abs(pVelocity.X) * elapsed;
                        pPosition.Y += pDirection.Y * Math.Abs(pVelocity.Y) * elapsed;
                    }
                    else
                    {
                        pPosition.X += pVelocity.X * (float)Math.Sin(pAngle) * elapsed;
                        pPosition.Y += pVelocity.Y * (float)Math.Cos(pAngle) * elapsed;
                    }
                }
            }
            #endregion
            else
            {
                if (!Creatures.Contains(pCreatureTarget))
                {
                    pCreatureTarget = null;
                    pChaseTimer = 0;
                }

                if (pCreatureTarget == null)
                {
                    pCreatureTarget = findCreatureTarget(Creatures);
                }

                if (pCreatureTarget != null && (pChaseTimer >= pChaseLimit|| pCreatureTarget.CreatureTarget == this))
                {
                    pCreatureTarget = null;
                    pChaseTimer = 0;
                }

                if (pCreatureTarget != null)
                {
                    pChaseTimer += Time.ElapsedGameTime.TotalSeconds;
                    Vector2 start = pPosition;
                    Vector2 end = pCreatureTarget.pPosition;
                    if (start != end)
                    {
                        pDirection = Vector2.Normalize(end - start);
                        pAngle = Math.Asin(pDirection.X / pDirection.Length()) * (180 / Math.PI);

                        pPosition.X += pDirection.X * Math.Abs(pVelocity.X) * elapsed;
                        pPosition.Y += pDirection.Y * Math.Abs(pVelocity.Y) * elapsed;
                    }
                }
                else
                {
                    pPosition.X += pVelocity.X * (float)Math.Sin(pAngle) * elapsed;
                    pPosition.Y += pVelocity.Y * (float)Math.Cos(pAngle) * elapsed;
                }
            }

            if (pPosition.X < 0)
            {
                pPosition.X = 1;
                pVelocity.X = -pVelocity.X;
            }

            if (pPosition.Y < 0)
            {
                pPosition.Y = 1;
                pVelocity.Y = -pVelocity.Y;
            }
            if (pPosition.X > gameSpaceLimit.X - pSize)
            {
                pPosition.X = gameSpaceLimit.X - pSize;
                pVelocity.X = -pVelocity.X;
            }
            if (pPosition.Y > gameSpaceLimit.Y - pSize)
            {
                pPosition.Y = gameSpaceLimit.Y - pSize;
                pVelocity.Y = -pVelocity.Y;
            }

            pCollider = new Rectangle(pPosition.ToPoint(), new Point(pSize));
            pSenses = new Worker.Circle(Origin, pSenseRange);
            #endregion
            #region Food
            pStomach -= (pSpeed/10) * elapsed;
            if (pStomach <= 0)
            {
                Death(Creatures,"Starvation: "+Foods.Count.ToString());
            }

            #endregion
            #region Pregnancy
            pEggCountdown += Time.ElapsedGameTime.TotalSeconds;
            pCurrentFertility += eat(Foods, Creatures);
            if (pEggCountdown >= pEggInterval)
            {
                if (pCurrentFertility >= pFertilityPeriod && (Creatures.Count + Eggs.Count) < populationCap)
                {
                    Eggs.Add(new Egg(pPosition, pGestationLength, this.getAttributes(), graphicsDevice));
                    Debug.Print("Egg Laid: "+pName);
                    pCurrentFertility -= pFertilityPeriod;
                }
                pEggCountdown -= pEggInterval;
            }

            #endregion

            if (double.IsNaN(pPosition.X))
            {
                this.Death(Creatures, "Position Error");
            }

        }

        public string[] getAttributes()
        {
            string[] attributes = new string[12];
            //Speed
            attributes[0] = this.pSpeed.ToString();
            //Colour
            attributes[1] = this.pColor.R.ToString();
            attributes[2] = this.pColor.G.ToString();
            attributes[3] = this.pColor.B.ToString();
            //Creature Properties
            attributes[4] = this.pHunger.ToString();
            attributes[5] = this.pSenseRange.ToString();
            attributes[6] = this.pGestationLength.ToString();
            attributes[7] = this.pFertilityPeriod.ToString();
            attributes[8] = this.pLongevity.ToString();
            attributes[9] = this.pGeneration.ToString();
            attributes[10] = this.pCarnivore.ToString();
            attributes[11] = this.pName;
            return attributes;
        }

        private double eat(List<Food> Foods, List<Creature> Creatures)
        {
            double foodValue = 0;
            if (pCarnivore == false)
            {
                for (int i = Foods.Count - 1; i >= 0; i--)
                {
                    if (pCollider.Contains(Foods[i].Position))
                    {
                        if (Foods[i] == pFoodTarget)
                            {
                                pFoodTarget = null;
                                pChaseTimer = 0;
                            }
                            Foods.Remove(Foods[i]);
                            foodValue = 15;
                            pStomach += foodValue;
                        }
                        else
                        {
                            pCreatureTarget = null;
                            pChaseTimer = 0;
                        }
                }
            }
            else
            {
                for (int i = Creatures.Count - 1; i >= 0; i--)
                {
                    if (Creatures[i]!= this && pCollider.Intersects(Creatures[i].pCollider))
                    {
                        if(Creatures[i].pCarnivore == true)
                        {
                            if (pSpeed >= Creatures[i].Speed)
                            {
                                if (Creatures[i] == pCreatureTarget)
                                {
                                    pCreatureTarget = null;
                                    pChaseTimer = 0;
                                }
                                foodValue = Creatures[i].pStomach/3;
                                pStomach += foodValue;
                                Debug.Print("Eaten: "+ Creatures[i].pName);
                                Creatures.Remove(Creatures[i]);
                            }
                        }
                        else
                        {
                            if (Creatures[i] == pCreatureTarget)
                            {
                                pCreatureTarget = null;
                                pChaseTimer = 0;
                            }
                            foodValue = Creatures[i].pStomach/3;
                            pStomach += foodValue;
                            Debug.Print("Eaten: "+ Creatures[i].pName);
                            Creatures.Remove(Creatures[i]);
                        }

                        
                    }
                }
            }
            return foodValue;
        }

        private Food findFoodTarget(List<Food> Foods)
        {
            Food FoodTarget=null;
            foreach (Food f in Foods)
            {
                if (pSenses.Contains(f.Position))
                {
                    if (FoodTarget == null)
                    {
                        FoodTarget = f;
                    }
                    else
                    {
                        if(((f.Position - Origin).Length() <= (FoodTarget.Position - Origin).Length()))
                        {
                            FoodTarget = f;
                        }
                    }
                }
            }
            return FoodTarget;
        }

        private Creature findCreatureTarget(List<Creature> Creatures)
        {
            Creature CreatureTarget = null;
            foreach (Creature c in Creatures)
            {
                if (pSenses.Contains(c.Position) && c != this)
                {
                    if (CreatureTarget == null)
                    {
                        CreatureTarget = c;
                    }
                    else
                    {
                        if (((c.Position - Origin).Length() <= (CreatureTarget.Position - Origin).Length()))
                        {
                            CreatureTarget = c;
                        }
                    }
                }
            }
            return CreatureTarget;
        }

        private void Death(List<Creature> Creatures,string Reason)
        {
            Creatures.Remove(this);
            Debug.Print("Death - "+pName+": " +Reason);
        }

    }
}
