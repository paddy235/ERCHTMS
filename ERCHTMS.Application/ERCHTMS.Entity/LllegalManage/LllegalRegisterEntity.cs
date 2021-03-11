using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.LllegalManage
{
    /// <summary>
    /// 描 述：违章基本信息
    /// </summary>
    [Table("BIS_LLLEGALREGISTER")]
    public class LllegalRegisterEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        [Column("BELONGDEPART")]
        public string BELONGDEPART { get; set; }
        /// <summary>
        /// 所属单位编号
        /// </summary>
        [Column("BELONGDEPARTID")]
        public string BELONGDEPARTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 创建部门ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDEPTID")]
        public string CREATEDEPTID { get; set; }
        /// <summary>
        /// 创建部门名称
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDEPTNAME")]
        public string CREATEDEPTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 违章编号
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALNUMBER")]
        public string LLLEGALNUMBER { get; set; }
        /// <summary>
        /// 违章类型
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALTYPE")]
        public string LLLEGALTYPE { get; set; }
        /// <summary>
        /// 违章时间
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALTIME")]
        public DateTime? LLLEGALTIME { get; set; }
        /// <summary>
        /// 违章级别
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALLEVEL")]
        public string LLLEGALLEVEL { get; set; }
        /// <summary>
        /// 外包工程
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERID")]
        public string ENGINEERID { get; set; }
        /// <summary>
        /// 外包工程
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }

        /// <summary>
        /// 违章人员
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALPERSON")]
        public string LLLEGALPERSON { get; set; }
        /// <summary>
        /// 违章人员Id
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALPERSONID")]
        public string LLLEGALPERSONID { get; set; }
        /// <summary>
        /// 违章班组
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALTEAM")]
        public string LLLEGALTEAM { get; set; }
        /// <summary>
        /// 违章班组CODE
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALTEAMCODE")]
        public string LLLEGALTEAMCODE { get; set; }
        /// <summary>
        /// 违章部门
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALDEPART")]
        public string LLLEGALDEPART { get; set; }
        /// <summary>
        /// 违章部门Code
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALDEPARTCODE")]
        public string LLLEGALDEPARTCODE { get; set; }
        /// <summary>
        /// 违章描述
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALDESCRIBE")]
        public string LLLEGALDESCRIBE { get; set; }
        /// <summary>
        /// 违章地点
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALADDRESS")]
        public string LLLEGALADDRESS { get; set; }
        /// <summary>
        /// 违章图片
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALPIC")]
        public string LLLEGALPIC { get; set; }
        /// <summary>
        /// 整改要求
        /// </summary>
        /// <returns></returns>
        [Column("REFORMREQUIRE")]
        public string REFORMREQUIRE { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTATE")]
        public string FLOWSTATE { get; set; }
        /// <summary>
        /// 新增的数据类型
        /// </summary>
        /// <returns></returns>
        [Column("ADDTYPE")]
        public string ADDTYPE { get; set; }
        /// <summary>
        /// 是否曝光违章
        /// </summary>
        /// <returns></returns>
        [Column("ISEXPOSURE")]
        public string ISEXPOSURE { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        /// <returns></returns>
        [Column("RESEVERID")]
        public string RESEVERID { get; set; }
        /// <summary>
        /// 关联类型
        /// </summary>
        /// <returns></returns>
        [Column("RESEVERTYPE")]
        public string RESEVERTYPE { get; set; }
        /// <summary>
        /// 是否上报安全部门
        /// </summary>
        /// <returns></returns>
        [Column("ISUPSAFETY")]
        public string ISUPSAFETY { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        /// <returns></returns>
        [Column("RESEVERONE")]
        public string RESEVERONE { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        /// <returns></returns>
        [Column("RESEVERTWO")]
        public string RESEVERTWO { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        /// <returns></returns>
        [Column("RESEVERTHREE")]
        public string RESEVERTHREE { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        /// <returns></returns>
        [Column("RESEVERFOUR")]
        public string RESEVERFOUR { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        /// <returns></returns>
        [Column("RESEVERFIVE")]
        public string RESEVERFIVE { get; set; }
        /// <summary>
        /// 专业分类
        /// </summary>
        /// <returns></returns>
        [Column("MAJORCLASSIFY")]
        public string MAJORCLASSIFY { get; set; }
        /// <summary>
        /// 审核部门id
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYDEPTID")]
        public string VERIFYDEPTID { get; set; }
        /// <summary>
        /// 审核部门名称
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYDEPTNAME")]
        public string VERIFYDEPTNAME { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        /// <returns></returns>
        [Column("DUTYID")]
        public string DUTYID { get; set; }
        
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            if (string.IsNullOrEmpty(this.CREATEUSERID))
            {
                this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERNAME))
            {
                this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERDEPTCODE))
            {
                this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERORGCODE))
            {
                this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            }
            if (string.IsNullOrEmpty(this.DUTYID))
            {
                this.DUTYID = OperatorProvider.Provider.Current().DutyId;
            }
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}