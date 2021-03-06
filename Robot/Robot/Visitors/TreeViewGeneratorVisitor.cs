﻿using Antlr4.Runtime.Misc;
using Robot.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Robot.Visitors
{
    class TreeViewGeneratorVisitor : RobotGrammarBaseVisitor<TreeViewItem>
    {
        public bool ExpandAll { get; set; }

        public override TreeViewItem VisitProgram([NotNull] RobotGrammarParser.ProgramContext context)
        {
            TreeViewItem newItem = VisitInstructionSet(context.instructionSet());
            newItem.Header = "program";
            if (ExpandAll)
            {
                newItem.ExpandSubtree();
            }
            return newItem;
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
