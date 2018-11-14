﻿using Antlr4.Runtime;
using Robot.Grammar;
using Robot.Model;
using Robot.Visitors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Robot.Commands;
using System.IO;
using Microsoft.Win32;

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
       
        void InitGame()
        {
            //game = new Game(10, 10);
            game = new Game(10, 10);

            //cmdManager = new Commands.CommandManager(); // eznemkellitt
            game.Board.Init2();
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
                //MessageBox.Show(filePicker.FileName);
                string map = File.ReadAllText(filePicker.FileName);
                var inputStream = new AntlrInputStream(map);
                var lexer = new MapEditorGrammarLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new MapEditorGrammarParser(tokenStream);
                MapEditorGrammarParser.MapContext mapCtx = parser.map();
                var mapBuilderVisitor = new MapBuilderVisitor();
                mapBuilderVisitor.Visit(mapCtx);
            }

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
                    //imgs[row, col].Height = 40;
                    //imgs[row, col].Width = 40;
                    imgs[row, col].Stretch = System.Windows.Media.Stretch.Uniform;
                    imgs[row, col].Margin = new Thickness(2, 2, 2, 2);
                    GameBoardGrid.Children.Add(imgs[row, col]);
                    if (game.Board.GetField(row, col).HasItem())
                    {
                        Item item = game.Board.GetField(row, col).item;
                        imgs[row, col].Source = new BitmapImage(new Uri("Resources/Items/key.png", UriKind.Relative));
                    } else if (game.Board.GetField(row,col).GetType() == new Wall(0,0).GetType()) {
                        imgs[row, col].Source = new BitmapImage(new Uri("Resources/wall.png", UriKind.Relative));
                    }
                    else {
                        imgs[row, col].Source = new BitmapImage(new Uri("Resources/emptyfield.png", UriKind.Relative));
                    }
                    Grid.SetColumn(imgs[row, col], col);
                    Grid.SetRow(imgs[row, col], row);
                    
                }
            }
            // ehelyett a Board-ban GetDestField kell majd !!!
            imgs[game.Board.Height - 1, game.Board.Width - 1].Source = new BitmapImage(new Uri("Resources/destflag.png", UriKind.Relative));

            // draw the player
            GameBoardGrid.Children.Add(RobotImg);
            //RobotImg.Height = 40;
            //RobotImg.Width = 40;
            RobotImg.Stretch = System.Windows.Media.Stretch.Uniform;
            RobotImg.Margin = new Thickness(2, 2, 2, 2);
            Grid.SetColumn(RobotImg, game.Player.Column);
            Grid.SetRow(RobotImg, game.Player.Row);
            switch (game.Player.Dir)  /* ezt is külön (viewmodel?????) */
            {
                case MoveDir.UP:
                    RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/up.png", UriKind.Relative));
                    break;
                case MoveDir.RIGHT:
                    RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/right.png", UriKind.Relative));
                    break;
                case MoveDir.DOWN:
                    RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/down.png", UriKind.Relative));
                    break;
                case MoveDir.LEFT:
                    RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/left.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
            
        }

    }
}
