using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ERCHTMS.Entity.CarManage
{
    public class GpsPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    public class GpsList
    {
        public List<GpsPoint> data { get; set; }

        public string ID { get; set; }
    }

    public class ReturnData
    {
        public int Code { get; set; }

        public int Info { get; set; }

        public int Count { get; set; }


    }


    public class DistrictGps
    {
        /// <summary>
        /// 区域主键
        /// </summary>
        /// <returns></returns>
        public string DistrictID { get; set; }
        ///// <summary>
        ///// 所属部门
        ///// </summary>
        ///// <returns></returns>
        //public string BelongDept { get; set; }
        ///// <summary>
        ///// 部门负责人
        ///// </summary>
        ///// <returns></returns>
        //public string DeptChargePerson { get; set; }

        ///// <summary>
        ///// 部门负责人主键
        ///// </summary>
        ///// <returns></returns>
        //public string DeptChargePersonID { get; set; }
        ///// <summary>
        ///// 创建日期
        ///// </summary>	
        //public DateTime? CreateDate { get; set; }
        ///// <summary>
        ///// 创建用户主键
        ///// </summary>		
        //public string CreateUserId { get; set; }
        ///// <summary>
        ///// 创建用户
        ///// </summary>	
        //public string CreateUserName { get; set; }
        ///// <summary>
        ///// 修改日期
        ///// </summary>		
        //public DateTime? ModifyDate { get; set; }
        ///// <summary>
        ///// 修改用户主键
        ///// </summary>		
        //public string ModifyUserId { get; set; }
        ///// <summary>
        ///// 修改用户
        ///// </summary>		
        //public string ModifyUserName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public int? SortCode { get; set; }
        ///// <summary>
        ///// 区域负责人
        ///// </summary>
        ///// <returns></returns>
        //public string DisreictChargePerson { get; set; }

        ///// <summary>
        ///// 区域负责人主键
        ///// </summary>
        ///// <returns></returns>
        //public string DisreictChargePersonID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        public string DistrictName { get; set; }
        ///// <summary>
        ///// 管控部门
        ///// </summary>
        ///// <returns></returns>
        //public string ChargeDept { get; set; }

        ///// <summary>
        ///// 管控部门主键
        ///// </summary>
        ///// <returns></returns>
        //public string ChargeDeptID { get; set; }
        ///// <summary>
        ///// 管控部门CODE
        ///// </summary>
        ///// <returns></returns>
        //public string ChargeDeptCode { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        /// <returns></returns>
        public string DistrictCode { get; set; }

        ///// <summary>
        ///// 所属公司
        ///// </summary>
        ///// <returns></returns>
        //public string BelongCompany { get; set; }
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

        ///// <summary>
        ///// 联系人
        ///// </summary>
        ///// <returns></returns>
        //public string LinkMan { get; set; }
        ///// <summary>
        ///// 联系人邮箱
        ///// </summary>
        ///// <returns></returns>
        //public string LinkMail { get; set; }

        ///// <summary>
        ///// 联系电话
        ///// </summary>
        ///// <returns></returns>
        //public string LinkTel { get; set; }

        ///// <summary>
        ///// 创建人所属部门编码
        ///// </summary>
        ///// <returns></returns>
        //public string CreateUserDeptCode { get; set; }

        ///// <summary>
        ///// 创建人所属机构编码
        ///// </summary>
        ///// <returns></returns>
        //public string CreateUserOrgCode { get; set; }

        ///// <summary>
        ///// 备注
        ///// </summary>
        ///// <returns></returns>
        //public string Description { get; set; }

        ///// <summary>
        ///// 关联公司的区域
        ///// </summary>
        ///// <returns></returns>
        //public string LinkToCompany { get; set; }

        ///// <summary>
        ///// 关联公司的区域ID
        ///// </summary>
        ///// <returns></returns>
        //public string LinkToCompanyID { get; set; }
        ///// <summary>
        ///// 区域坐标
        ///// </summary>
        ///// <returns></returns>
        //public string LatLng { get; set; }

        /// <summary>
        /// 关联表ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 坐标点
        /// </summary>
        public string PointList { get; set; }
    }

    public class SelectDeptData
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string deptid { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 新编码
        /// </summary>
        public string newcode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 父编码
        /// </summary>
        public string parentcode { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public string parentid { get; set; }
        /// <summary>
        /// 组织机构id
        /// </summary>
        public string oranizeid { get; set; }

        //是否是父节点
        public bool isparent { get; set; }
        //机构
        public int isorg { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public IList<SelectDeptData> children { get; set; }

        /// <summary>
        /// 是否可选
        /// </summary>
        public string isoptional { get; set; }

        public string ManagerId { get; set; }
        public string Manager { get; set; }
    }

    public class Route
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 一级菜单名称(危化品车辆路线、物料销售车辆、拜访车辆、其他车辆)
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 一级菜单排序码
        /// </summary>
        public int Sort1 { get; set; }
        /// <summary>
        /// 二级菜单名称（柴油、盐酸、液氮、一期灰库提货等）
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 二级菜单排序码
        /// </summary>
        public int Sort2 { get; set; }
        /// <summary>
        /// 三级菜单名称（一号路线、二号路线）
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 三级菜单排序码
        /// </summary>
        public int Sort3 { get; set; }
        /// <summary>
        /// 绑定字典ID
        /// </summary>
        public string GID { get; set; }
        /// <summary>
        /// 路线点位Json
        /// </summary>
        public string PointList { get; set; }

    }

    /// <summary>
    /// 用于跟底层GPS算法类交互实体
    /// </summary>
    public class CarAlgorithmEntity
    {
        /// <summary>
        /// 0为电厂班车 1为私家车 2为商务公车 3为拜访车辆 4为物料车辆 5为危化品车辆
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 当前记录ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// GPS设备ID
        /// </summary>
        public string GPSID { get; set; }

        /// <summary>
        /// GPS设备名称
        /// </summary>
        public string GPSName { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNo { get; set; }

        /// <summary>
        /// 所需路线名称
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 0位入场 1为离场
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 货物名称
        /// </summary>
        public string GoodsName { get; set; }
    }

    public class IotCar
    {
        public List<CarGpsData> result { get; set; }
    }

    public class CarGpsData
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceNo { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// 纬度类型
        /// </summary>
        public string LatType { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// 经度类型
        /// </summary>
        public string LngType { get; set; }

        /// <summary>
        /// 数据类型:1设备状态信息上报
        /// </summary>
        public int DataType { get; set; }


        /// <summary>
        /// 定位时间
        /// </summary>
        public DateTime? LocationTime { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        public double? Speed { get; set; }

        /// <summary>
        /// 电量
        /// </summary>
        public string ElectricQuantity { get; set; }

        /// <summary>
        /// 原始数据Id
        /// </summary>
        public string MetaDataId { get; set; }

        /// <summary>
        /// Gcj纬度
        /// </summary>
        public double? GcjLat { get; set; }

        /// <summary>
        /// Gcj经度
        /// </summary>
        public double? GcjLng { get; set; }

        /// <summary>
        /// Bd纬度
        /// </summary>
        public double? BdLat { get; set; }

        /// <summary>
        /// Bd经度
        /// </summary>
        public double? BdLng { get; set; }
    }
    public class parkList
    {
        public string code { get; set; }

        public string msg { get; set; }

        public List<parkData> data { get; set; }

    }

    /// <summary>
    ///人脸设备下发返回实体
    /// </summary>
    public class parkList1
    {
        public string code { get; set; }

        public string msg { get; set; }

        public string data { get; set; }
    }

    /// <summary>
    ///添加单个人脸返回实体
    /// </summary>
    public class faceentity
    {
        public string code { get; set; }

        public string msg { get; set; }

        public List<faceentity1> data { get; set; }
    }
    public class faceentity1
    {
        public string faceId { get; set; }

        public string faceUrl { get; set; }

        public string personId { get; set; }
    }


    /// <summary>
    ///人脸设备下发返回实体二
    /// </summary>
    public class parkList2
    {
        public string code { get; set; }

        public string msg { get; set; }

    }

    public class parkData
    {
        public string parkIndexCode { get; set; }
        /// <summary>
        /// 停车场Code
        /// </summary>
        public string parentParkIndexCode { get; set; }
        /// <summary>
        /// 停车场名称
        /// </summary>
        public string parkName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string createTime { get; set; }
        //修改时间
        public string updateTime { get; set; }

    }

    public class PreBill
    {

        public string code { get; set; }

        public string msg { get; set; }

        public PreBilldata data { get; set; }
    }

    public class PreBilldata
    {
        public string parkSyscode { get; set; }

        public string chargeRuleSyscode { get; set; }

        public string parkName { get; set; }

        public string inRecordSyscode { get; set; }

        public string enterTime { get; set; }

        public string vehiclePicUri { get; set; }

        public string plateNoPicUri { get; set; }

        public string aswSyscode { get; set; }

        public string plateNo { get; set; }

        public string cardNo { get; set; }

        public string parkTime { get; set; }

        public string billSyscode { get; set; }

        public int calcType { get; set; }

        public string supposeCost { get; set; }

        public string deduceCost { get; set; }

        public string paidCost { get; set; }

        public string totalCost { get; set; }

        public int isUsedCoupon { get; set; }

        public string couponCode { get; set; }

        public int couponUsedMsg { get; set; }

        public int delayTime { get; set; }

        public string currentDeduceCost { get; set; }
    }

    /// <summary>
    /// 支付返回实体
    /// </summary>
    public class receiptdata
    {
        public string code { get; set; }

        public string msg { get; set; }

        public string data { get; set; }
    }

    /// <summary>
    /// 拜访人员上传人脸信息
    /// </summary>
    public class FaceEntity
    {
        public string faceData { get; set; }
    }

    public class AddCarMsg
    {
        /// <summary>
        /// 返回码，0:接口业务处理成功，其它参考附录E.other.1
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回描述
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public AddCarEntity data { get; set; }
    }

    public class AddCarEntity
    {
        /// <summary>
        /// 批量添加成功的记录
        /// </summary>
        public List<CarSuccesses> successes { get; set; }
        /// <summary>
        /// 批量添加失败的记录
        /// </summary>
        public List<CarFailures> failures { get; set; }
    }

    public class CarSuccesses
    {
        /// <summary>
        /// 调用方指定Id
        /// </summary>
        public string clientId { get; set; }
        /// <summary>
        /// 服务端生成的唯一标识
        /// </summary>
        public string vehicleId { get; set; }
    }

    public class CarFailures
    {
        /// <summary>
        /// 调用方指定Id
        /// </summary>
        public string clientId { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }

    }


    public class cardList1
    {
        /// <summary>
        /// 卡号唯一
        /// </summary>
        public string cardNo { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string personId { get; set; }
        /// <summary>
        /// 卡类型 1IC卡
        /// </summary>
        public int cardType { get; set; }

    }

    /// <summary>
    /// 出入权限配置
    /// </summary>
    public class personDatas1
    {
        public List<string> indexCodes { get; set; }
        public string personDataType { get; set; }
    }

    /// <summary>
    /// 出入权限配置二
    /// </summary>
    public class resourceInfos1
    {
        /// <summary>
        /// 设备唯一编号
        /// </summary>
        public string resourceIndexCode { get; set; }
        public string resourceType { get; set; }
        /// <summary>
        /// 资源通道号
        /// </summary>
        public List<int> channelNos { get; set; }
    }




    public class DeptAddRtnMsg
    {
        /// <summary>
        /// 返回码 0：成功 其他：失败 参考附录E.other.1 资源目录错误码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 返回描述
        /// </summary>
        public string msg { get; set; }

        public DeptRtnMsg data { get; set; }

    }

    public class DeptRtnMsg
    {
        /// <summary>
        /// 成功消息
        /// </summary>
        public List<DeptSuc> successes { get; set; }

        /// <summary>
        /// 失败消息
        /// </summary>
        public List<DeptFail> failures { get; set; }
    }

    public class DeptSuc
    {
        /// <summary>
        /// 调用方指定Id
        /// </summary>
        public int clientId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string orgIndexCode { get; set; }
    }

    public class DeptFail
    {
        /// <summary>
        /// 调用方指定Id
        /// </summary>
        public int clientId { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }

    }


    public class UserAddRtnMsg
    {
        /// <summary>
        /// 返回码 0：成功 其他：失败 参考附录E.other.1 资源目录错误码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 返回描述
        /// </summary>
        public string msg { get; set; }

        public UserRtnMsg data { get; set; }

    }

    public class UserRtnMsg
    {
        /// <summary>
        /// 成功消息
        /// </summary>
        public List<UserSuc> successes { get; set; }

        /// <summary>
        /// 失败消息
        /// </summary>
        public List<UserFail> failures { get; set; }
    }

    public class UserSuc
    {
        /// <summary>
        /// 调用方指定Id
        /// </summary>
        public int clientId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string personId { get; set; }
    }

    public class UserFail
    {
        /// <summary>
        /// 调用方指定Id
        /// </summary>
        public int clientId { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string msg { get; set; }

    }
    #region 海康回调实体
    public class HikBack
    {
        /// <summary>
        /// 通知方法名称 默认事件类通知名称为”OnEventNotify”
        /// </summary>
        public string method { get; set; }
        [JsonProperty("params")]
        public FaceBack Params { get; set; }

    }

    public class FaceBack
    {
        /// <summary>
        /// 事件分类 门禁事件默认为”event_acs”
        /// </summary>
        public string ability { get; set; }

        public List<FaceEvents> Events { get; set; }
        /// <summary>
        /// 事件发送时间
        /// </summary>
        public string sendTime { get; set; }

    }

    public class FaceEvents
    {
        /// <summary>
        /// 事件详情
        /// </summary>
        public FaceData data { get; set; }
        /// <summary>
        /// 事件的唯一id
        /// </summary>
        public string eventId { get; set; }

        /// <summary>
        /// 事件类型码
        /// </summary>
        public int eventType { get; set; }

        /// <summary>
        /// 时间类型名称
        /// </summary>
        public string eventTypeName { get; set; }

        /// <summary>
        /// 事件产生时间
        /// </summary>
        public string happenTime { get; set; }

        /// <summary>
        /// 子类indexcode 门禁点唯一接入编码
        /// </summary>
        public string srcIndex { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string srcName { get; set; }

        /// <summary>
        /// 资源indexcode 控制器设备唯一接入编码
        /// </summary>
        public string srcParentIndex { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public string srcType { get; set; }
        /// <summary>
        /// 事件状态 0-瞬时 1-开始 2-停止 3-事件脉冲 4-事件联动结果更新 5-异步图片上传
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 脉冲超时时间 单位：秒，瞬时事件此字段填0
        /// </summary>
        public int timeout { get; set; }

    }

    public class FaceData
    {
        public ExtEventIdentityCardInfo ExtEventIdentityCardInfo { get; set; }

        /// <summary>
        /// 人员通道号
        /// </summary>
        public int ExtAccessChannel { get; set; }
        /// <summary>
        /// 报警输入/防区通道
        /// </summary>
        public int ExtEventAlarmInID { get; set; }
        /// <summary>
        /// 报警输出通道
        /// </summary>
        public int ExtEventAlarmOutID { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string ExtEventCardNo { get; set; }
        /// <summary>
        /// 事件输入通道
        /// </summary>
        public int ExtEventCaseID { get; set; }
        /// <summary>
        /// 事件类型代码
        /// </summary>
        public int ExtEventCode { get; set; }
        /// <summary>
        /// 通道事件信息
        /// </summary>
        public ExtEventCustomerNumInfo ExtEventCustomerNumInfo { get; set; }
        /// <summary>
        /// 门编号
        /// </summary>
        public int ExtEventDoorID { get; set; }
        /// <summary>
        /// 身份证图片
        /// </summary>
        public string ExtEventIDCardPictureURL { get; set; }
        /// <summary>
        /// 进出类型1：进 0：出 -1:未知要求：进门读卡器拨码设置为1，出门读卡器拨码设置为2
        /// </summary>
        public int ExtEventInOut { get; set; }
        /// <summary>
        /// 就地控制器id
        /// </summary>
        public int ExtEventLocalControllerID { get; set; }
        /// <summary>
        /// 主设备拨码
        /// </summary>
        public int ExtEventMainDevID { get; set; }
        /// <summary>
        /// 图片的url
        /// </summary>
        public string ExtEventPictureURL { get; set; }
        /// <summary>
        /// 读卡器id
        /// </summary>
        public int ExtEventReaderID { get; set; }
        /// <summary>
        /// 读卡器类别 0-无效1-IC读卡器2-身份证读卡器3-二维码读卡器4-指纹头v
        /// </summary>
        public int ExtEventReaderKind { get; set; }
        /// <summary>
        /// 报告上传通道 1-布防上传 2-中心组1上传 3-中心组2上传 0-无效
        /// </summary>
        public int ExtEventReportChannel { get; set; }
        /// <summary>
        /// 群组编号
        /// </summary>
        public int ExtEventRoleID { get; set; }
        /// <summary>
        /// 分控制器硬件ID
        /// </summary>
        public int ExtEventSubDevID { get; set; }
        /// <summary>
        /// 刷卡次数
        /// </summary>
        public int ExtEventSwipNum { get; set; }
        /// <summary>
        /// 事件类型，如普通门禁事件为0,身份证信息事件为1，客流量统计为2
        /// </summary>
        public int ExtEventType { get; set; }
        /// <summary>
        /// 多重认证序号
        /// </summary>
        public int ExtEventVerifyID { get; set; }
        /// <summary>
        /// 白名单单号 1-8，为0无效
        /// </summary>
        public int ExtEventWhiteListNo { get; set; }
        /// <summary>
        /// 事件上报驱动的时间
        /// </summary>
        public string ExtReceiveTime { get; set; }
        /// <summary>
        /// 事件流水号，为0无效
        /// </summary>
        public int Seq { get; set; }
        /// <summary>
        /// 图片服务器唯一编码
        /// </summary>
        public string svrIndexCode { get; set; }
    }

    public class ExtEventIdentityCardInfo
    {
        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birth { get; set; }
        /// <summary>
        /// 有效日期结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 身份证id
        /// </summary>
        public string IdNum { get; set; }
        /// <summary>
        /// 签发机关	
        /// </summary>
        public string IssuingAuthority { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 0-未知，其它参考附录A.13 民族类型
        /// </summary>
        public int Nation { get; set; }

        /// <summary>
        /// 性别0-未知 1-男 2-女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 有效日期开始时间
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 是否长期有效 0-否（有效截止日期有效）1-是（有效截止日期无效）
        /// </summary>
        public int TermOfValidity { get; set; }
    }

    public class ExtEventCustomerNumInfo
    {
        /// <summary>
        /// 通道号	
        /// </summary>
        public int AccessChannel { get; set; }
        /// <summary>
        /// 进人数
        /// </summary>
        public int EntryTimes { get; set; }
        /// <summary>
        /// 出人数	
        /// </summary>
        public int ExitTimes { get; set; }
        /// <summary>
        /// 总同行人数
        /// </summary>
        public int TotalTimes { get; set; }
    }



    public class HikCarBack
    {
        /// <summary>
        /// 通知方法名称 默认事件类通知名称为”OnEventNotify”
        /// </summary>
        public string method { get; set; }
        [JsonProperty("params")]
        public CarBack Params { get; set; }

    }

    public class CarBack
    {
        /// <summary>
        /// 事件分类 门禁事件默认为”event_acs”
        /// </summary>
        public string ability { get; set; }

        public List<CarEvents> Events { get; set; }
        /// <summary>
        /// 事件发送时间
        /// </summary>
        public string sendTime { get; set; }

    }

    public class CarEvents
    {
        /// <summary>
        /// 事件详情
        /// </summary>
        public CarData data { get; set; }
        /// <summary>
        /// 事件的唯一id
        /// </summary>
        public string eventId { get; set; }

        /// <summary>
        /// 子类indexcode 门禁点唯一接入编码
        /// </summary>
        public string srcIndex { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string srcType { get; set; }

        /// <summary>
        /// 事件类型码
        /// </summary>
        public int eventType { get; set; }

        /// <summary>
        /// 事件状态 0-瞬时 1-开始 2-停止 3-事件脉冲 4-事件联动结果更新 5-异步图片上传
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 脉冲超时时间 单位：秒，瞬时事件此字段填0
        /// </summary>
        public int timeout { get; set; }

        /// <summary>
        /// 事件产生时间
        /// </summary>
        public string happenTime { get; set; }

        /// <summary>
        /// 资源indexcode 控制器设备唯一接入编码
        /// </summary>
        public string srcParentIndex { get; set; }


    }

    public class CarData
    {
        /// <summary>
        /// 是否是黑名单
        /// </summary>
        public int alarmCar { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string cardNo { get; set; }

        /// <summary>
        /// 事件号 1:压线事件 2:上传图片 3:入场 4:出场 5:车牌矫正 6:图片重传
        /// </summary>
        public int eventCmd { get; set; }

        /// <summary>
        /// 事件号
        /// </summary>
        public string eventIndex { get; set; }

        /// <summary>
        /// 出入口编号
        /// </summary>
        public string gateIndex { get; set; }

        /// <summary>
        /// 出入口名称
        /// </summary>
        public string gateName { get; set; }

        /// <summary>
        /// 进出场类型 0进场 1出厂
        /// </summary>
        public int inoutType { get; set; }

        /// <summary>
        /// 车辆主品牌
        /// </summary>
        public string mainLogo { get; set; }

        /// <summary>
        /// 停车库编号
        /// </summary>
        public string parkIndex { get; set; }

        /// <summary>
        /// 停车库名称
        /// </summary>
        public string parkName { get; set; }

        /// <summary>
        /// 车辆图片
        /// </summary>
        public CarPic picUrl { get; set; }

        /// <summary>
        /// 图片服务器编号
        /// </summary>
        public string svrIndex { get; set; }

        /// <summary>
        /// 可信度
        /// </summary>
        public int plateBelieve { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        public int plateColor { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string plateNo { get; set; }

        /// <summary>
        /// 车牌类型
        /// </summary>
        public int plateType { get; set; }

        /// <summary>
        /// 车道编号
        /// </summary>
        public string roadwayIndex { get; set; }

        /// <summary>
        /// 车道名称
        /// </summary>
        public string roadwayName { get; set; }

        /// <summary>
        /// 车道类型 1：入场车道 2：出场不收费车道 3：出场缴费车道 4：中央缴费车道
        /// </summary>
        public int roadwayType { get; set; }

        /// <summary>
        /// 车辆自品牌
        /// </summary>
        public int subLogo { get; set; }

        /// <summary>
        /// 子品牌年款
        /// </summary>
        public int subModel { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// 车辆分类信息
        /// </summary>
        public int vehicleClass { get; set; }

        /// <summary>
        /// 车辆颜色
        /// </summary>
        public int vehicleColor { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public int vehicleType { get; set; }

        /// <summary>
        /// 车辆属性名称
        /// </summary>
        public string carAttributeName { get; set; }

        /// <summary>
        /// 放行结果数据
        /// </summary>
        public CarinResult inResult { get; set; }
    }

    public class CarinResult
    {
        /// <summary>
        /// 放行结果数据
        /// </summary>
        public CarrlsResult rlsResult { get; set; }
    }

    public class CarrlsResult
    {
        /// <summary>
        /// 放行权限
        /// </summary>
        public int releaseAuth { get; set; }
        /// <summary>
        /// 新体系放行结果
        /// </summary>
        public int releaseReason { get; set; }
        /// <summary>
        /// 放行结果
        /// </summary>
        public int releaseResult { get; set; }
        /// <summary>
        /// 放行原因
        /// </summary>
        public int releaseResultEx { get; set; }
        /// <summary>
        /// 放行方式
        /// </summary>
        public int releaseWay { get; set; }

    }

    public class CarPic
    {
        /// <summary>
        /// 车牌图片
        /// </summary>
        public string platePicUrl { get; set; }

        /// <summary>
        /// 车辆图片
        /// </summary>
        public string vehiclePicUrl { get; set; }
    }


    #endregion

    #region 海康接口实体

    public class UserSearch
    {
        public string code { get; set; }

        public string msg { get; set; }

        public UserSearchData data { get; set; }


    }

    public class UserSearchData
    {
        public int total { get; set; }

        public int pageNo { get; set; }

        public int pageSize { get; set; }

        public List<UserSearchEntity> list { get; set; }

    }

    public class UserSearchEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string personId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string personName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public string orgIndexCode { get; set; }
        /// <summary>
        /// 拼音
        /// </summary>
        public string pinyin { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public int certificateType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string certificateNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 部门ID节点树
        /// </summary>
        public string orgPath { get; set; }
        /// <summary>
        /// 部门名称节点树
        /// </summary>
        public string orgPathName { get; set; }
    }

    #endregion

    public class CarPicEntity
    {
        public string CarNo { get; set; }

        public string ImgUrl { get; set; }
    }

    public class Access
    {
        public string text { get; set; }

        public string id { get; set; }
    }

    #region 海康视频接口返回实体
    public class VideoReturn
    {
        public string code { get; set; }

        public string msg { get; set; }

        public VideoUrl data { get; set; }
    }

    public class VideoUrl
    {
        public string url { get; set; }
    }
    #endregion

    public class CarNoModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNo { get; set; }
        /// <summary>
        /// 0为拜访车辆 1为物料车辆 2为危化品车辆
        /// </summary>
        public int CarType { get; set; }
    }

    public class AreaModel
    {
        #region 实体成员
        /// <summary>
        /// 区域主键
        /// </summary>
        /// <returns></returns>
        public string DistrictID { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        public string BelongDept { get; set; }
        /// <summary>
        /// 部门负责人
        /// </summary>
        /// <returns></returns>
        public string DeptChargePerson { get; set; }

        /// <summary>
        /// 部门负责人主键
        /// </summary>
        /// <returns></returns>
        public string DeptChargePersonID { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>	
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>		
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>	
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>		
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        public int? SortCode { get; set; }
        /// <summary>
        /// 区域负责人
        /// </summary>
        /// <returns></returns>
        public string DisreictChargePerson { get; set; }

        /// <summary>
        /// 区域负责人主键
        /// </summary>
        /// <returns></returns>
        public string DisreictChargePersonID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        public string DistrictName { get; set; }
        /// <summary>
        /// 管控部门
        /// </summary>
        /// <returns></returns>
        public string ChargeDept { get; set; }

        /// <summary>
        /// 管控部门主键
        /// </summary>
        /// <returns></returns>
        public string ChargeDeptID { get; set; }
        /// <summary>
        /// 管控部门CODE
        /// </summary>
        /// <returns></returns>
        public string ChargeDeptCode { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        /// <returns></returns>
        public string DistrictCode { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        /// <returns></returns>
        public string BelongCompany { get; set; }
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
        /// 联系人
        /// </summary>
        /// <returns></returns>
        public string LinkMan { get; set; }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        /// <returns></returns>
        public string LinkMail { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        public string LinkTel { get; set; }

        /// <summary>
        /// 创建人所属部门编码
        /// </summary>
        /// <returns></returns>
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 创建人所属机构编码
        /// </summary>
        /// <returns></returns>
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }

        /// <summary>
        /// 关联公司的区域
        /// </summary>
        /// <returns></returns>
        public string LinkToCompany { get; set; }

        /// <summary>
        /// 关联公司的区域ID
        /// </summary>
        /// <returns></returns>
        public string LinkToCompanyID { get; set; }
        /// <summary>
        /// 区域坐标
        /// </summary>
        /// <returns></returns>
        public string LatLng { get; set; }

        public string AreaId { get; set; }

        /// <summary>
        /// 各个区域对应人数
        /// </summary>
        public int Numb { get; set; }
        #endregion
    }


    #region 更改人脸图片实体
    /// <summary>
    /// 获取人脸图片唯一标识
    /// </summary>
    public class UserPicEntity
    {
        public string code { get; set; }

        public string msg { get; set; }

        public UserPicPage data { get; set; }
    }


    public class UserPicPage
    {
        public int total { get; set; }

        public int pageNo { get; set; }


        public int pageSize { get; set; }

        public List<UserPicList> list { get; set; }
    }

    public class UserPicList
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public string personId { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string personName { get; set; }

        public List<personPhotoList> personPhoto { get; set; }
    }

    public class personPhotoList
    {
        /// <summary>
        /// 脸图片唯一标示
        /// </summary>
        public string personPhotoIndexCode { get; set; }
    }

    #endregion

    #region 门岗管理权限下发
    /// <summary>
    /// 添加或删除权限返回参数实体
    /// </summary>
    public class JurisdictionEntity
    {

        public string code { get; set; }

        public string msg { get; set; }
        public JurisdictionDate data { get; set; }

    }
    public class JurisdictionDate
    {
        public string taskId { get; set; }
    }


    /// <summary>
    /// 添加删除出入权限
    /// </summary>
    public class AddJurisdictionEntity
    {
        /// <summary>
        /// 调用接口返回唯一标示
        /// </summary>
        public string taskId { get; set; }
        /// <summary>
        /// 待下发设备信息
        /// </summary>
        public List<resourceInfos1> resourceInfos { get; set; }
        /// <summary>
        /// 类型 del 删除 add添加
        /// </summary>
        public string type { get; set; }
    }

    /// <summary>
    /// 查询权限配置单进度
    /// </summary>
    public class progress
    {
        public string code { get; set; }

        public string msg { get; set; }

        public progressDetails data { get; set; }
    }
    public class progressDetails
    {
        /// <summary>
        /// 任务进度，范围0~100
        /// </summary>
        public int percent { get; set; }
        /// <summary>
        /// 剩余时间，单位秒
        /// </summary>
        public int leftTime { get; set; }
        /// <summary>
        /// 下载是否结束
        /// </summary>
        public bool isFinished { get; set; }
        /// <summary>
        /// 配置单总数据量
        /// </summary>
        public int totalNum { get; set; }
        /// <summary>
        /// 配置成功数量
        /// </summary>
        public int successedNum { get; set; }
        /// <summary>
        /// 配置失败数量
        /// </summary>
        public int failedNum { get; set; }

    }


    #region 上传照片是否合格
    public class FaceTestingEntity
    {
        public string code { get; set; }

        public string msg { get; set; }

        public FaceTesting data { get; set; }
    }

    public class FaceTesting
    {
        /// <summary>
        /// 是否合格
        /// </summary>
        public bool checkResult { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public int faceScore { get; set; }

    }
    #endregion



    #endregion

    #region 车辆管理
    /// <summary>
    /// 查询车辆信息返回实体
    /// </summary>
    public class CarSelectEntity
    {
        public string code { get; set; }

        public CarSelectDetails data { get; set; }
    }
    public class CarSelectDetails
    {
        /// <summary>
        /// 数据行集
        /// </summary>
        public List<CarSelectItem> rows { get; set; }

    }
    public class CarSelectItem
    {
        /// <summary>
        /// 车辆记录唯一标识
        /// </summary>
        public string vehicleId { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string plateNo { get; set; }
    }
    #endregion

    public class GetCarEntity
    {
        /// <summary>
        /// 车辆记录ID    
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 车辆类型 0为电厂班车 1为私家车 2为商务公车 3为拜访车辆 4为物料车辆 5为危化品车辆 6临时车
        /// </summary>
        public int Type { get; set; }
    }

    public class RtnCarEntity
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNo { get; set; }
        /// <summary>
        /// 驾驶人
        /// </summary>
        public string Driver { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 定位设备ID
        /// </summary>
        public string GPSID { get; set; }
        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
    }

    /// <summary>
    /// 查询车辆坐标实体
    /// </summary>
    public class GetSafeHatGpsDataInput
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }

    /// <summary>
    /// 车辆定位记录类
    /// </summary>
    public class CarLocationData
    {
        public DateTime CreateDate { get; set; }

        public string DeviceNo
        {
            get;
            set;
        }

        public int? SpeedAlarm
        {
            get;
            set;
        }

        public int? PowerUnderVoltage
        {
            get;
            set;
        }

        public int? PowerFailure
        {
            get;
            set;
        }

        public int? ACC
        {
            get;
            set;
        }

        public int? IsLocation
        {
            get;
            set;
        }

        public int? LatitudeLine
        {
            get;
            set;
        }

        public int? LongitudeLine
        {
            get;
            set;
        }

        public int? OilCircuit
        {
            get;
            set;
        }

        public decimal? Latitude
        {
            get;
            set;
        }

        public decimal? Longitude
        {
            get;
            set;
        }

        public int? Height
        {
            get;
            set;
        }

        public int? Speed
        {
            get;
            set;
        }

        public int? Direction
        {
            get;
            set;
        }

        public DateTime UploadTime
        {
            get;
            set;
        }

        public int? Mileage
        {
            get;
            set;
        }

        public int? BatteryVoltage
        {
            get;
            set;
        }

        public int? Dismantle
        {
            get;
            set;
        }

        public int? IsBaseStation
        {
            get;
            set;
        }

        public int? StationAlarm
        {
            get;
            set;
        }

        public int? IsLight
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }

        public string Isp
        {
            get;
            set;
        }

        public string StationCount
        {
            get;
            set;
        }

        public string StationInfo
        {
            get;
            set;
        }
    }

    public class HikAccessNo
    {
        public string AccId { get; set; }

        public List<int> No { get; set; }
    }

    public class space
    {
        /// <summary>
        /// 类别 1测距 2测是否在面内
        /// </summary>
        public string type { get; set; }

        public spacedata data { get; set; }
    }

    public class spacedata
    {
        /// <summary>
        /// 摄像头坐标集
        /// </summary>
        public List<spacepnt> pnt { get; set; }
        /// <summary>
        /// 电子围栏坐标集
        /// </summary>
        public List<spacegeo> geo { get; set; }
    }

    public class spacepnt
    {
        public string id { get; set; }

        public double x { get; set; }

        public double y { get; set; }

        public List<double> coor { get; set; }
    }

    public class spacegeo
    {
        public string id { get; set; }
        /// <summary>
        /// 面的各个点位
        /// </summary>
        public List<double> coor { get; set; }
        /// <summary>
        /// type=0表示圆形,type=1表示多边形，type=2线
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 原型为半径 线为宽度
        /// </summary>
        public double distance { get; set; }
        /// <summary>
        /// 楼层编号
        /// </summary>
        public string floorID { get; set; }
    }

    public class AreaCode
    {
        /// <summary>
        /// 横坐标
        /// </summary>
        public double x { get; set; }
        /// <summary>
        /// 高度 无用
        /// </summary>
        public double y { get; set; }
        /// <summary>
        /// 纵坐标
        /// </summary>
        public double z { get; set; }
    }

    /// <summary>
    /// 电子围栏坐标集
    /// </summary>
    public class PositionsEntity
    {
        /// <summary>
        /// 电子围栏坐标
        /// </summary>
        public List<AreaCode> positions { get; set; }

        public double radius { get; set; }
        public string floorID { get; set; }
    }

    public class RtnSpace
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public string geoId { get; set; }
        /// <summary>
        /// 点位ID
        /// </summary>
        public string pntId { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        public int? result { get; set; }
    }


    /// <summary>
    /// 三维预警
    /// </summary>
    public class WarningEntity
    {
        /// <summary>
        /// 预警类型（0现场作业 1人员 2设备离线）
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 预警内容
        /// </summary>
        public string WarningContent { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string LiableName { get; set; }

        /// <summary>
        /// 责任人ID
        /// </summary>
        public string LiableId { get; set; }

        /// <summary>
        /// 关联ID 现场作业记录Id或人员Id
        /// </summary>
        public string BaseId { get; set; }

        /// <summary>
        /// 部门CODE
        /// </summary>
        public string deptCode { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string deptName { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 通知类型   工作预警     { 0 负责人未到岗 1工作成员未到岗 2 误入隔离区 }
        ///            人员预警     { 0 静止不动     1 SOS求助       2 误入隔离区 }
        ///            设备离线预警 { 0 基站故障     1 摄像头故障    2 标签电量低 }
        /// </summary>
        public int NoticeType { get; set; }
        public string typeIds { get; set; }
    }


    /// <summary>
    /// 获取海康门禁设备信息
    /// </summary>
    public class HikEqmitList
    {
        /// <summary>
        /// 返回码 0：成功 其他：失败 参考附录E.other.1 资源目录错误码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 返回描述
        /// </summary>
        public string msg { get; set; }
        public HikEqmitListItem data { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class HikEqmitListItem
    {
        public string total { get; set; }

        public List<HikEqmitListEntity> list { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HikEqmitListEntity
    {

        /// <summary>
        /// 设备唯一编号
        /// </summary>
        public string acsDevIndexCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string acsDevName { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public string acsDevIp { get; set; }

    }




}
