using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;
using EU.Domain.System;
using Newtonsoft.Json;

namespace EU.Model.System.Privilege
{
    //角色模块权限
    public class SmRoleModule : PersistPoco
    {
        [JsonIgnore]
        public virtual SmRole SmRole { get; set; }

        public Guid? SmRoleId { get; set; }

        [JsonIgnore]
        public virtual SmModule SmModule { get; set; }

        public Guid? SmModuleId { get; set; }
    }
}
