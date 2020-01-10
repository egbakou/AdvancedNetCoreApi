using System;

namespace System
{
    public interface IEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string RowVersion { get; set; }

        public bool IsDeleted { get; set; }

        public string FIN { get; set; }
    }
}
