using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.HseToolMange
{
    /// <summary>
    /// 安全观察内容标准
    /// </summary>
    [Table("HSE_SECURITYOBSERVENORM")]
    public  class HseObserveNormEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary> 
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary> 
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 内容
        /// </summary> 
        /// <returns></returns>
        [Column("OCONTENT")]
        public string Ocontent { get; set; }
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
