﻿using Robot.Model;

namespace Robot.Commands
{
    class PickUpCommand : SimpleCommand
    {

        private Game gameRef;
        private int itemId;

        public PickUpCommand(Game game)
        {
            gameRef = game;
        }

        public override void Do()
        {
            itemId = gameRef.PickUpItem();
        }

        public override void Undo()
        {
            gameRef.DropItem(itemId); 
        }
    }
}
