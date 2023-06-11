using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EU.Model.System.Privilege;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EU.EventBus
{
    public class testmodel
    {
       
        public Guid ID { get; set; }     

        public string Name { get; set; }
    }
}
