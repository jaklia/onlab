using Robot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    class PickUpCommand : CommandBase
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
