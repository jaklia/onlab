using Antlr4.Runtime;
using Robot.Grammar;
using Robot.Model;
using Robot.Visitors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using Robot.ViewModels;
using System.Collections.Generic;
using Robot.Errors;

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

        public MainWindow()
        {
            InitializeComponent();
        }
       
        void InitGame(Board map)
        {
            game = new Game(map);

            for (int i = 0; i < map.Height; i++)
            {
                GameBoardGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < map.Width; i++)
            {
                GameBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            //cmdManager = new Commands.CommandManager(); // eznemkellitt
        //    game.Board.Init2();
            startingState = game.Clone();   // ez nem feltétlen kell ide, a ResetButton_Click-ből meg lehet hívni az InitGame()-t
            DrawGame(game);
            StartButton.IsEnabled = false;
            DoCmdBtn.IsEnabled = false;
            UndoCmdBtn.IsEnabled = false;
            DoAllBtn.IsEnabled = false;
            UndoAllBtn.IsEnabled = false;
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

            // check errors
            var errorVisitor = new RobotErrorVisitor();
            var what = errorVisitor.Visit(ctx);

            if (errorVisitor.errorList.Count > 0)
            {
                MessageBox.Show("errors");
                return;
            }
            
            // TreeView 
            treeView.Items.Clear();
            TreeViewGeneratorVisitor treeViewGeneratorVisitor = new TreeViewGeneratorVisitor();
            treeViewGeneratorVisitor.ExpandAll = true;
            var tree = treeViewGeneratorVisitor.VisitProgram(ctx);
            tree.ExpandSubtree();
            treeView.Items.Add(tree);

            game = startingState.Clone();

            // build cmd list
         //   List<CommandBase> commands = new List<CommandBase>();
            //RobotControllerVisitor robotControllerVisitor = new RobotControllerVisitor(game, cmdManager);
         //   RobotControllerVisitor robotControllerVisitor = new RobotControllerVisitor(game, commands);
         //   robotControllerVisitor.VisitProgram(ctx);
            DrawGame(game);
            cmdManager = new Commands.CommandManager(game, ctx);

            StartButton.IsEnabled = true;
            DoCmdBtn.IsEnabled = true;
            UndoCmdBtn.IsEnabled = true;
            DoAllBtn.IsEnabled = true;
            UndoAllBtn.IsEnabled = true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            cmdManager.RunProg();
            DrawGame(game);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            cmdManager.UndoProg();
            DrawGame(game);
        }

        //void ResetGame()
        //{
        //    treeView.Items.Clear();
        //    game = startingState.Clone();
        //    cmdManager.Reset();
        //    textBox.Text = "";
        //    StartButton.IsEnabled = false;
        //    DoCmdBtn.IsEnabled = false;
        //    UndoCmdBtn.IsEnabled = false;
        //    DrawGame(game);
        //    //InitGame();
        //}

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
            cmdManager.UndoAll();
            DrawGame(game);
        }

        private void DoAllBtn_Click(object sender, RoutedEventArgs e)
        {
            // enable only if we are in a loop / function
            cmdManager.DoAll();
            DrawGame(game);
        }
        
        private void LoadMap_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog filePicker = new OpenFileDialog();
            bool res = filePicker.ShowDialog() ?? false;

            if (res) 
            {
                var ctx = getMapContext(filePicker.FileName);
                var errorList = ValidateMap(ctx);

                if (errorList.Count == 0)
                {
                    var map = GetMap(ctx);
                    InitGame(map);
                }else
                {
                    errorList.ForEach(item => ((MainViewModel)DataContext).errorList.Add(item));
                    
                }
            }
        }

        MapEditorGrammarParser.BuildMapContext getMapContext (string path)
        {
            // read the description from file
            string map = File.ReadAllText(path);
            // parse
            var inputStream = new AntlrInputStream(map);
            var lexer = new MapEditorGrammarLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new MapEditorGrammarParser(tokenStream);
            MapEditorGrammarParser.BuildMapContext ctx = parser.buildMap();
            return ctx;
        }

        List<ErrorLogItem> ValidateMap(MapEditorGrammarParser.BuildMapContext ctx)
        {
            // check for errors
            var mapErrorVisitor = new MapErrorVisitor();
            mapErrorVisitor.Visit(ctx);
            return mapErrorVisitor.errorList;
        }
        
        Board GetMap (MapEditorGrammarParser.BuildMapContext ctx)
        {
            // build the map in the visitor
            var mapBuilderVisitor = new MapBuilderVisitor();
            mapBuilderVisitor.Visit(ctx);
            return mapBuilderVisitor.Map;
        }

        void DrawGame(Game game)
        {
            GameBoardGrid.Children.Clear();
            Image[,] imgs = new Image[game.Board.Height,game.Board.Width];
            // draw the board
            for (int row=0; row<game.Board.Height; row++)
            {
                for (int col=0; col<game.Board.Width; col++)
                {
                    imgs[row, col] = new Image();
                    Grid.SetColumn(imgs[row, col], col);
                    Grid.SetRow(imgs[row, col], row);
                    imgs[row, col].MaxHeight = 40;
                    imgs[row, col].MaxWidth = 40;
                    imgs[row, col].Stretch = System.Windows.Media.Stretch.Uniform;
                    imgs[row, col].Margin = new Thickness(2, 2, 2, 2);
                    GameBoardGrid.Children.Add(imgs[row, col]);
                    if (game.Board.GetField(row, col).HasItem())
                    {
                        Item item = game.Board.GetField(row, col).item;
                        imgs[row, col].Source = new BitmapImage(new Uri("../Resources/Items/key.png", UriKind.Relative));
                    } else if (game.Board.GetField(row, col) is Wall) {
                        //  game.Board.GetField(row,col).GetType() == new Wall(0,0).GetType()
                        imgs[row, col].Source = new BitmapImage(new Uri("../Resources/wall.png", UriKind.Relative));
                    }
                    else {
                        imgs[row, col].Source = new BitmapImage(new Uri("../Resources/emptyfield.png", UriKind.Relative));
                    }                    
                }
            }
            Field finish = game.Board.Finish;
            imgs[finish.Row, finish.Column].Source = new BitmapImage(new Uri("../Resources/destflag.png", UriKind.Relative));
            
            // ez így nem jó,   btw miért csak úgy jó ha ki van írva a path??
            //imgs[finish.Row, finish.Column] = (Image)Resources["destfield"];  

            // draw the player
            GameBoardGrid.Children.Add(RobotImg);
            RobotImg.MaxHeight = 40;
            RobotImg.MaxWidth = 40;
            RobotImg.Stretch = System.Windows.Media.Stretch.Uniform;
            RobotImg.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetColumn(RobotImg, game.Player.Column);
            Grid.SetRow(RobotImg, game.Player.Row);
            switch (game.Player.Dir)  /* ezt is külön (viewmodel?????) */
            {
                case MoveDir.UP:
                    RobotImg.Source = new BitmapImage(new Uri("../Resources/Robot/up.png", UriKind.Relative));
                    break;
                case MoveDir.RIGHT:
                    RobotImg.Source = new BitmapImage(new Uri("../Resources/Robot/right.png", UriKind.Relative));
                    break;
                case MoveDir.DOWN:
                    RobotImg.Source = new BitmapImage(new Uri("../Resources/Robot/down.png", UriKind.Relative));
                    break;
                case MoveDir.LEFT:
                    RobotImg.Source = new BitmapImage(new Uri("../Resources/Robot/left.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
            
        }

    }
}
