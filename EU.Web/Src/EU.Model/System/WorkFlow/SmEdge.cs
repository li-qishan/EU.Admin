using System;
using System.Collections.Generic;
using System.Text;
using EU.Domain;

namespace EU.Model.System.WorkFlow
{
    //流程线
    public class SmEdge : PersistPoco
    {
        public virtual SmProjectFlow SmProjectFlow { get; set; }

        public Guid? SmProjectFlowId { get; set; }

        public string edgeid { get; set; }

        public string index { get; set; }

        public string label { get; set; }

        public string shape { get; set; }

        public string source { get; set; }

        public string sourceAnchor { get; set; }

        public string target { get; set; }

        public string targetAnchor { get; set; }


        public string ConditionField { get; set; }
        public string Condition { get; set; }
        public string ConditionValue { get; set; }

    }
}
