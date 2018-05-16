using Antlr4.Runtime;
using Robot.Grammar;
using Robot.Model;
using Robot.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        Image ItemImg = new Image();

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
        // reset game
        void InitGame()
        {
            game = new Game(10, 10);
            game.Board.Init1();
            //RobotImg = new Image();
            //RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/right.png", UriKind.Relative));
            GameBoardGrid.Children.Add(ItemImg);
            GameBoardGrid.Children.Add(RobotImg);
            RobotImg.Height = 40;
            RobotImg.Width = 40;
            ItemImg.Height = 40;
            ItemImg.Width = 40;

            //Grid.SetColumn(RobotImg, 0);
            //Grid.SetRow(RobotImg, 0);
            DrawGame(game);
            StartButton.IsEnabled = false;
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            string code = textBox.Text;
            var inputStream = new AntlrInputStream(code);
            var lexer = new RobotGrammarLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new RobotGrammarParser(tokenStream);
            parser.BuildParseTree = true;
            ctx = parser.program();
            

            // TreeView 
            treeView.Items.Clear();
            TreeViewGeneratorVisitor treeViewGeneratorVisitor = new TreeViewGeneratorVisitor();
            treeViewGeneratorVisitor.ExpandAll = true;
            var tree = treeViewGeneratorVisitor.VisitProgram(ctx);
            tree.ExpandSubtree();
            treeView.Items.Add(tree);

            StartButton.IsEnabled = true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            RobotControllerVisitor robotControllerVisitor = new RobotControllerVisitor(game);
            robotControllerVisitor.VisitProgram(ctx);
            DrawGame(game);
        }

        void DrawGame(Game game)
        {
            // draw the board
            for (int i=0; i<game.Board.Height; i++)
            {
                for (int j=0; j<game.Board.Width; j++)
                {
                    if (game.Board.GetField(i,j).HasItem())
                    {
                        Item item = game.Board.GetField(i, j).GetItem();
                        // item.id / name !!!
                        //Image ItemImg = new Image();
                        //ItemImg.Height = 40;
                        //ItemImg.Width = 40;
                        Grid.SetColumn(ItemImg, j);
                        Grid.SetRow(ItemImg, i);
                        //ItemImg.Stretch = Stretch.Fill;
                       ItemImg.Source = new BitmapImage(new Uri("Resources/Items/key.png", UriKind.Relative));
                    }
                }
            }
            
            // draw the player
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
