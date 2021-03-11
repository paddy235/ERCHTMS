--特遣设备
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='BIS_SPECIALEQUIPMENT' and column_name='RELWORD';
   if colCount=0 then
      execute immediate 'alter table BIS_SPECIALEQUIPMENT add RELWORD nvarchar2(200)';   
   end if;
end;
/
alter table BIS_SPECIALEQUIPMENT modify equipmentname nvarchar2(256);

--普通设备
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='BIS_EQUIPMENT';
   if tabCount>0 then
      execute immediate 'drop table BIS_EQUIPMENT cascade constraints';
   end if;   
end;
/
create table BIS_EQUIPMENT
(
  id                    NVARCHAR2(64) not null,
  autoid                NUMBER(10),
  createuserid          NVARCHAR2(128),
  createuserdeptcode    NVARCHAR2(64),
  createuserorgcode     NVARCHAR2(64),
  createdate            TIMESTAMP(4),
  createusername        NVARCHAR2(128),
  modifydate            TIMESTAMP(4),
  modifyuserid          NVARCHAR2(128),
  modifyusername        NVARCHAR2(128),
  equipmentname         NVARCHAR2(256),
  district              NVARCHAR2(256),
  districtid            NVARCHAR2(64),
  districtcode          NVARCHAR2(64),
  useaddress            NVARCHAR2(200),  
  affiliation           NVARCHAR2(64),
  epibolydept           NVARCHAR2(64),
  epibolydeptid         NVARCHAR2(64),
  epibolyproject        NVARCHAR2(64),
  epibolyprojectid      NVARCHAR2(64),  
  equipmenttype         NVARCHAR2(64),  
  equipmentno           NVARCHAR2(64),
  specifications        NVARCHAR2(128),  
  securitymanageruser   NVARCHAR2(128),
  securitymanageruserid NVARCHAR2(256),  
  telephone             NVARCHAR2(64),
  controldept           NVARCHAR2(64),
  controldeptid         NVARCHAR2(64),
  controldeptcode       NVARCHAR2(64),
  relword               NVARCHAR2(200),
  purchasetime          TIMESTAMP(4),
  outputdeptname        NVARCHAR2(64),
  factoryno             NVARCHAR2(64),
  factorydate           TIMESTAMP(4),
  state                 NVARCHAR2(64),
  ischeck               NVARCHAR2(64),
  remark                NVARCHAR2(200)
);
alter table BIS_EQUIPMENT add primary key (id);
-- Add comments to the table 
comment on table BIS_EQUIPMENT is '普通设备';
-- Add comments to the columns 
comment on column BIS_EQUIPMENT.id  is '主键';
comment on column BIS_EQUIPMENT.equipmentname  is '设备名称';
comment on column BIS_EQUIPMENT.useaddress  is '使用地址';
comment on column BIS_EQUIPMENT.district  is '所属区域';
comment on column BIS_EQUIPMENT.affiliation  is '所属关系';
comment on column BIS_EQUIPMENT.epibolydept  is '外包单位';
comment on column BIS_EQUIPMENT.epibolyproject  is '外包工程';
comment on column BIS_EQUIPMENT.equipmenttype  is '设备类型';
comment on column BIS_EQUIPMENT.equipmentno  is '设备编号';
comment on column BIS_EQUIPMENT.specifications  is '规格型号';
comment on column BIS_EQUIPMENT.securitymanageruser  is '安全管理员';
comment on column BIS_EQUIPMENT.telephone  is '联系电话';
comment on column BIS_EQUIPMENT.controldept  is '管控部门';
comment on column BIS_EQUIPMENT.relword  is '关联词';
comment on column BIS_EQUIPMENT.purchasetime  is '购买日期';
comment on column BIS_EQUIPMENT.outputdeptname  is '制造单位名称';
comment on column BIS_EQUIPMENT.factoryno  is '出厂编号';
comment on column BIS_EQUIPMENT.factorydate  is '出厂日期';
comment on column BIS_EQUIPMENT.state  is '状态';
comment on column BIS_EQUIPMENT.ischeck  is '是否检查验收';
comment on column BIS_EQUIPMENT.remark  is '备注';
/
create or replace trigger trgEQUIPMENT before insert on BIS_EQUIPMENT for each row
begin  select autoid.nextval into :new.autoid from dual;  end;
/
--插入普通设备类型
delete from base_dataitemdetail d where d.itemid=(select itemid from base_dataitem i where i.itemcode='EQUIPMENTXTYPE');
delete from base_dataitem where itemcode='EQUIPMENTXTYPE';
insert into base_dataitem (ITEMID, PARENTID, ITEMCODE, ITEMNAME, ISTREE, ISNAV, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('c46b6039-a91e-47cd-a41f-4d1327692e95', '7BCDCAA4-2C65-444A-9D04-57F990585C92', 'EQUIPMENTXTYPE', '普通设备类型', 0, null, 1, 0, 1, null, '2018-09-18 16:44:36', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('3056945f-923e-4968-aece-e66c58d5386f', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '汽机辅助设备', '1', null, 'qjfzsb', null, 1, 0, 1, null, '2018-09-18 16:45:25', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('7133a032-a14c-4e9a-aae7-4492361aca3a', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '锅炉辅助设备', '2', null, 'glfzsb', null, 2, 0, 1, null, '2018-09-18 16:45:41', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('ae13d4cb-fe2d-46a4-b493-1c7237c11993', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '高压电气设备', '3', null, 'gydqsb', null, 3, 0, 1, null, '2018-09-18 16:46:00', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('55eeb259-7a86-4397-825a-ee671634e80f', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '低压电气设备', '4', null, 'dydqsb', null, 4, 0, 1, null, '2018-09-18 16:46:09', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('cffb5c2d-0a7f-480a-a943-3d39515b56b0', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '继电保护设备', '5', null, 'jdbhsb', null, 5, 0, 1, null, '2018-09-18 16:46:21', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('d6d61497-c179-49b1-b578-8dec7dd24323', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '热工保护设备', '6', null, 'rgbhsb', null, 6, 0, 1, null, '2018-09-18 16:46:31', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('f5480c79-62f0-45dd-9b34-83783e77f5ee', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '燃煤储运设备', '7', null, 'rmcysb', null, 7, 0, 1, null, '2018-09-18 16:46:41', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('45244d73-e5f2-4996-9f03-ec75194400d7', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '水处理设备', '8', null, 'sclsb', null, 8, 0, 1, null, '2018-09-18 16:46:50', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('b33a8632-a20f-4352-a667-ed9548f481f6', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '脱硫设备', '9', null, 'tlsb', null, 9, 0, 1, null, '2018-09-18 16:47:00', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('c82ac3b3-cb09-4d44-9355-cfc3653785aa', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '灰渣设备', '10', null, 'hzsb', null, 10, 0, 1, null, '2018-09-18 16:47:12', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('332a97b0-7daf-4aec-85fe-b84d4baadc99', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '安全设施', '11', null, 'aqss', null, 11, 0, 1, null, '2018-09-18 16:48:07', 'System', '超级管理员', null, null, null);

insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('d0ba3288-4508-441b-8090-324926b16da0', 'c46b6039-a91e-47cd-a41f-4d1327692e95', '0', null, '其他设备设施', '12', null, 'qtsbss', null, 12, 0, 1, null, '2018-09-18 16:48:30', 'System', '超级管理员', null, null, null);
/

