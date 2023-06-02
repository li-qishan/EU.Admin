using System;
using System.Collections.Generic;
using System.Text;
using EU.Domain;
using Newtonsoft.Json;

namespace EU.Model.System.Privilege
{
    //用户角色
    public class SmUserRole : PersistPoco
    {
        [JsonIgnore]
        public virtual SmUser SmUser { get; set; }

        public Guid? SmUserId { get; set; }

        [JsonIgnore]
        public virtual SmRole SmRole { get; set; }

        public Guid? SmRoleId { get; set; }
    }
}
