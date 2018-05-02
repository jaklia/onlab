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
        Image RobotImg;

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++)
            {
                GameBoardGrid.RowDefinitions.Add(new RowDefinition());
                GameBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
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


            RobotImg = new Image();
            RobotImg.Source = new BitmapImage(new Uri("Resources/Robot/right.png", UriKind.Relative));
            GameBoardGrid.Children.Add(RobotImg);
            RobotImg.Height = 40;
            RobotImg.Width = 40;
            Grid.SetColumn(RobotImg, 0);
            Grid.SetRow(RobotImg, 0);


            // TreeView 
            treeView.Items.Clear();
            TreeViewGeneratorVisitor treeViewGeneratorVisitor = new TreeViewGeneratorVisitor();
            treeViewGeneratorVisitor.ExpandAll = true;
            var tree = treeViewGeneratorVisitor.VisitProgram(ctx);
            tree.ExpandSubtree();
            treeView.Items.Add(tree);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            

            game = new Game(10, 10);

            RobotControllerVisitor robotControllerVisitor = new RobotControllerVisitor(RobotImg, game);
            robotControllerVisitor.VisitProgram(ctx);
        }

    }
}
