using SQLite;
using System;

namespace Models
{
    public class Contact : ICloneable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public object Clone()
        {
            return new Contact()
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                Phone = this.Phone
            };
        }
    }
}
