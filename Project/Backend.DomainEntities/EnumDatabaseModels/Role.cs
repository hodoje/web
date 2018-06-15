using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.EnumDatabaseModels
{
    public class Role
    {
        [Key]
        public virtual int Id
        {
            get { return (int)Type; }
            set { Type = (RoleEnum) value; }
        }

        public RoleEnum Type { get; set; }
    }
}
