using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.BaseManage
{
    /// <summary>
    /// 部门树查询参数
    /// </summary>
   public class ConditionJson
   {
       /// <summary>
       /// 单位Id，多个逗号分隔
       /// </summary>
       public string Ids { set; get; }
       /// <summary>
       ///  单选或多选，0:单选，1:多选
       /// </summary>
       public int SelectMode { set; get; }
       /// <summary>
       ///查询模式
       /// </summary>
       public int Mode { set; get; }
       /// <summary>
       /// 页面带过来的部门ids,多个用逗号分隔(以设置默认选中状态)
       /// </summary>
       public string DeptIds { set; get; }
       /// <summary>
       /// 部门名称查询关键字
       /// </summary>
       public string KeyWord { set; get; }

       //部门编码
       public string DepartmentCode { get; set; }

   }
}
