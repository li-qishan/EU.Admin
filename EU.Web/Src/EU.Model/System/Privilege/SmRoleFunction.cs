using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;
using EU.Entity;
using Newtonsoft.Json;

namespace EU.Model.System.Privilege
{
    public class SmRoleFunction : PersistPoco
    {
        [JsonIgnore]
        public virtual SmRole SmRole { get; set; }

        public Guid? SmRoleId { get; set; }

        [JsonIgnore]
        public virtual SmFunctionPrivilege SmFunction { get; set; }

        public Guid? SmFunctionId { get; set; }

        public string NoActionCode { get; set; }

    }

    [Entity(TableCnName = "", TableName = "SmRoleFunction")]
    public class SmRoleFunction1 : PersistPoco
    {

        public Guid? SmRoleId { get; set; }

        public Guid? SmFunctionId { get; set; }

        public string NoActionCode { get; set; }

    }

}
