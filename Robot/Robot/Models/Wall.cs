namespace Robot.Model
{
    public class Wall : Field
    {
        public Wall( int row, int col) : base(row, col)
        {
        }

        public override bool AcceptsRobot()
        {
            return false;
        }

        public override Field Clone()
        {
            Wall w = new Wall(Row, Column);
            return w;
        }
    }
}
