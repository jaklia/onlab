using Robot.Grammar;
using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Robot.Errors;
using Robot.Model;

namespace Robot.Visitors
{
    class MapErrorVisitor : MapEditorGrammarBaseVisitor<object>
    {

        private int height = -1;
        private int width = -1;
        private bool hasStartField = false;
        private bool hasFinishField = false;
        private Board map; 

        private List<IErrorNode> antlrErrorList = new List<IErrorNode>();

        public List<ErrorLogItem> errorList { get; } = new List<ErrorLogItem>();

        public override object VisitErrorNode(IErrorNode node)
        {
            
            antlrErrorList.Add(node);  
            return base.VisitErrorNode(node);
        }

        public override object VisitHeight([NotNull] MapEditorGrammarParser.HeightContext context)
        {
            height = int.Parse(context.GetText());
            return base.VisitHeight(context);
        }

        public override object VisitWidth([NotNull] MapEditorGrammarParser.WidthContext context)
        {
            width = int.Parse(context.GetText());
            return base.VisitWidth(context);
        }

        public object Visit(IParseTree tree)
        {
            throw new NotImplementedException();
        }

        public object VisitChildren(IRuleNode node)
        {
            throw new NotImplementedException();
        }

        public object VisitCol([NotNull] MapEditorGrammarParser.ColContext context)
        {
            throw new NotImplementedException();
        }

        public object VisitFinish([NotNull] MapEditorGrammarParser.FinishContext context)
        {
            throw new NotImplementedException();
        }

        

        public object VisitKey([NotNull] MapEditorGrammarParser.KeyContext context)
        {
            throw new NotImplementedException();
        }

        public object VisitMap([NotNull] MapEditorGrammarParser.MapContext context)
        {
            throw new NotImplementedException();
        }

        public object VisitMapOptionRow([NotNull] MapEditorGrammarParser.MapOptionRowContext context)
        {
            throw new NotImplementedException();
        }

        public object VisitRow([NotNull] MapEditorGrammarParser.RowContext context)
        {
            throw new NotImplementedException();
        }

        public object VisitStart([NotNull] MapEditorGrammarParser.StartContext context)
        {
            throw new NotImplementedException();
        }

        public object VisitTerminal(ITerminalNode node)
        {
            throw new NotImplementedException();
        }

        public object VisitWall([NotNull] MapEditorGrammarParser.WallContext context)
        {
            throw new NotImplementedException();
        }

        public object VisitWalls([NotNull] MapEditorGrammarParser.WallsContext context)
        {
            throw new NotImplementedException();
        }

        
    }
}
