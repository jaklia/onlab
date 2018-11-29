﻿using Robot.Grammar;
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
        const int MAX_HEIGHT = 20;
        const int MAX_WIDTH = 20;

        private int height = -1;
        private int width = -1;
        private int col = -1;
        private int row = -1;
        private bool hasStartField = false;
        private bool hasFinishField = false;
        private Board map = null;

        private List<IErrorNode> antlrErrorList = new List<IErrorNode>();
        public List<ErrorLogItem> errorList { get; } = new List<ErrorLogItem>();

        public bool NoErrors { get { return errorList.Count == 0 && antlrErrorList.Count == 0; } }

        private bool mapCreated { get { return map != null; } }

        public override object VisitErrorNode(IErrorNode node)
        {
            errorList.Add(new ErrorLogItem(node.GetText(), "ANTLR", "Map", node.Symbol.Line, node.Symbol.Column));
            antlrErrorList.Add(node);  
            return base.VisitErrorNode(node);
        }

        public override object VisitMap([NotNull] MapEditorGrammarParser.MapContext context)
        {
            var res = base.VisitMap(context);
            
            if (context.height() == null)
            {
                errorList.Add(new ErrorLogItem("Map height missing", "Custom", "Map", context.Start.Line, context.Start.Column));  // context.start.line/col
            }
            if (context.width() == null)
            {
                errorList.Add(new ErrorLogItem("Map width missing", "Custom", "Map", context.Start.Line, context.Start.Column));
            }
            
            if (height > 0 && height <= MAX_HEIGHT && width > 0 && width <= MAX_WIDTH)
            {
                map = new Board(height, width);
            }
            return res;
        }

        public override object VisitOptions([NotNull] MapEditorGrammarParser.OptionsContext context)
        {
            var res = base.VisitOptions(context);
            if (!hasStartField)
            {
                errorList.Add(new ErrorLogItem("Start field missing", "Custom", "Map", context.Start.Line, context.Start.Column));
            }
            if (!hasFinishField)
            {
                errorList.Add(new ErrorLogItem("Finish field missing", "Custom", "Map", context.Start.Line, context.Start.Column));
            }
            return res;
        }

        public override object VisitHeight([NotNull] MapEditorGrammarParser.HeightContext context)
        {
            try
            {
                height = int.Parse(context.GetText());
                if(height < 1)
                {
                    errorList.Add(new ErrorLogItem("Map height must be greater than 0", "Custom", "Map", context.Start.Line, context.Start.Column));
                } else if (height > MAX_HEIGHT)
                {
                    errorList.Add(new ErrorLogItem($"Map height must be less than {MAX_HEIGHT + 1}", "Custom", "Map", context.Start.Line, context.Start.Column));
                }
            }
            catch (Exception)
            {
                errorList.Add(new ErrorLogItem("Map height missing", "Custom", "Map", context.Start.Line, context.Start.Column));
            }
            return base.VisitHeight(context);
        }

        public override object VisitWidth([NotNull] MapEditorGrammarParser.WidthContext context)
        {
            try
            {
                width = int.Parse(context.GetText());
                if (width < 1)
                {
                    errorList.Add(new ErrorLogItem("Map width must be greater than 0", "Custom", "Map", context.Start.Line, context.Start.Column));
                }
                else if (width > MAX_WIDTH)
                {
                    errorList.Add(new ErrorLogItem($"Map width must be less than {MAX_WIDTH + 1}", "Custom", "Map", context.Start.Line, context.Start.Column));
                }
            }
            catch (Exception)
            {
                errorList.Add(new ErrorLogItem("Map width missing", "Custom", "Map", context.Start.Line, context.Start.Column));
            }
            return base.VisitWidth(context);
        }

        public override object VisitStart([NotNull] MapEditorGrammarParser.StartContext context)
        {
            var res = base.VisitStart(context);
            if (col > 0 && col <= width && row > 0 && row <= height)
            {
                map.SetStartField(row, col);
                hasStartField = true;
            }
            col = -1;
            row = -1;
            return res;
        }

        public override object VisitFinish([NotNull] MapEditorGrammarParser.FinishContext context)
        {
            var res = base.VisitFinish(context);
            if (col > 0 && col <= width && row > 0 && row <= height)
            {
                map.SetFinishField(row, col);
                hasFinishField = true;
            }
            col = -1;
            row = -1;
            return res;
        }

        public override object VisitCol([NotNull] MapEditorGrammarParser.ColContext context)
        {
            if (height > -1 && width > -1)
            {
                try
                {
                    col = int.Parse(context.GetText());
                    if (col < 1)
                    {
                        errorList.Add(new ErrorLogItem("Column value must be greater than 0", "Custom", "Map", context.Start.Line, context.Start.Column));
                    } else if (col > width)
                    {
                        errorList.Add(new ErrorLogItem("Column value must be less than or equal to width", "Custom", "Map", context.Start.Line, context.Start.Column));
                    }
                }
                catch (Exception)
                {
                    errorList.Add(new ErrorLogItem("Column value missing", "Custom", "Map", context.Start.Line, context.Start.Column));
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
                    row = int.Parse(context.GetText());
                    if (row < 1)
                    {
                        errorList.Add(new ErrorLogItem("Row value must be greater than 0", "Custom", "Map", context.Start.Line, context.Start.Column));
                    }
                    else if (row > width)
                    {
                        errorList.Add(new ErrorLogItem("Row value must be less than or equal to width", "Custom", "Map", context.Start.Line, context.Start.Column));
                    }
                }
                catch (Exception)
                {
                    errorList.Add(new ErrorLogItem("Row value missing", "Custom", "Map", context.Start.Line, context.Start.Column));
                }
            }
            return base.VisitRow(context);
        }

        public override object VisitKey([NotNull] MapEditorGrammarParser.KeyContext context)
        {
            return base.VisitKey(context);
        }

        public override object VisitWall([NotNull] MapEditorGrammarParser.WallContext context)
        {
            return base.VisitWall(context);
        }


        //public object Visit(IParseTree tree)
        //{
        //    throw new NotImplementedException();
        //}

        //public object VisitChildren(IRuleNode node)
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
        
        //public object VisitWalls([NotNull] MapEditorGrammarParser.WallsContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}