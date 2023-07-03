

using System.ComponentModel.DataAnnotations;

namespace ApplicationAPI.Entities
{
    public class Group
    {
        [Key]
        public string Name { get; set; }
        public ICollection<Connection> connections { get; set; } = new List<Connection>();

        public Group() {}
        public Group(string name)
        {  
            Name = name;            
        }
    }
}