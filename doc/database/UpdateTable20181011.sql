--特种设备验收操作人员字段长度
alter table EPG_SPECIFICTOOLS modify OPERATIONPEOPLE nvarchar2(2000);
alter table EPG_SPECIFICTOOLS modify OPERATIONPEOPLEID nvarchar2(2000);
--开收工会添加架子工人数
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='BIS_WORKMEETING' and column_name='JNUM';
   if colCount=0 then
      execute immediate 'alter table BIS_WORKMEETING add JNUM NUMBER(8)';   
   end if;
end;
/
comment on column BIS_WORKMEETING.JNUM  is '架子工数量';
/

--设备工器具添加设备类型（1：施工机具，2：特种设备）
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='EPG_TOOLS' and column_name='EQUIPTYPE';
   if colCount=0 then
      execute immediate 'alter table EPG_TOOLS add EQUIPTYPE varchar(2)';   
   end if;
end;
/
comment on column EPG_TOOLS.EQUIPTYPE  is '设备类型';
update EPG_TOOLS set EQUIPTYPE='1' where EQUIPTYPE is null;
/

--特种设备工器具审批流程(不能确定部门、角色ID是否与正式库一样)。
--注：最好，在正式环境中重新创建该是审批流程(执行部专工->执行部负责人->生技部负责人->安环部负责人)。
/*
delete from  bis_manypowercheck where MODULENAME='特种设备工器具';
insert into bis_manypowercheck (ID, AUTOID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, FLOWNAME, MODULENO, MODULENAME, CHECKDEPTID, CHECKDEPTCODE, CHECKDEPTNAME, CHECKROLEID, CHECKROLENAME, REMARK, SERIALNUM)
values ('0038ac27-42ca-4f19-8c7c-8e6f183f1168', 1, '38fab486-9b35-4d1a-bda0-f6a0e93e0ee4', '001', '001', '2018-10-15 09:00:07', 'F公司管理员', null, null, null, '执行部门专工', 'TZSBGQJ', '特种设备工器具', '-1', '-1', '执行部门', 'ddb2ffc9-f58f-417a-87a4-28a8c44103fd', '专工', null, 1.00);
insert into bis_manypowercheck (ID, AUTOID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, FLOWNAME, MODULENO, MODULENAME, CHECKDEPTID, CHECKDEPTCODE, CHECKDEPTNAME, CHECKROLEID, CHECKROLENAME, REMARK, SERIALNUM)
values ('d78151b2-dbcd-4cbe-a659-88f5172534ee', 2, '38fab486-9b35-4d1a-bda0-f6a0e93e0ee4', '001', '001', '2018-10-15 09:00:30', 'F公司管理员', null, null, null, '执行部门负责人', 'TZSBGQJ', '特种设备工器具', '-1', '-1', '执行部门', '27eb996b-1294-41d6-b8e6-837645a66819', '负责人', null, 2.00);
insert into bis_manypowercheck (ID, AUTOID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, FLOWNAME, MODULENO, MODULENAME, CHECKDEPTID, CHECKDEPTCODE, CHECKDEPTNAME, CHECKROLEID, CHECKROLENAME, REMARK, SERIALNUM)
values ('10c121c5-e5c8-4476-a1e4-5907172a4e06', 3, '38fab486-9b35-4d1a-bda0-f6a0e93e0ee4', '001', '001', '2018-10-15 09:01:02', 'F公司管理员', null, null, null, '生技部负责人', 'TZSBGQJ', '特种设备工器具', 'e0932a1b-08df-4754-842b-49c1aaf20e43', '001002', '生技部', '27eb996b-1294-41d6-b8e6-837645a66819', '负责人', null, 3.00);
insert into bis_manypowercheck (ID, AUTOID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, FLOWNAME, MODULENO, MODULENAME, CHECKDEPTID, CHECKDEPTCODE, CHECKDEPTNAME, CHECKROLEID, CHECKROLENAME, REMARK, SERIALNUM)
values ('97090be3-3a7b-4ae8-b42e-8705323f84f2', 4, '38fab486-9b35-4d1a-bda0-f6a0e93e0ee4', '001', '001', '2018-10-15 09:01:34', 'F公司管理员', null, null, null, '安环部负责人', 'TZSBGQJ', '特种设备工器具', 'b6fe8845-9427-4aa1-86c2-3e7efd4b40aa', '001001', '安环部', '27eb996b-1294-41d6-b8e6-837645a66819', '负责人', null, 4.00);
*/
