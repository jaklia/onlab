using Robot.Model;

namespace Robot.Commands
{
    class DropCommand : SimpleCommand
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
