using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.EnumDatabaseModels
{
    public class RideStatus
    {
        [Key]
        public virtual int Id
        {
            get { return (int) Type; }
            set { Type = (RideStatusEnum) value; }
        }

        public RideStatusEnum Type { get; set; }
    }
}
