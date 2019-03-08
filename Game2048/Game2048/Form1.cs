using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Game2048
{
    public partial class Form1 : Form
    {
        //2048 Game
        //Declerations
        Graphics graphics;//GDI will draw the tiles on bitmap
        Bitmap bmp;//will be shown in pixturebox
        Random rnd = new Random();//random tile location will be generated
        List<Tile> tiles = new List<Tile>();//we'll add every new created tile inside the list
        int[,] game_matrix = new int[4, 4];//will give information about occupied and empty slots, moving and new tile creations are dependant to this information
        long overall_score = 0, best_score = 0;//single game's score and best score, best score will be written in a text file.

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(this.pb.Width, this.pb.Height);//bitmap fills the picturebox
            graphics = Graphics.FromImage(bmp);//GDI will draw in this bitmap
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;//for high speed
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(!File.Exists("best_score.txt"))
            {
                using (StreamWriter wr = new StreamWriter("best_score.txt"))
                {
                    wr.WriteLine(0);//create best score text file if doesn't exist
                }
            }
            using (StreamReader sr = new StreamReader("best_score.txt"))//reading the best score from text file
            {
                best_score = Convert.ToInt64(sr.ReadLine());
            }
            lbl_bestScore.Text = "Best: " + best_score.ToString();//writing the best score into label
            prepare_new_game();//new game is prepared and two tiles placed on the matrix with random locations
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)//if left key is down
            {
                for(int i = 0; i < 4; i++)//reading the four rows of game matrix
                {
                    var row1 = tiles.FindAll(x => x.location.Y == i * 100);//getting four rows of the game matrix in a list
                    row1 = row1.OrderBy(o => o.location.X).ToList();//LINQ changes order of the tiles randomly so we need to order them ASC by location.Y
                    if (row1.Count == 1)//only one tile in this row so it goes to the end
                    {
                        row1[0].location.X = 0;//move the single tile to the end without merging
                    }
                    else if(row1.Count == 2)//two tiles will be merged if scores are the same
                    {
                        if (row1[0].score == row1[1].score)//their scores are equal then we need to merge them
                        {
                            overall_score += row1[0].score;//sum up the overall score
                            row1[0].score += row1[1].score;//merged at left
                            row1[1].disposed = true;//this tile is disposed
                            row1[0].location.X = 0;//merged tile moved to the end
                        }
                        else//their scores are different so we will move both to the end
                        {
                            row1[0].location.X = 0;//moved next to each other
                            row1[1].location.X = 100;//moved next to each other
                        }
                    }
                    else if (row1.Count == 3)//three tiles, the adjacent ones with the same score will be merged
                    {
                        if (row1[0].score == row1[1].score)//the most left two ones' scores are equal then we need to merge them
                        {
                            overall_score += row1[0].score;//sum up the overall score
                            row1[0].score += row1[1].score;//merged at left
                            row1[1].disposed = true;//this tile is disposed
                            row1[0].location.X = 0;//merged tile moved to the end
                            row1[2].location.X = 100;//third one moved without merging
                        }
                        else if (row1[1].score == row1[2].score)//first and second tile scores are different so we will check the second and third ones
                        {
                            overall_score += row1[1].score;//sum up the overall score
                            row1[1].score += row1[2].score;//merged at left
                            row1[2].disposed = true;//this tile is disposed
                            row1[0].location.X = 0;//first tile moved to the left end without merging
                            row1[1].location.X = 100;//second one moved next to the first one
                        }
                        else//there is no same score between the tiles
                        {
                            row1[0].location.X = 0;//move them to the end next to each other
                            row1[1].location.X = 100;//move them to the end next to each other
                            row1[2].location.X = 200;//move them to the end next to each other
                        }
                    }
                    else if (row1.Count == 4)//four tiles, the adjacent same score ones will be merged
                    {
                        if (row1[0].score == row1[1].score)//the left most ones' scores are equal then we need to merge them
                        {
                            overall_score += row1[0].score;//sum up the overall score
                            row1[0].score += row1[1].score;//merged at left
                            row1[1].disposed = true;//this tile is disposed
                            row1[0].location.X = 0;//merged tile moved to the end
                            if (row1[2].score == row1[3].score)//now checking the third and fourth
                            {
                                overall_score += row1[2].score;//sum up the overall score
                                row1[2].score += row1[3].score;//merged at left
                                row1[3].disposed = true;//this tile is disposed
                                row1[2].location.X = 100;//third tile moved to the left end after merging
                            }
                            else//third and fourth have different score then moving left seperately
                            {
                                row1[2].location.X = 100;//move them to the end next to each other
                                row1[3].location.X = 200;//move them to the end next to each other
                            }
                        }
                        else if (row1[1].score == row1[2].score)//first and second tile scores are different so we will check the second and third ones
                        {
                            overall_score += row1[1].score;//sum up the overall score
                            row1[1].score += row1[2].score;//merged at left
                            row1[2].disposed = true;//this tile is disposed
                            row1[0].location.X = 0;//first tile moved to the left end without merging
                            row1[1].location.X = 100;//second one moved next to the first one after merging
                            row1[3].location.X = 200;//fourth one moved next to them
                        }
                        else if (row1[2].score == row1[3].score)//second and third scores are different, then we check the third and fourth one
                        {
                            overall_score += row1[2].score;//sum up the overall score
                            row1[2].score += row1[3].score;//merged at left
                            row1[3].disposed = true;//this tile is disposed
                            row1[0].location.X = 0;//first tile moved to the left end without merging
                            row1[1].location.X = 100;//second one moved next to the first one after merging
                            row1[2].location.X = 200;//third one moved next to them after merging
                        }
                        else
                        {
                            //all slots are filled and all scores are different, so, no merging no moving
                        }
                    }
                }
                create_new_tile();//after all merge and moving finished we need to create a new tile with a random location
            }
            else if (e.KeyCode == Keys.Right)
            {
                for (int i = 0; i < 4; i++)//reading the four rows of game matrix
                {
                    var row1 = tiles.FindAll(x => x.location.Y == i * 100);//getting four rows of the game matrix in a list
                    row1 = row1.OrderBy(o => o.location.X).ToList();//LINQ changes order of the tiles randomly so we need to order them ASC by location.Y
                    if (row1.Count == 1)//only one tile in this row so it goes to the end
                    {
                        row1[0].location.X = 300;//move the single tile to the end without merging
                    }
                    else if (row1.Count == 2)//two tiles will be merged if scores are the same
                    {
                        if (row1[0].score == row1[1].score)//their scores are equal then we need to merge them
                        {
                            overall_score += row1[1].score;//sum up the overall score
                            row1[1].score += row1[0].score;//merged at right
                            row1[0].disposed = true;//this tile is disposed
                            row1[1].location.X = 300;//merged tile moved to the end
                        }
                        else//their scores are different so we will move both to the right end
                        {
                            row1[0].location.X = 200;//moved next to each other
                            row1[1].location.X = 300;//moved next to each other
                        }
                    }
                    else if (row1.Count == 3)//three tiles, the adjacent same score ones will be merged
                    {
                        if (row1[2].score == row1[1].score)//the right most ones' scores are equal then we need to merge them
                        {
                            overall_score += row1[2].score;//sum up the overall score
                            row1[2].score += row1[1].score;//merged at right
                            row1[1].disposed = true;//this tile is disposed
                            row1[2].location.X = 300;//merged tile moved to the end
                            row1[0].location.X = 200;//first one moved without merging
                        }
                        else if (row1[1].score == row1[0].score)//second and third tile scores are different so we will check the first and second ones
                        {
                            overall_score += row1[1].score;//sum up the overall score
                            row1[1].score += row1[0].score;//merged at right
                            row1[0].disposed = true;//this tile is disposed
                            row1[2].location.X = 300;//third tile moved to the rigth end without merging
                            row1[1].location.X = 200;//seocond one moved next to the third one
                        }
                        else//there is no same score between the tiles
                        {
                            row1[2].location.X = 300;//move them to the end next to each other
                            row1[1].location.X = 200;//move them to the end next to each other
                            row1[0].location.X = 100;//move them to the end next to each other
                        }
                    }
                    else if (row1.Count == 4)//four tiles, the adjacent same score ones will be merged
                    {
                        if (row1[3].score == row1[2].score)//the right most ones' scores are equal then we need to merge them
                        {
                            overall_score += row1[3].score;//sum up the overall score
                            row1[3].score += row1[2].score;//merged at right
                            row1[2].disposed = true;//this tile is disposed
                            row1[3].location.X = 300;//merged tile moved to the end
                            if (row1[1].score == row1[0].score)//now checking the first and second
                            {
                                overall_score += row1[1].score;//sum up the overall score
                                row1[1].score += row1[0].score;//merged at right
                                row1[0].disposed = true;//this tile is disposed
                                row1[1].location.X = 200;//second tile moved next to the fourth tile after merging
                            }
                            else//first and second have different score then moving to the right end seperately
                            {
                                row1[0].location.X = 100;//move them to the end next to each other
                                row1[1].location.X = 200;//move them to the end next to each other
                            }
                        }
                        else if (row1[2].score == row1[1].score)//third and fourth tile scores are different so we will check the third and second ones
                        {
                            overall_score += row1[2].score;//sum up the overall score
                            row1[2].score += row1[1].score;//merged at right
                            row1[1].disposed = true;//this tile is disposed
                            row1[3].location.X = 300;//fourth tile moved to the right end without merging
                            row1[2].location.X = 200;//third one moved next to the fourth one after merging
                            row1[0].location.X = 100;//first one moved next to them without merging
                        }
                        else if (row1[1].score == row1[0].score)//third and second are different too then we check the first and second one
                        {
                            overall_score += row1[1].score;//sum up the overall score
                            row1[1].score += row1[0].score;//merged at right
                            row1[0].disposed = true;//this tile is disposed
                            row1[3].location.X = 300;//fourth tile moved to the right end without merging
                            row1[2].location.X = 200;//third one moved next to the fourth one
                            row1[1].location.X = 100;//second one moved next to them after merging
                        }
                        else
                        {
                            //all slots are filled and all scores are different, so, no merging no moving
                        }
                    }
                }
                create_new_tile();//after all merge and moving finished we need to create a new tile with a random location
            }
            else if (e.KeyCode == Keys.Up)
            {
                for (int i = 0; i < 4; i++)//reading the four columns of game matrix
                {
                    var col1 = tiles.FindAll(x => x.location.X == i * 100);//getting four columns of the game matrix in a list
                    col1 = col1.OrderBy(o => o.location.Y).ToList();//LINQ changes order of the tiles randomly so we need to order them ASC by location.Y
                    if (col1.Count == 1)//only one tile in this col so it goes to the end
                    {
                        col1[0].location.Y = 0;//move the single tile to the end without merging
                    }
                    else if (col1.Count == 2)//two tiles will be merged if scores are the same
                    {
                        if (col1[0].score == col1[1].score)//their scores are equal then we need to merge them
                        {
                            overall_score += col1[0].score;//sum up the overall score
                            col1[0].score += col1[1].score;//merged at up
                            col1[1].disposed = true;//this tile is disposed
                            col1[0].location.Y = 0;//merged tile moved to the end
                        }
                        else//their scores are different so we will move both to the up
                        {
                            col1[0].location.Y = 0;//moved next to each other
                            col1[1].location.Y = 100;//moved next to each other
                        }
                    }
                    else if (col1.Count == 3)//three tiles, the adjacent same score ones will be merged
                    {
                        if (col1[0].score == col1[1].score)//the top most ones' scores are equal then we need to merge them
                        {
                            overall_score += col1[0].score;//sum up the overall score
                            col1[0].score += col1[1].score;//merged at top
                            col1[1].disposed = true;//this tile is disposed
                            col1[0].location.Y = 0;//merged tile moved to the end
                            col1[2].location.Y = 100;//other one moved without merging
                        }
                        else if (col1[1].score == col1[2].score)//first and second tile scores are different so we will check the second and third ones
                        {
                            overall_score += col1[1].score;//sum up the overall score
                            col1[1].score += col1[2].score;//merged at top
                            col1[2].disposed = true;//this tile is disposed
                            col1[0].location.Y = 0;//first tile moved to the up end without merging
                            col1[1].location.Y = 100;//other one moved next to the first one
                        }
                        else//there is no same score between the tiles
                        {
                            col1[0].location.Y = 0;//move them to the end next to each other
                            col1[1].location.Y = 100;//move them to the end next to each other
                            col1[2].location.Y = 200;//move them to the end next to each other
                        }
                    }
                    else if (col1.Count == 4)//four tiles, the adjacent same score ones will be merged
                    {
                        if (col1[0].score == col1[1].score)//the top most ones' scores are equal then we need to merge them
                        {
                            overall_score += col1[0].score;//sum up the overall score
                            col1[0].score += col1[1].score;//merged at top
                            col1[1].disposed = true;//this tile is disposed
                            col1[0].location.Y = 0;//merged tile moved to the end
                            if (col1[2].score == col1[3].score)//now checking the third and fourth
                            {
                                overall_score += col1[2].score;//sum up the overall score
                                col1[2].score += col1[3].score;//merged at top
                                col1[3].disposed = true;//this tile is disposed
                                col1[2].location.Y = 100;//third tile moved next to the first after merging
                            }
                            else//third and fourth have different score then moving left seperately
                            {
                                col1[2].location.Y = 100;//move them to the end next to each other
                                col1[3].location.Y = 200;//move them to the end next to each other
                            }
                        }
                        else if (col1[1].score == col1[2].score)//first and second tile scores are different so we will check the second and third ones
                        {
                            overall_score += col1[1].score;//sum up the overall score
                            col1[1].score += col1[2].score;//merged at top
                            col1[2].disposed = true;//this tile is disposed
                            col1[0].location.Y = 0;//first tile moved to the up end without merging
                            col1[1].location.Y = 100;//second one moved next to the first one after merging
                            col1[3].location.Y = 200;//fourth one moved next to them
                        }
                        else if (col1[2].score == col1[3].score)//second and third are different too then we will check the third and fourth one
                        {
                            overall_score += col1[2].score;//sum up the overall score
                            col1[2].score += col1[3].score;//merged at top
                            col1[3].disposed = true;//this tile is disposed
                            col1[0].location.Y = 0;//first tile moved to the up end without merging
                            col1[1].location.Y = 100;//second one moved next to the first one after merging
                            col1[2].location.Y = 200;//third one moved next to them after merging
                        }
                        else
                        {
                            //all slots are filled and all scores are different, so, no merging no moving
                        }
                    }
                }
                create_new_tile();//after all merge and moving finished we need to create a new tile with a random location
            }
            else if (e.KeyCode == Keys.Down)
            {
                for (int i = 0; i < 4; i++)//reading the four columns of game matrix
                {
                    var col1 = tiles.FindAll(x => x.location.X == i * 100);//getting four columns of the game matrix in a list
                    col1 = col1.OrderBy(o => o.location.Y).ToList();//LINQ changes order of the tiles randomly so we need to order them ASC by location.Y
                    if (col1.Count == 1)//only one tile in this col so it goes to the end
                    {
                        col1[0].location.Y = 300;//move the single tile to the bottom end without merging
                    }
                    else if (col1.Count == 2)//two tiles will be merged if scores are the same
                    {
                        if (col1[1].score == col1[0].score)//their scores are equal then we need to merge them
                        {
                            overall_score += col1[1].score;//sum up the overall score
                            col1[1].score += col1[0].score;//merged at bottom
                            col1[0].disposed = true;//this tile is disposed
                            col1[1].location.Y = 300;//merged tile moved to the end
                        }
                        else//their scores are different so we will move both to the bottom
                        {
                            col1[0].location.Y = 200;//moved next to each other
                            col1[1].location.Y = 300;//moved next to each other
                        }
                    }
                    else if (col1.Count == 3)//three tiles, the adjacent same score ones will be merged
                    {
                        if (col1[2].score == col1[1].score)//the right most ones' scores are equal then we need to merge them
                        {
                            overall_score += col1[2].score;//sum up the overall score
                            col1[2].score += col1[1].score;//merged at bottom
                            col1[1].disposed = true;//this tile is disposed
                            col1[2].location.Y = 300;//merged tile moved to the end
                            col1[0].location.Y = 200;//other one moved without merging
                        }
                        else if (col1[1].score == col1[0].score)//second and third tile scores are different so we will check the first and second ones
                        {
                            overall_score += col1[1].score;//sum up the overall score
                            col1[1].score += col1[0].score;//merged at bottom
                            col1[0].disposed = true;//this tile is disposed
                            col1[1].location.Y = 200;//other one moved next to the third one
                            col1[2].location.Y = 300;//third tile moved to the bottom end without merging
                        }
                        else//there is no same score between the tiles
                        {
                            col1[0].location.Y = 100;//move them to the end next to each other
                            col1[1].location.Y = 200;//move them to the end next to each other
                            col1[2].location.Y = 300;//move them to the end next to each other
                        }
                    }
                    else if (col1.Count == 4)//four tiles, the adjacent same score ones will be merged
                    {
                        if (col1[3].score == col1[2].score)//the bottom ones' scores are equal then we need to merge them
                        {
                            overall_score += col1[3].score;//sum up the overall score
                            col1[3].score += col1[2].score;//merged at bottom
                            col1[2].disposed = true;//this tile is disposed
                            col1[3].location.Y = 300;//merged tile moved to the end
                            if (col1[1].score == col1[0].score)//now checking the first and second
                            {
                                overall_score += col1[1].score;//sum up the overall score
                                col1[1].score += col1[0].score;//merged at bottom
                                col1[0].disposed = true;//this tile is disposed
                                col1[1].location.Y = 200;//second tile moved next to the fourth tile after merging
                            }
                            else//first and second have different score then moving bottom seperately
                            {
                                col1[0].location.Y = 100;//move them to the end next to each other
                                col1[1].location.Y = 200;//move them to the end next to each other
                            }
                        }
                        else if (col1[2].score == col1[1].score)//third and fourth tile scores are different so we will check the third and second ones
                        {
                            overall_score += col1[2].score;//sum up the overall score
                            col1[2].score += col1[1].score;//merged at bottom
                            col1[1].disposed = true;//this tile is disposed
                            col1[3].location.Y = 300;//fourth tile moved to the bottom end without merging
                            col1[2].location.Y = 200;//third one moved next to the fourth one after merging
                            col1[0].location.Y = 100;//first one moved next to them
                        }
                        else if (col1[1].score == col1[0].score)//third and second are different, then we will check the first and second one
                        {
                            overall_score += col1[1].score;//sum up the overall score
                            col1[1].score += col1[0].score;//merged at bottom
                            col1[0].disposed = true;//this tile is disposed
                            col1[3].location.Y = 300;//fourth tile moved to the bottom end without merging
                            col1[2].location.Y = 200;//third one moved next to the fourth one
                            col1[1].location.Y = 100;//second one moved next to them after merging
                        }
                        else
                        {
                            //all slots are filled and all scores are different, so, no merging no moving
                        }
                    }
                }
                create_new_tile();//after all merge and moving finished we need to create a new tile with a random location
            }
        }

        /// <summary>
        /// Prepares the new game with two new tiles with random locations
        /// </summary>
        void prepare_new_game()
        {
            overall_score = 0;
            lbl_gameOver.Visible = false;//hide 'game over' message
            tiles.Clear();//clear the Tiles list
            update_game_matrix();//make all zeros the game matrix
            create_new_tile();//prepare one tile with random location
            create_new_tile();//prepare another tile with random location
        }

        /// <summary>
        /// If the game is over
        /// </summary>
        /// <returns>true=game over</returns>
        bool game_over()
        {
            bool gameover = true;
            for(int i = 0; i < game_matrix.GetLength(0); i ++)
            {
                for (int j = 0; j < game_matrix.GetLength(1); j++)
                {
                    if (game_matrix[i, j] == 0)//if there is no item with zero in the game matrix then the game is over
                    {
                        gameover = false;
                    }
                }
            }
            return gameover;
        }

        /// <summary>
        /// Creates each tile with random locations in the free slots of the gam matrix
        /// </summary>
        void create_new_tile()
        {
            if (overall_score > best_score)
            {
                best_score = overall_score;//best score is changed because current one is bigger than that
            }
            //check is the game over?
            if (game_over())
            {
                //game over
                lbl_gameOver.Visible = true;//make 'game over' message visible
            }
            else//if game is not over yet
            {
                lbl_score.Text = "Score: " + overall_score.ToString();//print the current score
                lbl_bestScore.Text = "Best: " + best_score.ToString();//print the best score
                update_game_matrix();//alter the game matrix values, make the occupied ones' values one
                //create new tile
                Point rnd_point;//new random location for new tile
                int rnd_x = 0;//we need to check in the game matrix if this location is occupied already
                int rnd_y = 0;//we need to check in the game matrix if this location is occupied already
                do//I preferred do instead of while here because random numbers will be created at least once so not all the new tiles will start from [0, 0]
                {
                    rnd_x = rnd.Next(0, 4);//gives a random number between 0 and 3, including 3
                    rnd_y = rnd.Next(0, 4);
                } while (game_matrix[rnd_y, rnd_x] != 0);//we need to check in the game matrix if this location is occupied already
                rnd_point = new Point(rnd_x * 100, rnd_y * 100);//we have a free slot
                Tile t = new Tile(rnd_point);//create new tile in this free slot
                tiles.Add(t);//add the new tile into the list
                update_game_matrix();
                draw_all_scene();//clear and draw everything on bitmap again.
            }
        }

        /// <summary>
        /// Clears the bitmap and redraws all the scene onto bitmap and assigns bitmap to the picturebox
        /// </summary>
        void draw_all_scene()
        {
            graphics.Clear(Color.White);//clear the bitmap with the picturebox's back colour (white)
            foreach(Tile t in tiles)
            {
                t.draw(graphics);//call aevery tile's draw method
            }
            pb.Image = bmp;//assign the bitmap to the picturebos's image property
        }

        /// <summary>
        /// Make the occupied slots' values one in the game matrix
        /// </summary>
        void update_game_matrix()
        {
            //update tiles
            tiles.Remove(tiles.Find(x => x.disposed == true));//remove the merged (and disposed) tiles from the list
            //update matrix
            Array.Clear(game_matrix, 0, game_matrix.Length);//make all the game matrix values 0
            int x_index = 0;
            int y_index = 0;
            foreach(Tile t in tiles)
            {
                x_index = t.location.Y / 100;//tile's location's Y and X components are game matrix's indices after divided by 100
                y_index = t.location.X / 100;
                game_matrix[x_index, y_index] = 1;//make the occupied slot's value 1
            }
        }

        private void btn_newGame_Click(object sender, EventArgs e)
        {
            prepare_new_game();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (StreamWriter wr = new StreamWriter("best_score.txt"))
            {
                wr.WriteLine(best_score);//write the best score to the text file before closing the app
            }
        }
    }

    internal class Tile
    {
        internal Point location;//location of the tile. We want to reach from the main form class then it is defined as internal since they are in the same namespace
        internal int score = 2;//accessible by same namespace's members
        Color backColour, fontColour;//these will cahnge with the score of the each tile
        Font f = new Font("Arial", 24);//for writing the tile's score to the centre
        internal bool disposed = false;//merged ones will be disposed and will be removed from the list in main form

        public Tile(Point Location)
        {
            this.location = Location;//tile will be created with the given random location by the main form
        }

        /// <summary>
        /// This is the main drawing function of the tile class. Draws the tile and it's score on it
        /// </summary>
        /// <param name="g">Graphics object, will be used to draw the tile and the score. Must be derived in the main form before calling the function.</param>
        internal void draw(Graphics g)
        {
            #region set the colour
            switch(score)//tile's and text's colours are being changed by the score
            {
                case 2:
                    {
                        backColour = Color.LightGray;
                        fontColour = Color.Black;
                        break;
                    }
                case 4:
                    {
                        backColour = Color.LightPink;
                        fontColour = Color.Black;
                        break;
                    }
                case 8:
                    {
                        backColour = Color.LightSalmon;
                        fontColour = Color.White;
                        break;
                    }
                case 16:
                    {
                        backColour = Color.OrangeRed;
                        fontColour = Color.White;
                        break;
                    }
                case 32:
                    {
                        backColour = Color.Orange;
                        fontColour = Color.White;
                        break;
                    }
                case 64:
                    {
                        backColour = Color.Pink;
                        fontColour = Color.White;
                        break;
                    }
                case 128:
                    {
                        backColour = Color.PaleVioletRed;
                        fontColour = Color.White;
                        break;
                    }
                case 256:
                    {
                        backColour = Color.MediumVioletRed;
                        fontColour = Color.White;
                        break;
                    }
                case 512:
                    {
                        backColour = Color.Red;
                        fontColour = Color.White;
                        break;
                    }
                case 1024:
                    {
                        backColour = Color.IndianRed;
                        fontColour = Color.White;
                        break;
                    }
                case 2048:
                    {
                        backColour = Color.DarkRed;
                        fontColour = Color.White;
                        break;
                    }
            }
            #endregion
            g.FillRectangle(new SolidBrush(backColour), new Rectangle(location, new Size(100, 100)));//tile itself, drawn as fillrectangle
            SizeF text_size = g.MeasureString(score.ToString(), f);//measuring the text size for aligning it to the centre of the tile
            g.DrawString(score.ToString(), f, new SolidBrush(fontColour), new PointF(location.X + ((100 - text_size.Width) / 2), location.Y + ((100 - text_size.Height) / 2)));//tile's score is written in the centre
        }
    }
}