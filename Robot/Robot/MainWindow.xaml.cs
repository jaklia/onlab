using Antlr4.Runtime;
using Robot.Grammar;
using Robot.Model;
using Robot.Visitors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Robot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RobotGrammarParser.ProgramContext ctx;   /* ezeket külön osztályba (viewmodel????) !!!!! */
        Game game;
        Image RobotImg = new Image();
        Game startingState;
        Commands.CommandManager cmdManager;  
        // !!!  TODO:  add the commands in the visitor, and execute them in DoCmdBtn_Click
        //           and UndoCmdBtn_Click

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++)
            {
                GameBoardGrid.RowDefinitions.Add(new RowDefinition());
                GameBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            InitGame();
        }
        // TODO :
        // (error)
        // (material design)
        // (treeview)
        void InitGame()
        {
            game = new Game(10, 10);
            cmdManager = new Commands.CommandManager();
            game.Board.Init2();
            startingState = game.Clone();   // ez nem feltétlen kell ide, a ResetButton_Click-ből meg lehet hívni az InitGame()-t
            DrawGame(game);
            StartButton.IsEnabled = false;
            DoCmdBtn.IsEnabled = false;
            UndoCmdBtn.IsEnabled = false;
            textBox.Text = "";
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            string code = textBox.Text;
            var inputStream = new AntlrInputStream(code);
            var lexer = new RobotGrammarLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new RobotGrammarParser(tokenStream);
            //parser.BuildParseTree = true;
            ctx = parser.program();
            
            // TreeView 
            treeView.Items.Clear();
            TreeViewGeneratorVisitor treeViewGeneratorVisitor = new TreeViewGeneratorVisitor();
            treeViewGeneratorVisitor.ExpandAll = true;
            var tree = treeViewGeneratorVisitor.VisitProgram(ctx);
            tree.ExpandSubtree();
            treeView.Items.Add(tree);

            game = startingState.Clone();
            cmdManager.Reset();

            // build cmd list
            RobotControllerVisitor robotControllerVisitor = new RobotControllerVisitor(game, cmdManager);
            robotControllerVisitor.VisitProgram(ctx);
            DrawGame(game);

            StartButton.IsEnabled = true;
            DoCmdBtn.IsEnabled = true;
            UndoCmdBtn.IsEnabled = true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            cmdManager.DoAll();
            DrawGame(game);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            cmdManager.UndoAll();
            DrawGame(game);
        }

        void ResetGame()
        {
            treeView.Items.Clear();
            game = startingState.Clone();
            cmdManager.Reset();
            textBox.Text = "";
            StartButton.IsEnabled = false;
            DoCmdBtn.IsEnabled = false;
            UndoCmdBtn.IsEnabled = false;
            DrawGame(game);
            //InitGame();
        }

        private void DoCmdBtn_Click(object sender, RoutedEventArgs e)
        {
            cmdManager.DoCommand();
            DrawGame(game);
        }

        private void UndoCmdBtn_Click(object sender, RoutedEventArgs e)
        {
            cmdManager.UndoCommand();
            DrawGame(game);
        }

        private void UndoAllBtn_Click(object sender, RoutedEventArgs e)
        {
            // enable only if we are in a loop / function
        }

        private void DoAllBtn_Click(object sender, RoutedEventArgs e)
        {
            // enable only if we are in a loop / function
        }

        void DrawGame(Game game)
        {
            GameBoardGrid.Children.Clear();
            Image[,] imgs = new Image[game.Board.Height,game.Board.Width];
            // draw the board
            for (int i=0; i<game.Board.Height; i++)
            {
                for (int j=0; j<game.Board.Width; j++)
                {
                    imgs[i,j] = new Image();
                    imgs[i, j].Height = 40;
                    imgs[i, j].Width = 40;
                    GameBoardGrid.Children.Add(imgs[i, j]);
                    if (game.Board.GetField(i,j).HasItem())
                    {
                        Item item = game.Board.GetField(i, j).item;
                        imgs[i, j].Source = new BitmapImage(new Uri("Resources/Items/key.png", UriKind.Relative));
                    } else if (game.Board.GetField(i,j).GetType() == new Wall(0,0).GetType()) {
                        imgs[i, j].Source = new BitmapImage(new Uri("Resources/wall.png", UriKind.Relative));
                    }
                    else {
                        imgs[i, j].Source = new BitmapImage(new Uri("Resources/emptyfield.png", UriKind.Relative));
                    }
                    Grid.SetColumn(imgs[i, j], j);
                    Grid.SetRow(imgs[i, j], i);
                    
                }
            }
            // ehelyett a Board-ban GetDestField kell majd !!!
            imgs[game.Board.Height - 1, game.Board.Width - 1].Source = new BitmapImage(new Uri("Resources/destflag.png", UriKind.Relative));

            // draw the player
            GameBoardGrid.Children.Add(RobotImg);
            RobotImg.Height = 40;
            RobotImg.Width = 40;
            Grid.SetColumn(RobotImg, game.Player.Column);
            Grid.SetRow(RobotImg, game.Player.Row);
            switch (game.Player.Dir)  /* ezt is külön (viewmodel?????) */
            {
                case Model.Robot.MoveDir.UP:
                    RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/up.png", UriKind.Relative));
                    break;
                case Model.Robot.MoveDir.RIGHT:
                    RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/right.png", UriKind.Relative));
                    break;
                case Model.Robot.MoveDir.DOWN:
                    RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/down.png", UriKind.Relative));
                    break;
                case Model.Robot.MoveDir.LEFT:
                    RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/left.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
            
        }

    }
}
