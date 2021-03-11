using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.CarManage
{
    public class KbsEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 占比
        /// </summary>
        public string Proportion { get; set; }
        /// <summary>
        /// 在线数量
        /// </summary>
        public int Num2 { get; set; }
    }

    public class LableModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        public string ID { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        /// <returns></returns>
        public string LableId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
        /// <summary>
        /// 类型0人员 1车辆
        /// </summary>
        /// <returns></returns>
        public int? Type { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        public string DeptName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        /// <returns></returns>
        public string DeptId { get; set; }
        /// <summary>
        /// 部门CODE
        /// </summary>
        /// <returns></returns>
        public string DeptCode { get; set; }
        /// <summary>
        /// 标签类型
        /// </summary>
        /// <returns></returns>
        public string LableTypeName { get; set; }
        /// <summary>
        /// 标签类型ID
        /// </summary>
        /// <returns></returns>
        public string LableTypeId { get; set; }
        /// <summary>
        /// 身份证号/驾驶员
        /// </summary>
        /// <returns></returns>
        public string IdCardOrDriver { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        public string Phone { get; set; }
        /// <summary>
        /// 绑定用户ID
        /// </summary>
        /// <returns></returns>
        public string UserId { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        /// <returns></returns>
        public DateTime? BindTime { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        /// <returns></returns>
        public string OperUserId { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 是否绑定 0为未绑定 1为绑定
        /// </summary>
        /// <returns></returns>
        public int IsBind { get; set; }

        /// <summary>
        /// 状态 在线/离线
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 电量
        /// </summary>
        public string Power { get; set; }
    }

    public class LableStatus
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        public string LableId { get; set; }
        /// <summary>
        /// 标签状态 0上线 1下线
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 标签绑定用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        public string DeptName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        /// <returns></returns>
        public string DeptId { get; set; }
        /// <summary>
        /// 部门CODE
        /// </summary>
        /// <returns></returns>
        public string DeptCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
    }

    public class KbssEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public string Proportion { get; set; }
        /// <summary>
        /// 在线占比
        /// </summary>
        public string OnProportion { get; set; }
        /// <summary>
        /// 在线数量
        /// </summary>
        public int OnNum { get; set; }

        /// <summary>
        /// 离线数量
        /// </summary>
        public int OffNum { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 离线占比
        /// </summary>
        public string OfflineProportion { get; set; }

        /// <summary>
        /// 区域code
        /// </summary>
        public string DistrictCode { get; set; }



    }


    public class SendData
    {
        /// <summary>
        /// 发送类名
        /// </summary>
        public string DataName { get; set; }
        /// <summary>
        /// 类json字符串
        /// </summary>
        public string EntityString { get; set; }
    }
    /// <summary>
    /// 作业管理监管成员
    /// </summary>
    public class SafeWorkUserEntity
    {
        public string WorkId { get; set; }
        public string Userid { get; set; }
        public string State { get; set; }
    }

    public class LableState
    {
        public string ID { get; set; }

        public string State { get; set; }

        public string Power { get; set; }

    }

    public class OffineEntity
    {
        /// <summary>
        /// 设备类型 0标签 1基站 2门禁 3摄像头
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 所属ID
        /// </summary>
        public string ID { get; set; }
    }

    public class KbsGPSPoint
    {
        public double x { get; set; }

        public double y { get; set; }

        public double z { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 区域点位
    /// </summary>
    public class KbsAreaLocation
    {
        /// <summary>
        /// 区域主键
        /// </summary>
        /// <returns></returns>
        public string DistrictID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public int? SortCode { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        public string DistrictName { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        /// <returns></returns>
        public string DistrictCode { get; set; }

        /// <summary>
        /// 父级主键
        /// </summary>
        /// <returns></returns>
        public string ParentID { get; set; }

        /// <summary>
        /// 公司主键
        /// </summary>
        /// <returns></returns>
        public string OrganizeId { get; set; }

        /// <summary>
        /// 关联表ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 坐标点
        /// </summary>
        public string PointList { get; set; }

        /// <summary>
        /// 模型集合
        /// </summary>
        public string ModelIds { get; set; }


    }
    /// <summary>
    /// 抓拍返回结果
    /// </summary>
    public class CaptureRtn
    {
        public string code { get; set; }

        public string msg { get; set; }

        public CaptureData data { get; set; }
    }

    public class CaptureData
    {
        public string picUrl { get; set; }
    }

    /// <summary>
    /// 区域统计类
    /// </summary>
    public class Areastatistics
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public string AreaID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 区域人数
        /// </summary>
        public int AreaNum { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int Total { get; set; }
    }

    /// <summary>
    /// 视频流返回结果
    /// </summary>
    public class CameraRtn
    {
        public string code { get; set; }

        public string msg { get; set; }

        public CameraData data { get; set; }
    }

    public class CameraData
    {
        public string url { get; set; }
    }

    /// <summary>
    /// 区域点位
    /// </summary>
    public class KbsAreaColor
    {
        /// <summary>
        /// 区域主键
        /// </summary>
        /// <returns></returns>
        public string DistrictID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public int? SortCode { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        public string DistrictName { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        /// <returns></returns>
        public string DistrictCode { get; set; }

        /// <summary>
        /// 父级主键
        /// </summary>
        /// <returns></returns>
        public string ParentID { get; set; }

        /// <summary>
        /// 公司主键
        /// </summary>
        /// <returns></returns>
        public string OrganizeId { get; set; }

        /// <summary>
        /// 关联表ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 坐标点
        /// </summary>
        public string PointList { get; set; }

        /// <summary>
        /// 模型集合
        /// </summary>
        public string ModelIds { get; set; }
        /// <summary>
        /// 风险级别
        /// </summary>
        public int Level { get; set; }
        /// 动态风险级别
        /// </summary>
        public int Level2 { get; set; }
        /// <summary>
        /// 隐患数量
        /// </summary>
        public int HtNum { get; set; }

    }

    public class AreaRiskLevel
    {
        /// <summary>
        /// 区域CODE
        /// </summary>
        public string areacode { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>
        public int gradeval { get; set; }
    }

    public class AreaHiddenCount
    {
        /// <summary>
        /// 区域CODE
        /// </summary>
        public string areacode { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaname { get; set; }
        /// <summary>
        /// 隐患数量
        /// </summary>
        public int htcount { get; set; }
    }
}
