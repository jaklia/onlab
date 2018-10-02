using Robot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    class DropCommand : CommandBase
    {

        private Game gameRef;
        int itemId;

        public DropCommand(Game game, int itemId)
        {
            gameRef = game;
            this.itemId = itemId;
        }

        public override void Do()
        {
            gameRef.DropItem(itemId);
        }

        public override void Undo()
        {
            itemId = gameRef.PickUpItem();
        }
    }
}
