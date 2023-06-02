using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace EU.Domain
{
    /// <summary>
    /// 表主键
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TopBasePoco<T> where T : struct
    {
        private T _id;

        [Key]
        public T ID
        {
            get
            {
                if (typeof(T) == typeof(Guid))
                {
                    if (_id.ToString() == Guid.Empty.ToString())
                    {
                        _id = (T)Convert.ChangeType(Guid.NewGuid(), typeof(T));
                    }
                }
                return _id;
            }
            set
            {
                _id = value;
            }
        }
    }
}
