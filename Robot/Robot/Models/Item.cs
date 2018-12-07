namespace Robot.Model
{
    public class Item 
    {
        int id;
        string name;

        public Item(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Item(Item other)
        {

            id = other.id;
            name = new string(other.name.ToCharArray());
        }

        public Item Clone()
        {
            return new Model.Item(id, new string(name.ToCharArray()));
        }
    }
}
