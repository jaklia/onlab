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
            string headerText = context.LOOPCMD().GetText() + " " + context.repeatCnt().GetText();
            TreeViewItem newItem = VisitInstructionSet(context.instructionSet());
            newItem.Header = headerText;
            return newItem;
        }

        public override TreeViewItem VisitFunctionDef([NotNull] RobotGrammarParser.FunctionDefContext context)
        {
            string headerText = context.FUNCTIONCMD().GetText() + " " + context.functionName().GetText();
            TreeViewItem newItem = VisitInstructionSet(context.instructionSet());
            newItem.Header = headerText;
            return newItem;
        }

        public override TreeViewItem VisitFunctionCall([NotNull] RobotGrammarParser.FunctionCallContext context)
        {
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.functionName().GetText() + context.BRACKET1().GetText() + context.BRACKET2().GetText();
            return newItem;
        }


        public override TreeViewItem VisitDropInstruction([NotNull] RobotGrammarParser.DropInstructionContext context)
        {
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.DROPCMD().GetText()
                           + " "
                           + context.itemId().GetText();
            return newItem;
        }

        public override TreeViewItem VisitMoveInstruction([NotNull] RobotGrammarParser.MoveInstructionContext context)
        {
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.MOVECMD().GetText()
                           + " "
                           + context.moveAmount().GetText();
            return newItem;
        }

        public override TreeViewItem VisitPickUpInstruction([NotNull] RobotGrammarParser.PickUpInstructionContext context)
        {
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.PICKUPCMD();
            return newItem;
        }

        public override TreeViewItem VisitTurnInstruction([NotNull] RobotGrammarParser.TurnInstructionContext context)
        {
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = context.TURNCMD().GetText()
                           + " "
                           + context.dir().GetText();
            return newItem;
        }

    

    }
}
