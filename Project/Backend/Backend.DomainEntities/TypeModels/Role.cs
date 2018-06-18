﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities.Models;

namespace DomainEntities.EnumDatabaseModels
{
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
