using Robot.Grammar;
using Antlr4.Runtime.Misc;
using Robot.Model;

namespace Robot.Visitors
{
    class MapBuilderVisitor : MapEditorGrammarBaseVisitor<object>
    {
        private Map map;
        public Map Map { get { return map; } }

        public override object VisitMap([NotNull] MapEditorGrammarParser.MapContext context)
        {
            var height = int.Parse(context.height().GetText());
            var width = int.Parse(context.width().GetText());
            map = new Map(width, height);
            return base.VisitMap(context);
        }


        public override object VisitMapOptionRow([NotNull] MapEditorGrammarParser.MapOptionRowContext context)
        {
            return base.VisitMapOptionRow(context);
        }

        public override object VisitStart([NotNull] MapEditorGrammarParser.StartContext context)
        {
            var col = int.Parse(context.col().GetText()) - 1;
            var row = int.Parse(context.row().GetText()) - 1;
            map.SetStartField(row, col);
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
            map.SetWall(row, col);
            return base.VisitWall(context);
        }
        
        public override object VisitFinish([NotNull] MapEditorGrammarParser.FinishContext context)
        {
            var col = int.Parse(context.col().GetText()) - 1;
            var row = int.Parse(context.row().GetText()) - 1;
            map.SetFinishField(row, col);
            return base.VisitFinish(context);
        }
        
        public override object VisitKey([NotNull] MapEditorGrammarParser.KeyContext context)
        {
            var col = int.Parse(context.col().GetText()) - 1;
            var row = int.Parse(context.row().GetText()) - 1;
            map.Key(row, col);
            return base.VisitKey(context);
        }
        
       

    }
}
