using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ObstacleLine2D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Song mainSound; // Песня меню

        Scrolling scrolling1;
        Scrolling scrolling2;

        enum GameState // Состояние 
        {
            MainMenu,
            SingleGame,
            MultiGame,
        }
        GameState CurrentGameState = GameState.MainMenu;

        //Кнопки меню
        cButton SingleBtn;
        cButton MultiBtn;
        cButton ExitBtn;

        //Машинка
        Texture2D Car2D;
        Rectangle CarRect;

        //Траффик
        List<Traffic> traffic = new List<Traffic>();
        Random rand = new Random();

        public Game1()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            SingleBtn = new cButton(Content.Load<Texture2D>("Content/SingleGame"), graphics.GraphicsDevice);
            SingleBtn.setPosition(new Vector2(145, 340));
            MultiBtn = new cButton(Content.Load<Texture2D>("Content/MultiGame"), graphics.GraphicsDevice);
            MultiBtn.setPosition(new Vector2(145, 480));
            ExitBtn = new cButton(Content.Load<Texture2D>("Content/ExitGame"), graphics.GraphicsDevice);
            ExitBtn.setPosition(new Vector2(145, 620));
            Car2D = Content.Load<Texture2D>("Content/car3");
            CarRect = new Rectangle(300,600,162,83);
            //Загрузка фона
            scrolling1 = new Scrolling (Content.Load<Texture2D>("Content/GameBack"), new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("Content/GameBack2"), new Rectangle(graphics.PreferredBackBufferWidth, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            //Загрузка музыки
            mainSound = Content.Load<Song>("Content/MainSound");
            if (CurrentGameState == GameState.MainMenu)
                MediaPlayer.Play(mainSound);
            /*if (CurrentGameState == GameState.SingleGame)
                MediaPlayer.Stop(mainSound);*/
        }

        protected override void UnloadContent()
        {

        }

        float spawntraffic = 0;
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                   if(SingleBtn.isCicked == true) CurrentGameState = GameState.SingleGame;
                    SingleBtn.Update(mouse);
                    if (MultiBtn.isCicked == true) CurrentGameState = GameState.MultiGame;
                    MultiBtn.Update(mouse); 
                   if (ExitBtn.isCicked == true) this.Exit();
                    ExitBtn.Update(mouse);
                    break;

                case GameState.SingleGame:
                    if (keyboard.IsKeyDown(Keys.Escape)) CurrentGameState = GameState.MainMenu;

                    //Управление машинкой
                    if (keyboard.IsKeyDown(Keys.Right))
                        CarRect.X += 8;
                    if (keyboard.IsKeyDown(Keys.Left))
                        CarRect.X -= 8;
                    if (keyboard.IsKeyDown(Keys.Up))
                        CarRect.Y -= 8;
                    if (keyboard.IsKeyDown(Keys.Down))
                        CarRect.Y += 8;

                    //Границы экрана
                    if(CarRect.X <= 0) CarRect.X = 0;
                    if (CarRect.X + Car2D.Width >= graphics.PreferredBackBufferWidth)
                        CarRect.X = graphics.PreferredBackBufferWidth - Car2D.Width;

                    if (CarRect.Y <= 0) CarRect.Y = 0;
                    if (CarRect.Y + Car2D.Height >= graphics.PreferredBackBufferHeight)
                        CarRect.Y = graphics.PreferredBackBufferHeight - Car2D.Height;

                    //Траффик
                    spawntraffic += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    foreach (Traffic trafic in traffic)
                        trafic.Update(graphics.GraphicsDevice);

                    LoadTraffic();



                    //Движение фона
                    if (scrolling1.rect.X + scrolling1.texture.Width <= 0)
                        scrolling1.rect.X = scrolling2.rect.X + scrolling2.texture.Width;
                    if (scrolling2.rect.X + scrolling2.texture.Width <= 0)
                        scrolling2.rect.X = scrolling1.rect.X + scrolling2.texture.Width;

                    scrolling1.Update();
                    scrolling2.Update();

                    break;

                case GameState.MultiGame:
                    //Движение фона
                    if (scrolling1.rect.X + scrolling1.texture.Width <= 0)
                        scrolling1.rect.X = scrolling2.rect.X + scrolling2.texture.Width;
                    if (scrolling2.rect.X + scrolling2.texture.Width <= 0)
                        scrolling2.rect.X = scrolling1.rect.X + scrolling2.texture.Width;

                    scrolling1.Update();
                    scrolling2.Update();

                    //Траффик
                    spawntraffic += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    foreach (Traffic trafic in traffic)
                        trafic.Update(graphics.GraphicsDevice);

                    LoadTraffic();

                    //Управление машинкой
                    if (keyboard.IsKeyDown(Keys.Right))
                        CarRect.X += 8;
                    if (keyboard.IsKeyDown(Keys.Left))
                        CarRect.X -= 8;
                    if (keyboard.IsKeyDown(Keys.Up))
                        CarRect.Y -= 8;
                    if (keyboard.IsKeyDown(Keys.Down))
                        CarRect.Y += 8;

                    //Границы экрана
                    if (CarRect.X <= 0) CarRect.X = 0;
                    if (CarRect.X + Car2D.Width >= graphics.PreferredBackBufferWidth)
                        CarRect.X = graphics.PreferredBackBufferWidth - Car2D.Width;

                    if (CarRect.Y <= 0) CarRect.Y = 0;
                    if (CarRect.Y + Car2D.Height >= graphics.PreferredBackBufferHeight)
                        CarRect.Y = graphics.PreferredBackBufferHeight - Car2D.Height;

                    break;

            }

            base.Update(gameTime);
        }

        public void LoadTraffic()
        {


            int randY = rand.Next(30, 600);

            if (spawntraffic >= 1)
            {
                spawntraffic = 0;
                if (traffic.Count() < 4)
                    traffic.Add(new Traffic(Content.Load<Texture2D>("Content/Mini_truck"), new Vector2(graphics.PreferredBackBufferWidth + 3, randY)));
            }

            for (int i = 0; i < traffic.Count; i++)
                if (!traffic[i].isVisible)
                {
                    traffic.RemoveAt(i);
                    i--;
                }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("Content/Background"), new Rectangle(0, 0,  graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>("Content/GameName1"), new Rectangle(150, 0, 1066, 163), Color.White);
                    SingleBtn.Draw(spriteBatch);
                    MultiBtn.Draw(spriteBatch);
                    ExitBtn.Draw(spriteBatch);

                    break;

                case GameState.SingleGame:
                    //Отрисовка фона
                    scrolling1.Draw(spriteBatch);
                    scrolling2.Draw(spriteBatch);
                    //Отрисовка траффика
                    foreach (Traffic trafic in traffic)
                        trafic.Draw(spriteBatch);
                    spriteBatch.Draw(Car2D, CarRect, Color.White);
                    break;

                case GameState.MultiGame:
                    //Отрисовка фона
                    scrolling1.Draw(spriteBatch);
                    scrolling2.Draw(spriteBatch);
                    //Отрисовка траффика
                    foreach (Traffic trafic in traffic)
                        trafic.Draw(spriteBatch);
                    spriteBatch.Draw(Car2D, CarRect, Color.White);
                    break;

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
