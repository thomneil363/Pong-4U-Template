/*
 * Description:     A basic PONG game against an AI
 * Author:          Thomas Neilson
 * Date:            2020-09-22
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush drawBrush = new SolidBrush(Color.White);

        SolidBrush blackBrush = new SolidBrush(Color.Black);

        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean wKeyDown, sKeyDown, upKeyDown, downKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        int BALL_SPEED = 6;
        Rectangle ball;

        //paddle speeds and rectangles
        int PADDLE_SPEED = 5;
        

        Rectangle p1, p2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 5;  // number of points needed to win game

        // Creative Changes

        int ballSize = 10;

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }
        
        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
            }
        }

        private void StartLabel_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
            }

            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            

            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 40;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - PADDLE_EDGE - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            // set Width and Height of ball

            ball.Width = ballSize;

            ball.Height = ballSize;

            // set starting X position for ball to middle of screen, (use this.Width and ball.Width)

            ball.X = this.Width / 2 - ball.Width / 2;

            // set starting Y position for ball to middle of screen, (use this.Height and ball.Height)

            ball.Y = this.Height / 2 - ball.Height / 2;

        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region update ball position

            // code to move ball either left or right based on ballMoveRight and using BALL_SPEED

            if(ballMoveRight == true)
            {
                ball.X = ball.X + BALL_SPEED;
            }
            else
            {
                ball.X = ball.X - BALL_SPEED;
            }

            // code to move ball either down or up based on ballMoveDown and using BALL_SPEED

            if (ballMoveDown == true)
            {
                ball.Y = ball.Y + BALL_SPEED;
            }
            else
            {
                ball.Y = ball.Y - BALL_SPEED;
            }

            #endregion

            #region update paddle positions

            if ((wKeyDown == true || upKeyDown == true) && p1.Y > 0)
            {
                // code to move player 1 paddle up using p1.Y and PADDLE_SPEED

                p1.Y = p1.Y - PADDLE_SPEED;

            }
            if ((sKeyDown == true || downKeyDown == true) && p1.Y < this.Height - p1.Height)
            {
                // if statement and code to move player 1 paddle down using p1.Y and PADDLE_SPEED

                p1.Y = p1.Y + PADDLE_SPEED;

            }

            /*
             * 
             * if (upKeyDown == true && p2.Y > 0)
            {
                // if statement and code to move player 2 paddle up using p2.Y and PADDLE_SPEED

                p2.Y = p2.Y - PADDLE_SPEED;

            }
            if (downKeyDown == true && p2.Y < this.Height - p2.Height)
            {
                // if statement and code to move player 2 paddle down using p2.Y and PADDLE_SPEED

                p2.Y = p2.Y + PADDLE_SPEED;

            }
            */

            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0) // if ball hits top line
            {
                // use ballMoveDown boolean to change direction

                ballMoveDown = true;

                collisionSound.Play();

                // TODO play a collision sound
            }

            if (ball.Y > this.Height - ball.Height)
            {
                ballMoveDown = false;

                collisionSound.Play();
            }

            // AI Code

            if (ball.X <= this.Width / 2)
            {
                if (ballMoveDown == true && p2.Y > 0)
                {
                    p2.Y = p2.Y - PADDLE_SPEED;
                }

                if (ballMoveDown == false && p2.Y < this.Height - p2.Height)
                {
                    p2.Y = p2.Y + PADDLE_SPEED;
                }
            }
            else
            {
                if (ball.Y > p2.Y)
                {
                    p2.Y = p2.Y + PADDLE_SPEED;
                }
                if (ball.Y < p2.Y)
                {
                    p2.Y = p2.Y - PADDLE_SPEED;
                }
                else
                {
                    p2.Y = p2.Y;
                }
            }

            #endregion

            #region ball collision with paddles

            if (ball.IntersectsWith(p1) || ball.IntersectsWith(p2))
            {
                collisionSound.Play();

                ballMoveRight = !ballMoveRight;
            }


            // create if statment that checks p1 collides with ball and if it does
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction

            // create if statment that checks p2 collides with ball and if it does
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction

            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                // TODO
                // --- play score sound
                // --- update player 2 score

                ballMoveRight = !ballMoveRight;

                player2Score = player2Score + 1;

                scoreSound.Play();

                if (player2Score >= gameWinScore)
                {

                    GameOver("The AI");
                }
                else
                {
                    SetParameters();
                }

                // use if statement to check to see if player 2 has won the game. If true run 
                // GameOver method. Else change direction of ball and call SetParameters method.

            }

            if (ball.X > this.Width - ball.Width)  // ball hits left wall logic
            {
                // TODO
                // --- play score sound
                // --- update player 2 score

                ballMoveRight = !ballMoveRight;

                player1Score = player1Score + 1;

                scoreSound.Play();

                if (player1Score >= gameWinScore)
                {
                    
                    GameOver("The Player");
                }
                else
                {
                    SetParameters();
                }

                // use if statement to check to see if player 1 has won the game. If true run 
                // GameOver method. Else change direction of ball and call SetParameters method.

            }

            // same as above but this time check for collision with the right wall

            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }
        
        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            newGameOk = true;

            // TODO create game over logic
            // --- stop the gameUpdateLoop
            // --- show a message on the startLabel to indicate a winner, (need to Refresh).
            // --- pause for two seconds 
            // --- use the startLabel to ask the user if they want to play again

            gameUpdateLoop.Stop();

            ball.Y = -100;
            p1.Y = -100;
            p2.Y = -100;

            startLabel.Visible = true;
      
                startLabel.Text = winner + " Won! Press Space to play again.";

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // draw paddles using FillRectangle

            e.Graphics.FillRectangle(drawBrush, p1);

            e.Graphics.FillRectangle(drawBrush, p2);

            // draw ball using FillRectangle

            e.Graphics.FillRectangle(drawBrush, ball);

            // draw scores to the screen using DrawString

            e.Graphics.DrawString(player1Score.ToString(), drawFont, drawBrush, this.Width / 8, 10, null);

            e.Graphics.DrawString(player2Score.ToString(), drawFont, drawBrush, this.Width - this.Width / 8, 10, null);
        }

       

    }
}
