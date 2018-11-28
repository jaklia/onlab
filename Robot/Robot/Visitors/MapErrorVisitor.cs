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
            errorList.Add(new ErrorLogItem(node.GetText(), "ANTLR", "Map", 0));
            antlrErrorList.Add(node);  
            return base.VisitErrorNode(node);
        }

        public override object VisitMap([NotNull] MapEditorGrammarParser.MapContext context)
        {
            //var a = context.height().GetText();
            //var b = context.width().GetText();
            //var c = context.children;
            if (context.height() == null)
            {
                errorList.Add(new ErrorLogItem("Map height missing", "Custom", "Map", 0));
            }
            if (context.width() == null)
            {
                errorList.Add(new ErrorLogItem("Map width missing", "Custom", "Map", 0));
            }
            return base.VisitMap(context);
        }

        public override object VisitHeight([NotNull] MapEditorGrammarParser.HeightContext context)
        {
            try
            {
                height = int.Parse(context.GetText());
            }
            catch (Exception)
            {
                errorList.Add(new ErrorLogItem("Map height missing", "Custom", "Map", 0));
            }
            
            return base.VisitHeight(context);
        }

        public override object VisitWidth([NotNull] MapEditorGrammarParser.WidthContext context)
        {
            try
            {
                width = int.Parse(context.GetText());
            }
            catch (Exception)
            {
                errorList.Add(new ErrorLogItem("Map width missing", "Custom", "Map", 0));
            }
            
            return base.VisitWidth(context);
        }

        public override object VisitStart([NotNull] MapEditorGrammarParser.StartContext context)
        {

            return base.VisitStart(context);
        }

        public override object VisitFinish([NotNull] MapEditorGrammarParser.FinishContext context)
        {
            return base.VisitFinish(context);
        }

        public override object VisitCol([NotNull] MapEditorGrammarParser.ColContext context)
        {
            if (height > -1 && width > -1)
            {
                try
                {
                    int col = int.Parse(context.GetText());
                }
                catch (Exception)
                {
                    
                }
            }
            return base.VisitCol(context);
        }

        public override object VisitRow([NotNull] MapEditorGrammarParser.RowContext context)
        {
            if (height > -1 && width > -1)
            {
                try
                {
                    int row = int.Parse(context.GetText());
                }
                catch (Exception)
                {

                }
            }
            return base.VisitRow(context);
        }



        //public object Visit(IParseTree tree)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitChildren(IRuleNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitKey([NotNull] MapEditorGrammarParser.KeyContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitMapOptionRow([NotNull] MapEditorGrammarParser.MapOptionRowContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitTerminal(ITerminalNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitWall([NotNull] MapEditorGrammarParser.WallContext context)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitWalls([NotNull] MapEditorGrammarParser.WallsContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
