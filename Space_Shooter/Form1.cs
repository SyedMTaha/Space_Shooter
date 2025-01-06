using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Space_Shooter
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootgMedia;
        WindowsMediaPlayer explosion;


        PictureBox[] enemiesMunition;
        int enemiesMunitionSpeed;

        PictureBox[] stars;
        int playerSpeed;

        PictureBox[] munitions;
        int MunitionSpeed;

        PictureBox[] enemies;
        int enemiSpeed;

        int backgroundspeed;
        Random rnd;

        int score;
        int level;
        int difficulty;
        bool pause;
        bool gameIsOver;
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            pause = false;
            gameIsOver = false;
            score = 0;
            level = 1;
            difficulty = 9 ;

            backgroundspeed = 4;
            playerSpeed = 8;
            enemiSpeed = 4;
            MunitionSpeed = 20;
            enemiesMunitionSpeed = 4;

            munitions = new PictureBox[3];

            //load images
            Image munition = Image.FromFile(@"asserts\munition.png");

            Image enemi1 = Image.FromFile(@"asserts\E1.png");
            Image enemi2 = Image.FromFile(@"asserts\E2.png");
            Image enemi3 = Image.FromFile(@"asserts\E3.png");
            Image boss1 = Image.FromFile(@"asserts\Boss1.png");
            Image boss2 = Image.FromFile(@"asserts\Boss2.png");

            enemies = new PictureBox[10];    

            //Initialze EnemiesPictureBoxes
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40,40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point( (i + 1) * 50 , -50);
            }

            enemies[0].Image = boss1;
            enemies[1].Image = enemi2;
            enemies[2].Image = enemi3;
            enemies[3].Image = enemi3;
            enemies[4].Image = enemi1;
            enemies[5].Image = enemi3;
            enemies[6].Image = enemi2;
            enemies[7].Image = enemi3;
            enemies[8].Image = enemi2;
            enemies[9].Image = boss2;



            for (int i = 0; i< munitions.Length; i ++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8,8);
                munitions[i].Image = munition;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;    
                this.Controls.Add(munitions[i]);    
            }

            //Create WMP
            gameMedia = new WindowsMediaPlayer();
            shootgMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            //load all songs
            gameMedia.URL = "songs\\GameSong.mp3";
            shootgMedia.URL = "songs\\shoot.mp3";
            explosion.URL = "songs\\boom.mp3";

            //setup song settings
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 5;
            shootgMedia.settings.volume = 1;
            explosion.settings.volume = 6;
                        
            stars= new PictureBox[15];

            rnd = new Random();

            for (int i = 0; i < stars.Length ; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point( rnd.Next(20,580), rnd.Next(-10,400) );
                if(i % 2 == 1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;

                }
                else
                {
                    stars[i].Size = new Size(3,3);
                    stars[i].BackColor = Color.DarkGray;

                }

                this.Controls.Add( stars[i] );
            }

            //Enemies Munition
            enemiesMunition = new PictureBox[10];
            for(int i = 0;i < enemiesMunition.Length ; i++)
            {
                enemiesMunition[i] = new PictureBox();
                enemiesMunition[i].Size = new Size(2,25); 
                enemiesMunition[i].Visible = false;
                enemiesMunition[i].BackColor= Color.Yellow;
                int x = rnd.Next(0, 10);
                enemiesMunition[i].Location= new Point(enemies[x].Location.X, enemies[x].Location.Y -20);
                this.Controls.Add(enemiesMunition[i]);  

            }    


            gameMedia.controls.play();
            

        }
        private void MoveBgTimer_Tick_1(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length/2 ; i++)
            {
                stars[i].Top += backgroundspeed;

                if (stars[i].Top >= this.Height )
                {
                    stars[i].Top = -stars[i].Height;   
                }
            }

            for(int i = stars.Length / 2 ; i < stars.Length; i++)
            {
                stars[i].Top += backgroundspeed-2;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
        }

        //Movement Funcitons
        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if(Player.Left > 10)
            {
                Player.Left -= playerSpeed;
            }
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580)
            {
                Player.Left += playerSpeed;
            }
        }

        private void DownMoveTime_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
            {
                Player.Top += playerSpeed;
            }
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
            {
                Player.Top -= playerSpeed;
            }
        }

     
        //Player Movement
        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {

            if (!pause)
            {

                if (e.KeyCode == Keys.Right)
                {
                    RightMoveTimer.Start();
                }

                if (e.KeyCode == Keys.Left)
                {
                    LeftMoveTimer.Start();
                }

                if (e.KeyCode == Keys.Down)
                {
                    DownMoveTimer.Start();
                }

                if (e.KeyCode == Keys.Up)
                {
                    UpMoveTimer.Start();
                }
            }
        }

        private void Form1_KeyUp_1(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"Key Up: {e.KeyCode}"); // Debug message1
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            DownMoveTimer.Stop();
            UpMoveTimer.Stop();

            if(e.KeyCode == Keys.Space)
            {
                if(!gameIsOver)
                {
                    if (pause)
                    {
                        StartTimers();
                        label1.Visible = false;
                        gameMedia.controls.play();
                        pause = false;
                    }
                    else
                    {
                        label1.Location = new Point(this.Width / 2 - 120, 150);
                        label1.Text = "PAUSED";
                        label1.Visible= true;
                        gameMedia.controls.play();  
                        StopTimers();
                        pause = true;   

                    }
                }
            }
        }


        private void MoveMunitiontimer_Tick(object sender, EventArgs e)
        {
            shootgMedia.controls.play();
            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top > 0)
                {
                    munitions[i].Visible = true ; 
                    munitions[i].Top -= MunitionSpeed;

                    Collision();
                }
                else
                {
                    munitions[i].Visible = false ;
                    munitions[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemiSpeed);
        }

        private void MoveEnemies(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible= true ;
                array[i].Top += speed;

                if (array[i].Top > this.Height )
                {
                    array[i].Location = new Point( (i+1) * 50, -200 );
                }
            }

        }

        private void Collision()
        {
            for ( int i = 0;i < enemies.Length; i++)
            {
                if (munitions[0].Bounds.IntersectsWith(enemies[i].Bounds) 
                    || munitions[1].Bounds.IntersectsWith (enemies[i].Bounds) || munitions[2].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.controls.play();

                    score += 1;
                    scorelbl.Text = (score < 10) ?  "0" + score .ToString() : score.ToString();

                    if(score % 30 == 0)
                    {
                        level += 1;
                        levellbl.Text = (level < 10 ) ?  "0" + level .ToString() : level.ToString();    

                        if(enemiSpeed <= 10  && enemiesMunitionSpeed <= 10 && difficulty >= 0)
                        {
                            difficulty--;
                            enemiSpeed++;
                            enemiesMunitionSpeed++;
                        }

                        if(level == 10)
                        {
                            GameOver("NICE DONE");
                        }

                    }


                    enemies[i].Location = new Point( (i+1) * 50,  -100);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }

        private void GameOver(String str)
        {
            label1.Text = str;
            label1.Location = new Point(120, 120);
            label1.Visible = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true; 

            gameMedia.controls.stop();
            StopTimers();

        }

        //Stop timers
        private void StopTimers()
        {
            MoveBgTimer.Stop();
            MoveEnemiesTimer.Stop();
            MoveMunitiontimer.Stop();
            EnemiesMunitionTimer.Stop();
        }

        //Start timers
        private void StartTimers()
        {
            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            MoveMunitiontimer.Start();
            EnemiesMunitionTimer.Start();
        }

        private void EnemiesMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < (enemiesMunition.Length - difficulty); i++)
            {
                if (enemiesMunition[i].Top < this.Height)
                {
                    enemiesMunition[i].Visible = true;
                    enemiesMunition[i].Top += enemiesMunitionSpeed;
                }
                else
                {
                    enemiesMunition[i].Visible = false;
                    int x = rnd.Next(0, 10);
                    enemiesMunition[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
            }

            // Check for collisions after updating bullet positions
            CollisionWithEnemiesMunition();
        }


        private void CollisionWithEnemiesMunition()
        {
            for (int i = 0; i < enemiesMunition.Length; i++)
            {
                if (enemiesMunition[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesMunition[i].Visible = false;
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over");
                }
            }
        }


        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear(); 
            InitializeComponent(); 
            this.Size = new Size(800, 600); 
            Form1_Load(e, e); 
        }


        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
