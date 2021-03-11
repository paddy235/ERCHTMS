using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HseToolMange
{

    /// <summary>
    /// HSE观察者
    /// </summary>
    [Table("HSE_SECURITYOBSERVE")]
    public class HseObserveEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary> 
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 观察人
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVEUSER")]
        public string ObserveUser { get; set; }
        /// <summary>
        /// 观察人id
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVEUSERID")]
        public string ObserveUserid { get; set; }
        /// <summary>
        /// 观察部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTMENT")]
        public string Department { get; set; }
        /// <summary>
        /// 观察部门id
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTMENTID")]
        public string Departmentid { get; set; }
        /// <summary>
        /// 观察部门code
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTMENTCODE")]
        public string deptcode { get; set; }
        /// <summary>
        /// 任务
        /// </summary>
        /// <returns></returns>
        [Column("TASK")]
        public string Task { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        /// <returns></returns>
        [Column("AREA")]
        public string Area { get; set; }
        /// <summary>
        /// 观察时间
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVEDATE")]
        public DateTime ObserveDate { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 观察属性
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVETYPE")]
        public string ObserveType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        [Column("DESCRIBE")]
        public string Describe { get; set; }
        /// <summary>
        /// 是否纠正
        /// </summary>
        /// <returns></returns>
        [Column("ISMODIFY")]
        public int? IsModify { get; set; }
        /// <summary>
        /// 被观察者是否整改
        /// </summary>
        /// <returns></returns>
        [Column("ISTOMODIFY")]
        public int? IsToModify { get; set; }
        /// <summary>
        /// 列举措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASURES")]
        public string Measures { get; set; }
        /// <summary>
        /// 纠正措施
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVEACTION")]
        public string ObserveAction { get; set; }
        /// <summary>
        /// 风险程度
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVELEVEL")]
        public int? ObserveLevel { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVESTATE")]
        public string ObserveState { get; set; }
        /// <summary>
        /// 结论
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVERESULT")]
        public string ObserveResult { get; set; }
        
        /// <summary>
        /// 创建人id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 创建人部门code
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 创建人机构code
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 创建人部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPT")]
        public string CREATEUSERDEPT { get; set; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime MODIFYDATE { get; set; }
        /// <summary>
        /// 修改人id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 修改人名称
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
    }
}
