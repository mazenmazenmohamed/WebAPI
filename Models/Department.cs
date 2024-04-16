using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        [JsonIgnore]
        public virtual List<Employee> Employee { get; set; }


    }

}
