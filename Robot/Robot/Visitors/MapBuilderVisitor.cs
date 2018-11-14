using Robot.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Robot.Model;

namespace Robot.Visitors
{
    class MapBuilderVisitor : MapEditorGrammarBaseVisitor<object>
    {
        Board map;
        

      

        public override object VisitMap([NotNull] MapEditorGrammarParser.MapContext context)
        {
            var height = int.Parse(context.height().GetText());
            var width = int.Parse(context.width().GetText());
            map = new Board(width, height);
            // map = new Board(10, 10);
            return base.VisitMap(context);
        }

        //public override object VisitHeight([NotNull] MapEditorGrammarParser.HeightContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public override object VisitWidth([NotNull] MapEditorGrammarParser.WidthContext context)
        //{
        //    throw new NotImplementedException();
        //}

        public override object VisitMapOptionRow([NotNull] MapEditorGrammarParser.MapOptionRowContext context)
        {
            return base.VisitMapOptionRow(context);
        }

        public override object VisitStart([NotNull] MapEditorGrammarParser.StartContext context)
        {
            var col = int.Parse(context.col().GetText()) - 1;
            var row = int.Parse(context.row().GetText()) - 1;
            map.Start(row, col);
            return base.VisitStart(context);
        }

        public override object VisitWalls([NotNull] MapEditorGrammarParser.WallsContext context)
        {
            return base.VisitWalls(context);
        }

        public override object VisitWall([NotNull] MapEditorGrammarParser.WallContext context)
        {
            var col = int.Parse(context.col().GetText()) - 1;
            var row = int.Parse(context.row().GetText()) - 1;
            map.Wall(row, col);
            return base.VisitWall(context);
        }
        
        public override object VisitFinish([NotNull] MapEditorGrammarParser.FinishContext context)
        {
            var col = int.Parse(context.col().GetText()) - 1;
            var row = int.Parse(context.row().GetText()) - 1;
            map.Finish(row, col);
            return base.VisitFinish(context);
        }
        
        public override object VisitKey([NotNull] MapEditorGrammarParser.KeyContext context)
        {
            var col = int.Parse(context.col().GetText()) - 1;
            var row = int.Parse(context.row().GetText()) - 1;
            map.Key(row, col);
            return base.VisitKey(context);
        }
        
        //public override object VisitRow([NotNull] MapEditorGrammarParser.RowContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public override object VisitCol([NotNull] MapEditorGrammarParser.ColContext context)
        //{
        //    throw new NotImplementedException();
        //}





        //public object VisitTerminal(ITerminalNode node)
        //{
        //    throw new NotImplementedException();
        //}
        //public object VisitErrorNode(IErrorNode node)
        //{
        //    throw new NotImplementedException();
        //}
        //public object VisitChildren(IRuleNode node)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
