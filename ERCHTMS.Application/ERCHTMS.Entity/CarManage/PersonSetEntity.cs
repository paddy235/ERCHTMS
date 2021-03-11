using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：人员进出门禁设置
    /// </summary>
    [Table("HJB_PERSONSET")]
    public class PersonSetEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PERSONSETID")]
        public string PersonSetId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODULETYPE")]
        public string ModuleType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("ISREFER")]
        public int Isrefer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEORS")]
        public string Createors { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.PersonSetId = string.IsNullOrEmpty(PersonSetId) ? Guid.NewGuid().ToString() : PersonSetId;
            this.CreateTime = DateTime.Now;
            this.Createors = OperatorProvider.Provider.Current().UserId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            //this.Id = keyValue;
            //this.ModifyDate = DateTime.Now;
            //this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }
}
