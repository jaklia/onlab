using Antlr4.Runtime.Misc;
using Robot.Commands;
using Robot.Grammar;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Robot.Visitors
{
    class TreeViewGeneratorVisitor : RobotGrammarBaseVisitor<TreeViewItem>
    {
        private Dictionary<string, List<CommandBase>> declaredFunctions;
        public bool ExpandAll { get; set; }

        public TreeViewGeneratorVisitor(Dictionary<string, List<CommandBase>> declaredFunctions)
        {
            this.declaredFunctions = declaredFunctions;
        }

        public override TreeViewItem VisitProgram([NotNull] RobotGrammarParser.ProgramContext context)
        {
            TreeViewItem item = new TreeViewItem();

            //if (context.functionDefinitions() != null)
            //{
            //    item = VisitFunctionDefinitions(context.functionDefinitions());
            //}
            //item.Header = "Functions";

            TreeViewItem newItem = VisitProgInstructionSet(context.progInstructionSet());
            newItem.Items.Add(item);
            newItem.Header = "program";
            if (ExpandAll)
            {
                newItem.ExpandSubtree();
            }
            return newItem;
        }

        public override TreeViewItem VisitProgInstructionSet([NotNull] RobotGrammarParser.ProgInstructionSetContext context)
        {
            TreeViewItem item = new TreeViewItem();
            foreach (var instruction in context.progInstruction())
            {
                item.Items.Add(VisitProgInstruction(instruction));
            }
            return item;
        }

        public override TreeViewItem VisitInstructionSet([NotNull] RobotGrammarParser.InstructionSetContext context)
        {
            TreeViewItem item = new TreeViewItem();
            foreach (var instruction in context.instruction())
            {
                item.Items.Add(VisitInstruction(instruction));
            }
            return item;
        }

        //public override TreeViewItem VisitFunctionDefinitions([NotNull] RobotGrammarParser.FunctionDefinitionsContext context)
        //{
        //    TreeViewItem item = new TreeViewItem();
        //    foreach (var function in context.functionDef())
        //    {
        //        item.Items.Add(VisitFunctionDef(function));
        //    }
        //    return item;
        //}

        public override TreeViewItem VisitProgInstruction([NotNull] RobotGrammarParser.ProgInstructionContext context)
        {
            TreeViewItem newItem = new TreeViewItem();
            if (context.functionDef() != null)
                newItem = VisitFunctionDef(context.functionDef());
            else if (context.instruction() != null)
                newItem = VisitInstruction(context.instruction());
            return newItem;
        }

        public override TreeViewItem VisitInstruction([NotNull] RobotGrammarParser.InstructionContext context)
        {
            TreeViewItem newItem = new TreeViewItem();
            if (context.loopInstruction() != null)
                newItem = VisitLoopInstruction(context.loopInstruction());
            else if (context.loopWhileInstruction() != null)
                newItem = VisitLoopWhileInstruction(context.loopWhileInstruction());
            else if (context.dropInstruction() != null)
                newItem = VisitDropInstruction(context.dropInstruction());
            else if (context.moveInstruction() != null)
                newItem = VisitMoveInstruction(context.moveInstruction());
            else if (context.pickUpInstruction() != null)
                newItem = VisitPickUpInstruction(context.pickUpInstruction());
            else if (context.turnInstruction() != null)
                newItem = VisitTurnInstruction(context.turnInstruction());
            else if (context.functionCall() != null)
                newItem = VisitFunctionCall(context.functionCall());
            return newItem;
        }

        public override TreeViewItem VisitLoopInstruction([NotNull] RobotGrammarParser.LoopInstructionContext context)
        {
            //TreeViewItem item = new TreeViewItem();
            //item.Header = "loopInstruction";
            string headerText = context.LOOPCMD().GetText() + " " + context.repeatCnt().GetText();
            TreeViewItem newItem = VisitInstructionSet(context.instructionSet());
            newItem.Header = headerText;
            //item.Items.Add(newItem);
            //return item;
            return newItem;
        }

        public override TreeViewItem VisitFunctionDef([NotNull] RobotGrammarParser.FunctionDefContext context)
        {
            //TreeViewItem item = new TreeViewItem();
            //item.Header = "loopInstruction";
            string headerText = context.FUNCTIONCMD().GetText() + " " + context.functionName().GetText();
            TreeViewItem newItem = VisitInstructionSet(context.instructionSet());
            newItem.Header = headerText;
            //item.Items.Add(newItem);
            //return item;
            return newItem;
        }

        public override TreeViewItem VisitFunctionCall([NotNull] RobotGrammarParser.FunctionCallContext context)
        {
            //TreeViewItem item = new TreeViewItem();
            //item.Header = "loopInstruction";
          //  string headerText = context.functionName().GetText() + context.BRACKET1().GetText() + context.BRACKET2().GetText();
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.functionName().GetText() + context.BRACKET1().GetText() + context.BRACKET2().GetText();
            //item.Items.Add(newItem);
            //return item;
            return newItem;
        }

        public override TreeViewItem VisitLoopWhileInstruction([NotNull] RobotGrammarParser.LoopWhileInstructionContext context)
        {
            //TreeViewItem item = new TreeViewItem();
            //item.Header = "loopWhileInstruction";
            var conditionText = VisitCondition(context.condition()).Header;
            string headerText = context.LOOPWHILECMD().GetText() + " "
                              + context.BRACKET1().GetText() + conditionText + context.BRACKET2().GetText();
            TreeViewItem newItem = VisitInstructionSet(context.instructionSet());
            newItem.Header = headerText;
            //item.Items.Add(newItem);
            //return item;
            return newItem;
        }

        public override TreeViewItem VisitDropInstruction([NotNull] RobotGrammarParser.DropInstructionContext context)
        {
            //TreeViewItem item = new TreeViewItem();
            //item.Header = "dropInstruction";
            //item.Items.Add(new TreeViewItem().Header = context.DROPCMD().GetText() 
            //                                         + " "
            //                                         + context.itemId().GetText());
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.DROPCMD().GetText()
                           + " "
                           + context.itemId().GetText();
            //return item;
            return newItem;
        }

        public override TreeViewItem VisitMoveInstruction([NotNull] RobotGrammarParser.MoveInstructionContext context)
        {
            //TreeViewItem item = new TreeViewItem();
            //item.Header = "moveInstruction";
            //item.Items.Add(new TreeViewItem().Header = context.MOVECMD().GetText()
            //                                         + " "
            //                                         + context.moveAmount().GetText());
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.MOVECMD().GetText()
                           + " "
                           + context.moveAmount().GetText();
            //return item;
            return newItem;
        }

        public override TreeViewItem VisitPickUpInstruction([NotNull] RobotGrammarParser.PickUpInstructionContext context)
        {
            //TreeViewItem item = new TreeViewItem();
            //item.Header = "pickUpInstruction";
            //item.Items.Add(new TreeViewItem().Header = context.PICKUPCMD());
            //return item;
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.PICKUPCMD();
            return newItem;
        }

        public override TreeViewItem VisitTurnInstruction([NotNull] RobotGrammarParser.TurnInstructionContext context)
        {
            //TreeViewItem item = new TreeViewItem();
            //item.Header = "turnInstruction";
            //item.Items.Add(new TreeViewItem().Header = context.TURNCMD().GetText()
            //                                         + " "
            //                                         + context.dir().GetText());
            //return item;
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.TURNCMD().GetText()
                           + " "
                           + context.dir().GetText();
            return newItem;
        }

        public override TreeViewItem VisitCondition([NotNull] RobotGrammarParser.ConditionContext context)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = "";
            if (context.NOT() != null)
            {
                item.Header += context.NOT().GetText() == "!" ? "!" : "not ";
            }
            if (context.WALLCMD() != null)
            {
                item.Header += context.WALLCMD().GetText();
            }
            else if (context.FREECMD() != null)
            {
                item.Header += context.FREECMD().GetText();
            }
            return item;
        }

        /*  ezek  most nincsenek felhasználva 4!!!!!!
        
        public override TreeViewItem VisitDir([NotNull] RobotGrammarParser.DirContext context)
        {
            TreeViewItem item = new TreeViewItem();
            TreeViewItem newItem = new TreeViewItem();
            item.Header = "dir";
            if (context.leftDir() != null)
                newItem = VisitLeftDir(context.leftDir());
            else if (context.rightDir() != null)
                newItem = VisitRightDir(context.rightDir());
            item.Items.Add(newItem);
            return item;
        }

        public override TreeViewItem VisitItemId([NotNull] RobotGrammarParser.ItemIdContext context)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = context.INT().GetText();
            return item;
        }

        public override TreeViewItem VisitMoveAmount([NotNull] RobotGrammarParser.MoveAmountContext context)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = context.INT().GetText();
            return item;
        }

        public override TreeViewItem VisitRepeatCnt([NotNull] RobotGrammarParser.RepeatCntContext context)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = context.INT().GetText();
            return item;
        }

        public override TreeViewItem VisitLeftDir([NotNull] RobotGrammarParser.LeftDirContext context)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = context.GetText();
            return item;
        }

        public override TreeViewItem VisitRightDir([NotNull] RobotGrammarParser.RightDirContext context)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = context.GetText();
            return item;
        }
        
        */
    }
}
