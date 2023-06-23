using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EU.CodeGenerator;
using EU.Core.Utilities;
using MySqlX.XDevAPI.Relational;
using NPOI.HPSF;

namespace JianLian.HDIS.CodeGenerator
{
    /// <summary>
    /// ControllerGenerator
    /// </summary>
    public class ControllerGenerator
    {
        /// <summary>
        /// 生成Controller，每次都是删除重建
        /// </summary>
        /// <param name="tables"></param>
        public static void Generator(string tableName)
        {
            string _tmpl = File.ReadAllText("..\\..\\..\\Tmpl\\ControllerTmpl.txt");
            string path = $"{Utilities.BasePath}EU.Web\\Controllers\\{Utilities.FileName}\\";
            _tmpl = _tmpl.Replace("@TableNameBase", tableName);
            _tmpl = _tmpl.Replace("@Prefix", Utilities.FileName);
            _tmpl = _tmpl.Replace("@CurrentTimeYear", DateTime.Now.Year.ToString());

            //生成文件
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Utilities.CreateFile($"{path}\\{tableName}Controller.cs", _tmpl, true);

            Console.WriteLine("ModelGenerator Completed!");
        }

    }
}
