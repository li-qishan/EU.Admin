using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JianLian.HDIS.PublishHelper.Frm
{
    public partial class Form_CodeGenerator : Form
    {
        #region 初始化
        string Ip = "192.168.8.72";
        List<ModelView> m_Result = new List<ModelView>();
        Dictionary<string, List<ModelView>> m_All = new Dictionary<string, List<ModelView>>();
        public Form_CodeGenerator()
        {
            InitializeComponent();
            this.cmb_type.Text = this.cmb_type.Items[0].ToString();
            this.cmb_action.Text = this.cmb_action.Items[0].ToString();
            GetHttp(this.cmb_type.Text.Substring(this.cmb_type.Text.IndexOf('(') + 1, this.cmb_type.Text.IndexOf(')') - this.cmb_type.Text.IndexOf('(') - 1));
            var types = new List<string>();
            for (int i = this.cmb_type.Items.Count - 1; i >= 0; --i)
            {
                types.Add(this.cmb_type.Items[i].ToString().Substring(this.cmb_type.Items[i].ToString().IndexOf('(') + 1, this.cmb_type.Items[i].ToString().IndexOf(')') - this.cmb_type.Items[i].ToString().IndexOf('(') - 1));
            }
            m_Thread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate
            {
                foreach (var itemtype in types)
                {
                    if (m_All.ContainsKey(itemtype))
                        continue;
                    try
                    {
                        var result = Utility.GetHttpResult($"http://{Ip}:60010/swagger/{itemtype}/swagger.json", "");
                        if (result.Contains("\"components\"") && result.Contains("\"securitySchemes\""))
                        {
                            int index_components = result.IndexOf("\"components\"");
                            int index_securitySchemes = result.IndexOf("\"securitySchemes\"");
                            result = $"{{{result.Substring(index_components, index_securitySchemes - index_components - 6)}}}}}";
                            var components = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(result);

                            List<ModelView> modelResult = new List<ModelView>();
                            foreach (var item in components)
                            {
                                if (!(item.Value is string))
                                {
                                    var schemas = item.Value.schemas;
                                    foreach (var schema in schemas)
                                    {
                                        if (schema.Name.Contains("ServiceResult"))
                                            continue;
                                        if (!modelResult.Any(o => o.Name == schema.Name))
                                        {
                                            List<ModelPropertiesView> properties = new List<ModelPropertiesView>();
                                            if (schema.Value.properties != null)
                                            {
                                                foreach (var property in schema.Value.properties)
                                                {
                                                    var mp = new ModelPropertiesView
                                                    {
                                                        Name = property.Name.Substring(0, 1).ToUpper() + property.Name.Substring(1),
                                                        Description = property.Value.description ?? "",
                                                        Format = "",
                                                        Nullable = "",
                                                        MaxLength = "",
                                                    };
                                                    var str = property.ToString();
                                                    if (str.Contains("type\":"))
                                                    {
                                                        mp.Type = property.Value.type;
                                                    }
                                                    if (str.Contains("format\":"))
                                                    {
                                                        mp.Format = property.Value.format;
                                                    }
                                                    if (str.Contains("maxLength\":"))
                                                    {
                                                        mp.MaxLength = property.Value.maxLength;
                                                    }
                                                    if (str.Contains("nullable\":"))
                                                    {
                                                        mp.Nullable = property.Value.nullable;
                                                    }
                                                    properties.Add(mp);
                                                }
                                            }

                                            var m = new ModelView
                                            {
                                                Name = schema.Name,
                                                Description = schema.Value.description ?? "",
                                                Properties = properties,
                                                Required = ""
                                            };
                                            if (schema.Value.ToString().Contains("required\""))
                                            {
                                                m.Required = schema.Value.required.ToString();
                                            }
                                            modelResult.Add(m);
                                        }
                                    }
                                }
                            }

                            if (modelResult.Any() && !m_All.ContainsKey(itemtype))
                            {
                                m_All.Add(itemtype, modelResult);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }));
            m_Thread.Start();

        }

        System.Threading.Thread m_Thread;
        private void Form_CodeGenerator_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_Thread.Abort();
            }
            catch
            {
            }
        }
        private void GetHttp(string type)
        {
            if (!Utility.Ping(Ip))
            {
                return;
            }
            this.Text = "代码生成 -- 正在请求swagger，请稍等...";
            try
            {
                if (m_All.ContainsKey(type))
                {
                    m_Result = m_All[type];
                }
                else
                {
                    //var type = this.cmb_type.Text.Substring(this.cmb_type.Text.IndexOf('(') + 1, this.cmb_type.Text.IndexOf(')') - this.cmb_type.Text.IndexOf('(') - 1);
                    var result = Utility.GetHttpResult($"http://{Ip}:60010/swagger/{type}/swagger.json", "");
                    if (result.Contains("\"components\"") && result.Contains("\"securitySchemes\""))
                    {
                        int index_components = result.IndexOf("\"components\"");
                        int index_securitySchemes = result.IndexOf("\"securitySchemes\"");
                        result = $"{{{result.Substring(index_components, index_securitySchemes - index_components - 6)}}}}}";
                        var components = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(result);
                        m_Result.Clear();
                        foreach (var item in components)
                        {
                            if (!(item.Value is string))
                            {
                                var schemas = item.Value.schemas;
                                foreach (var schema in schemas)
                                {
                                    if (schema.Name.Contains("ServiceResult"))
                                        continue;
                                    if (!m_Result.Any(o => o.Name == schema.Name))
                                    {
                                        List<ModelPropertiesView> properties = new List<ModelPropertiesView>();
                                        if (schema.Value.properties != null)
                                        {
                                            foreach (var property in schema.Value.properties)
                                            {
                                                var mp = new ModelPropertiesView
                                                {
                                                    Name = property.Name.Substring(0, 1).ToUpper() + property.Name.Substring(1),
                                                    Description = property.Value.description ?? "",
                                                    Format = "",
                                                    Nullable = "",
                                                    MaxLength = "",
                                                };
                                                var str = property.ToString();
                                                if (str.Contains("type\":"))
                                                {
                                                    mp.Type = property.Value.type;
                                                }
                                                if (str.Contains("format\":"))
                                                {
                                                    mp.Format = property.Value.format;
                                                }
                                                if (str.Contains("maxLength\":"))
                                                {
                                                    mp.MaxLength = property.Value.maxLength;
                                                }
                                                if (str.Contains("nullable\":"))
                                                {
                                                    mp.Nullable = property.Value.nullable;
                                                }
                                                properties.Add(mp);
                                            }
                                        }

                                        var m = new ModelView
                                        {
                                            Name = schema.Name,
                                            Description = schema.Value.description ?? "",
                                            Properties = properties,
                                            Required = ""
                                        };
                                        if (schema.Value.ToString().Contains("required\""))
                                        {
                                            m.Required = schema.Value.required.ToString();
                                        }
                                        m_Result.Add(m);
                                    }
                                }
                            }
                        }
                    }
                    if (!m_All.ContainsKey(type))
                        m_All.Add(type, m_Result);
                }
                this.cmb_models.Items.Clear();
                if (m_Result.Any())
                {
                    this.cmb_models.Items.AddRange(m_Result.Select(o => o.Name).ToArray());
                    this.cmb_models.Text = this.cmb_models.Items[0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Text = "代码生成 -- 请求完成";
            }
        }
        #endregion

        #region 切换模块
        private void cmb_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetHttp(this.cmb_type.Text.Substring(this.cmb_type.Text.IndexOf('(') + 1, this.cmb_type.Text.IndexOf(')') - this.cmb_type.Text.IndexOf('(') - 1));
        }
        #endregion

        #region 快速搜索
        private void txt_tname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_tname.Text.Trim()))
                return;

            this.cmb_models.Items.Clear();
            if (!m_Result.Any(o => o.Name.ToLower().Contains($"{txt_tname.Text.Trim().ToLower()}")))
            {
                return;
            }
            try
            {
                this.cmb_models.Items.AddRange(m_Result.Where(o => o.Name.ToLower().Contains($"{txt_tname.Text.Trim().ToLower()}")).Select(o => o.Name).ToArray());

                this.cmb_models.Text = this.cmb_models.Items[0].ToString();
            }
            catch
            {

            }
        }
        #endregion

        #region 生成代码
        private void cmb_models_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateCode();
        }

        private void cmb_action_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateCode();
        }

        private void CreateCode()
        {
            if (string.IsNullOrEmpty(this.cmb_models.Text) || string.IsNullOrEmpty(this.cmb_action.Text))
            {
                return;
            }

            var model = m_Result.Where(o => o.Name == this.cmb_models.Text).FirstOrDefault();

            if (model == null)
            {
                return;
            }
            try
            {
                StringBuilder sb = new StringBuilder();
                switch (this.cmb_action.Text)
                {
                    case "pc_model":
                        {
                            int index = 0;
                            model.Properties?.ForEach(p =>
                            {
                                index++;
                                if (p.Name.ToLower() == "id")
                                    return;
                                sb.AppendLine($"\t\t\t{p.Name}: ''{(index < model.Properties.Count ? "," : "")}");
                            });
                            break;
                        }
                    case "pc_columns":
                        {
                            sb.AppendLine("\t\t\t\tcolumns: [");

                            int index = 0;
                            model.Properties?.ForEach(p =>
                            {
                                index++;
                                sb.AppendLine("\t\t\t\t\t{");
                                sb.AppendLine($"\t\t\t\t\t\ttitle: '{(p.Description.Contains(" ") ? p.Description.Substring(0, p.Description.IndexOf(" ")) : p.Description)}',");
                                sb.AppendLine($"\t\t\t\t\t\tdataIndex: '{p.Name}',");
                                sb.AppendLine($"\t\t\t\t\t\tscopedSlots: {{ customRender: '{p.Name}' }}");
                                sb.AppendLine("\t\t\t\t\t}" + (index < model.Properties.Count ? "," : ""));
                            });

                            sb.AppendLine("\t\t\t\t]");
                            break;
                        }
                    case "pc_form":
                        {
                            sb.AppendLine("\t\treturn [");

                            int index = 0;
                            model.Properties?.ForEach(p =>
                            {
                                index++;
                                sb.AppendLine("\t\t\t{");
                                sb.AppendLine("\t\t\t\tspan: 8,");
                                sb.AppendLine("\t\t\t\tclass: '',");
                                if (p.Description.Contains("`DIC."))
                                {
                                    sb.AppendLine($"\t\t\t\tcode: {p.Description.Substring(p.Description.IndexOf('`') + 1, p.Description.LastIndexOf('`') - p.Description.IndexOf('`') - 1)},");
                                }
                                sb.AppendLine($"\t\t\t\tlabel: '{(p.Description.Contains(" ") ? p.Description.Substring(0, p.Description.IndexOf(" ")) : p.Description)}',");
                                sb.AppendLine($"\t\t\t\tmodel: '{p.Name}',");
                                sb.AppendLine("\t\t\t\tplaceholder: '请输入',");
                                sb.AppendLine($"\t\t\t\t{GetRules(p, model.Required)}");
                                var type = GetType(p);
                                sb.AppendLine($"\t\t\t\ttype: '{type}',");
                                if (type == "input" && p.MaxLength != "")
                                {
                                    sb.AppendLine($"\t\t\t\tmaxlength: {p.MaxLength},");
                                }
                                sb.AppendLine($"\t\t\t\tisDic: {(p.Description.Contains("`DIC.") ? "true" : "false")},");
                                sb.AppendLine("\t\t\t\tchange: ''");

                                sb.AppendLine("\t\t\t}" + (index < model.Properties.Count ? "," : ""));
                            });

                            sb.AppendLine("\t\t]");
                            break;
                        }
                    case "android_model":
                        {
                            sb.AppendLine("package com.jianlian.ihdis.main.recipe.bean;");
                            sb.AppendLine();
                            sb.AppendLine("import java.util.List;");
                            sb.AppendLine();
                            sb.AppendLine($"public class {model.Name}Bean {{");
                            sb.AppendLine();
                            int index = 0;

                            StringBuilder sb_colunms = new StringBuilder();
                            StringBuilder sb_properties = new StringBuilder();
                            model.Properties?.ForEach(p =>
                            {
                                index++;
                                string ctype = GetJavaType(p);
                                bool b_list = ctype.Contains("List<");
                                string pname = p.Name.EndsWith("s") ? p.Name : (p.Name + (b_list ? "s" : ""));
                                string pnamelower = pname.Substring(0, 1).ToLower() + pname.Substring(1);
                                //字段
                                sb_colunms.AppendLine($"    /**");
                                sb_colunms.AppendLine($"     * {p.Description}");
                                sb_colunms.AppendLine("     */");
                                sb_colunms.AppendLine($"    private {ctype} {pname};");

                                //属性
                                sb_properties.AppendLine($"    public {ctype} get{pname}() {{");
                                sb_properties.AppendLine($"        return {pname};");
                                sb_properties.AppendLine("    }");
                                sb_properties.AppendLine();

                                sb_properties.AppendLine($"    public void set{pname}({ctype} {pnamelower}) {{");
                                sb_properties.AppendLine($"        {pname} = {pnamelower};");
                                sb_properties.AppendLine("    }");
                                if (index < model.Properties.Count)
                                    sb_properties.AppendLine();

                            });
                            sb.AppendLine(sb_colunms.ToString());
                            sb.AppendLine(sb_properties.ToString());
                            sb.AppendLine("}");
                            break;
                        }
                }

                this.txt_value.Text = sb.ToString();
            }
            catch { }
        }
        private string GetRules(ModelPropertiesView p, string rule)
        {
            if (rule.Contains($"\"{p.Name}\""))
            {
                return $"rules: [{{ required: true, message: '请选择{(p.Description.Contains(" ") ? p.Description.Substring(0, p.Description.IndexOf(" ")) : p.Description)}', trigger: 'change' }}],";
            }
            else
            {
                return $"rules: [{{ required: false, message: '', trigger: '' }}],";
            }
        }
        private string GetType(ModelPropertiesView p)
        {
            if (p.Description.Contains("`DIC."))
                return "select";
            if (p.Type == "integer")
                return "number";
            if (p.Format.ToLower().Contains("date"))
                return "datetime";
            return "input";
        }
        private string GetJavaType(ModelPropertiesView p)
        {
            if (p.Type == "number")
                return "double";
            if (p.Type == "boolean")
                return "boolean";
            if (p.Type == "integer")
                return "int";
            if (p.Type == "array")
                return $"List<{p.Name.Substring(0, p.Name.Length - 1)}Bean>";
            if (p.Format.ToLower().Contains("date"))
                return "Date";
            return "String";
        }
        #endregion

        #region 复制代码
        private void ll_Copy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_value.Text))
            {
                return;
            }
            try
            {
                Clipboard.SetDataObject(this.txt_value.Text, true);
                //MessageBox.Show("已经拷贝至系统剪贴板，请执行CTRL+V粘贴！", "提示");
            }
            catch
            {
            }
        }
        #endregion
    }

    #region SwaggerJson
    public class ModelView
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Required { get; set; }
        public List<ModelPropertiesView> Properties { get; set; }
    }
    public class ModelPropertiesView
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public string Nullable { get; set; }
        public string MaxLength { get; set; }
    }
    #endregion
}
