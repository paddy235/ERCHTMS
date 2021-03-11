--反违章验收确认表
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='BIS_LLLEGALCONFIRM';
   if tabCount>0 then
      execute immediate 'drop table BIS_LLLEGALCONFIRM cascade constraints';
   end if;   
end;
/
create table BIS_LLLEGALCONFIRM
(
  id                 NVARCHAR2(36) not null,
  autoid             NUMBER(10),
  createuserid       NVARCHAR2(128),
  createuserdeptcode NVARCHAR2(64),
  createuserorgcode  NVARCHAR2(64),
  createdate         TIMESTAMP(4),
  createusername     NVARCHAR2(128),
  modifydate         TIMESTAMP(4),
  modifyuserid       NVARCHAR2(128),
  modifyusername     NVARCHAR2(128),
  lllegalid          VARCHAR2(64) not null,
  confirmpeople       VARCHAR2(64),
  confirmpeopleid     VARCHAR2(64),
  confirmdeptcode     VARCHAR2(64),
  confirmdeptname     VARCHAR2(200),
  confirmresult       VARCHAR2(100),
  confirmmind         VARCHAR2(2000),
  confirmtime         TIMESTAMP(4)
);
alter table BIS_LLLEGALCONFIRM add primary key (id);
-- Add comments to the table 
comment on table BIS_LLLEGALCONFIRM  is '反违章验收确认表';
-- Add comments to the columns 
comment on column BIS_LLLEGALCONFIRM.lllegalid  is '违章id';
comment on column BIS_LLLEGALCONFIRM.confirmpeople  is '确认人';
comment on column BIS_LLLEGALCONFIRM.confirmpeopleid  is '确认人';
comment on column BIS_LLLEGALCONFIRM.confirmdeptcode  is '确认部门';
comment on column BIS_LLLEGALCONFIRM.confirmdeptname  is '确认部门';
comment on column BIS_LLLEGALCONFIRM.confirmresult  is '确认结果';
comment on column BIS_LLLEGALCONFIRM.confirmmind  is '确认意见';
comment on column BIS_LLLEGALCONFIRM.confirmtime  is '确认时间';
/
create or replace trigger trgBIS_LLLEGALCONFIRM before insert on BIS_LLLEGALCONFIRM for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--反违章登记表
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='BIS_LLLEGALREGISTER' and column_name='BELONGDEPART';
   if colCount=0 then
      execute immediate 'alter table BIS_LLLEGALREGISTER add BELONGDEPARTID varchar(36)';   
      execute immediate 'alter table BIS_LLLEGALREGISTER add BELONGDEPART varchar(500)';   
   end if;
end;
/
comment on column BIS_LLLEGALREGISTER.BELONGDEPARTID  is '所属单位编号';
comment on column BIS_LLLEGALREGISTER.BELONGDEPART  is '所属单位';
/
--反违章验收表
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='BIS_LLLEGALACCEPT' and column_name='ISGRPACCEPT';
   if colCount=0 then
      execute immediate 'alter table BIS_LLLEGALACCEPT add ISGRPACCEPT varchar(2)';      
   end if;
end;
/
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='BIS_LLLEGALACCEPT' and column_name='CONFIRMUSERID';
   if colCount=0 then  
      execute immediate 'alter table BIS_LLLEGALACCEPT add CONFIRMUSERID varchar(64)';  
      execute immediate 'alter table BIS_LLLEGALACCEPT add CONFIRMUSERNAME varchar(64)';  
   end if;
end;
/
comment on column BIS_LLLEGALACCEPT.ISGRPACCEPT  is '是否集团验收';
comment on column BIS_LLLEGALACCEPT.CONFIRMUSERID  is '验收确认人编号';
comment on column BIS_LLLEGALACCEPT.CONFIRMUSERNAME  is '验收确认人姓名';
/
--反违章验收信息
create or replace view v_lllegalacceptinfo as
select  lllegalid,id,acceptpeopleid ,acceptpeople,acceptdeptname,acceptdeptcode ,acceptresult, acceptmind ,accepttime,acceptpic,isgrpaccept,confirmuserid,confirmusername,rn
from ( select lllegalid,id,acceptpeopleid ,acceptpeople,acceptdeptname,acceptdeptcode ,acceptresult, acceptmind ,accepttime,acceptpic,isgrpaccept,confirmuserid,confirmusername,
row_number() over(partition by lllegalid order by autoid desc ) rn from bis_lllegalaccept) where rn=1;
/
--反违章统计数据视图
create or replace view v_lllegalbaseinfo as
select a.createuserdeptcode,a.createuserorgcode,a.modifydate, a.createuserid, a.id,a.createdate, a.lllegalnumber,a.lllegaltype,b.itemname  lllegaltypename ,a.lllegaltime,a.lllegallevel,
c.itemname lllegallevelname, a.lllegalperson,a.lllegalpersonid,a.lllegalteam,a.lllegalteamcode,a.lllegaldepart,a.lllegaldepartcode,
a.lllegaldescribe,a.lllegaladdress ,a.lllegalpic,a.reformrequire,a.flowstate,a.createusername ,a.addtype,a.isexposure,
e.reformpeople,e.reformpeopleid,e.reformtel,e.reformdeptcode,e.reformdeptname,e.reformdeadline,e.reformfinishdate,e.reformstatus,e.reformmeasure,e.reformpic,
f.acceptpeopleid,f.acceptpeople,f.acceptdeptname,f.acceptdeptcode,f.acceptresult,f.acceptmind,f.accepttime,f.acceptpic,f.isgrpaccept,f.confirmuserid,f.confirmusername,a.reseverid,a.resevertype ,t.participant,a.isupsafety ,
g.personinchargeid,g.personinchargename,g.economicspunish,g.lllegalpoint,g.awaitjob,g.firstinchargeid,g.firstinchargename,g.firsteconomicspunish,g.firstlllegalpoint,g.firstawaitjob,
g.secondinchargeid,g.secondinchargename,g.secondeconomicspunish,g.secondlllegalpoint,g.secondawaitjob,h.approvedeptname ,
a.reseverone,a.resevertwo,a.reseverthree,a.reseverfour,a.reseverfive from bis_lllegalregister a
left join v_lllegalworkflow t on a.id = t.id
left join base_dataitemdetail  b on a.lllegaltype = b.itemdetailid
left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid
left join base_user d on a.createuserid = d.userid
left join v_lllegalreforminfo e on a.id = e.lllegalid
left join  v_lllegalacceptinfo  f on a.id = f.lllegalid
left join v_lllegalpunishinfo g on a.id = g.lllegalid
left join v_lllegalapproveinfo h on a.id = h.lllegalid
where a.flowstate !='违章登记' and a.flowstate !='违章完善' and a.flowstate !='违章核准';

--反违章全部数据视图
create or replace view v_lllegalallbaseinfo as
select a.belongdepart,a.belongdepartid, a.createuserdeptcode,a.createuserorgcode,a.modifydate, a.createuserid, a.id,a.createdate, a.lllegalnumber,a.lllegaltype,b.itemname  lllegaltypename ,a.lllegaltime,a.lllegallevel,
c.itemname lllegallevelname, a.lllegalperson,a.lllegalpersonid,a.lllegalteam,a.lllegalteamcode,a.lllegaldepart,a.lllegaldepartcode,
a.lllegaldescribe,a.lllegaladdress ,a.lllegalpic,a.reformrequire,a.flowstate,a.createusername,a.addtype,a.isexposure,
e.reformpeople,e.reformpeopleid,e.reformtel,e.reformdeptcode,e.reformdeptname,e.reformdeadline,e.reformfinishdate,e.reformstatus,e.reformmeasure,e.reformpic,
f.acceptpeopleid,f.acceptpeople,f.acceptdeptname,f.acceptdeptcode,f.acceptresult,f.acceptmind,f.accepttime,f.acceptpic,f.isgrpaccept,f.confirmuserid,f.confirmusername,a.reseverid,a.resevertype ,t.participant,a.isupsafety ,
g.personinchargeid,g.personinchargename,g.economicspunish,g.lllegalpoint,g.awaitjob,g.firstinchargeid,g.firstinchargename,g.firsteconomicspunish,g.firstlllegalpoint,g.firstawaitjob,
g.secondinchargeid,g.secondinchargename,g.secondeconomicspunish,g.secondlllegalpoint,g.secondawaitjob,h.approvedeptname,
a.reseverone,a.resevertwo,a.reseverthree,a.reseverfour,a.reseverfive,a.engineerid,a.engineername from bis_lllegalregister a
left join v_lllegalworkflow t on a.id = t.id
left join base_dataitemdetail  b on a.lllegaltype = b.itemdetailid
left join base_dataitemdetail  c on a.lllegallevel = c.itemdetailid
left join base_user d on a.createuserid = d.userid
left join v_lllegalreforminfo e on a.id = e.lllegalid
left join  v_lllegalacceptinfo  f on a.id = f.lllegalid
left join v_lllegalpunishinfo g on a.id = g.lllegalid
left join v_lllegalapproveinfo h on a.id = h.lllegalid;
--违章流程
--1.添加节点
delete from sys_wftbactivity where id in('dcc514fb-63dc-4087-8fb2-9f92772dea12','34235141-8d4b-45d6-92b8-c646395f2df8');
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('dcc514fb-63dc-4087-8fb2-9f92772dea12', '标准节点', '2018-11-01 09:29:45', 10014, '违章完善', 'stepnode', 150, 65, 176, 195, 'System', 'System', '2018-11-01 09:15:18', 'cc12f144-487b-4ac1-a12f-f842d620ca81', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('34235141-8d4b-45d6-92b8-c646395f2df8', '标准节点', '2018-11-01 09:29:46', 10015, '验收确认', 'stepnode', 150, 65, 1068, 268, 'System', 'System', '2018-11-01 09:17:27', 'cc12f144-487b-4ac1-a12f-f842d620ca81', null, null, null, null, null, null, null, null, null, null, null, null, null);
--2添加节点流向条件
delete from sys_wftbcondition where id in(
'6555d670-2818-4a79-b246-3fb6bb75f48a',
'feae9e92-4ee7-4674-8d20-eb0ecf9d9189',
'e3901199-9bfe-46bf-a0af-744fefb25bfb',
'1206dc08-8ca3-4b54-982b-76685b9473be',
'194b57de-9342-4796-95b7-3e57fdf9572a',
'2b331583-323e-4a79-9f60-8f7901159e36',
'c9336d73-2e6d-4c5e-8e5b-37a233d797cc',
'4cc4726a-6949-4ee4-b1e1-555a9e974786'
);
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('4cc4726a-6949-4ee4-b1e1-555a9e974786', '2018-11-02 13:51:27', null, '@{wfFlag}==5', '2', 'System', 'System', '2018-11-02 13:51:27', '8c67a66a-af78-41ba-9e98-8fc9a2760b07', 'dcc514fb-63dc-4087-8fb2-9f92772dea12', 'cc12f144-487b-4ac1-a12f-f842d620ca81');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('6555d670-2818-4a79-b246-3fb6bb75f48a', '2018-11-01 09:29:46', null, '@{wfFlag}==2', '2', 'System', 'System', '2018-11-01 09:29:46', '34235141-8d4b-45d6-92b8-c646395f2df8', '8c67a66a-af78-41ba-9e98-8fc9a2760b07', 'cc12f144-487b-4ac1-a12f-f842d620ca81');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('feae9e92-4ee7-4674-8d20-eb0ecf9d9189', '2018-11-01 09:29:46', null, '@{wfFlag}==3', '2', 'System', 'System', '2018-11-01 09:17:28', '5339940e-f2e7-4f06-863c-eef0bfe2c345', '34235141-8d4b-45d6-92b8-c646395f2df8', 'cc12f144-487b-4ac1-a12f-f842d620ca81');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('e3901199-9bfe-46bf-a0af-744fefb25bfb', '2018-11-01 09:29:46', null, '@{wfFlag}==1', '2', 'System', 'System', '2018-11-01 09:17:28', '34235141-8d4b-45d6-92b8-c646395f2df8', 'ef419a2e-4860-43b5-9b66-5314c694d6f8', 'cc12f144-487b-4ac1-a12f-f842d620ca81');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('1206dc08-8ca3-4b54-982b-76685b9473be', '2018-11-01 09:29:46', null, '@{wfFlag}==4', '2', 'System', 'System', '2018-11-01 09:15:18', 'edf3c6d6-92df-4715-aa0b-2c2f424c250b', 'dcc514fb-63dc-4087-8fb2-9f92772dea12', 'cc12f144-487b-4ac1-a12f-f842d620ca81');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('194b57de-9342-4796-95b7-3e57fdf9572a', '2018-11-01 09:29:46', null, '@{wfFlag}==3', '2', 'System', 'System', '2018-11-01 09:15:18', 'dcc514fb-63dc-4087-8fb2-9f92772dea12', '9dae9c4e-6aaf-471d-8516-f3dba7f0e95e', 'cc12f144-487b-4ac1-a12f-f842d620ca81');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('2b331583-323e-4a79-9f60-8f7901159e36', '2018-11-01 09:29:46', null, '@{wfFlag}==3', '2', 'System', 'System', '2018-11-01 09:15:18', '9dae9c4e-6aaf-471d-8516-f3dba7f0e95e', 'dcc514fb-63dc-4087-8fb2-9f92772dea12', 'cc12f144-487b-4ac1-a12f-f842d620ca81');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('c9336d73-2e6d-4c5e-8e5b-37a233d797cc', '2018-11-01 09:29:46', null, '@{wfFlag}==2', '2', 'System', 'System', '2018-11-01 09:15:18', 'dcc514fb-63dc-4087-8fb2-9f92772dea12', '8c67a66a-af78-41ba-9e98-8fc9a2760b07', 'cc12f144-487b-4ac1-a12f-f842d620ca81');
--省公司用户、省级用户
delete from base_dataitemdetail where itemname='GrpUser';
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('998d46f1-64d7-4c38-9a6e-ae3b8e2846b6', 'b04a89e4-7c4c-4c81-bbdc-9522c936e677', '0', null, 'GrpUser', '''300010'',''200010''', null, 'grpuser', null, 1, 0, 1, '集团用户、省级用户', '2018-10-31 17:06:14', 'System', '超级管理员', '2018-11-01 08:30:25', 'System', '超级管理员');
--违章数据范围
delete from base_dataitemdetail where itemname in('本人完善','本部门完善','本单位验收','本人确认','本单位确认');
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('fb6546ce-9764-4729-aab3-7c732d43d943', 'ba911dde-5fcf-4a06-b77a-2411eb4a5c59', '0', null, '本人完善', '本人完善', null, 'brws', null, 8, 0, 1, null, '2018-11-01 11:21:01', 'System', '超级管理员', null, null, null);
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('dd6e6d75-54d2-4d79-be71-e5b8a00b9458', 'ba911dde-5fcf-4a06-b77a-2411eb4a5c59', '0', null, '本部门完善', '本部门完善', null, 'bbmws', null, 9, 0, 1, null, '2018-11-01 11:21:16', 'System', '超级管理员', null, null, null);
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('dd6e6d75-54d2-4d79-be71-e5b8a00b9468', 'ba911dde-5fcf-4a06-b77a-2411eb4a5c59', '0', null, '本单位验收', '本单位验收', null, 'bdwys', null, 10, 0, 1, null, '2018-11-01 11:21:16', 'System', '超级管理员', null, null, null);
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('dd6e6d75-54d2-4d79-be71-e5b8a00b9478', 'ba911dde-5fcf-4a06-b77a-2411eb4a5c59', '0', null, '本人确认', '本人确认', null, 'brqr', null, 11, 0, 1, null, '2018-11-01 11:21:16', 'System', '超级管理员', null, null, null);
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('dd6e6d75-54d2-4d79-be71-e5b8a00b9488', 'ba911dde-5fcf-4a06-b77a-2411eb4a5c59', '0', null, '本单位确认', '本单位确认', null, 'bdwqr', null, 12, 0, 1, null, '2018-11-01 11:21:16', 'System', '超级管理员', null, null, null);
--违章流程状态编码
delete from base_dataitemdetail where itemname in('违章完善','验收确认');
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('7537efb4-c563-4a2f-9059-92e549138e4b', '71e8dfbc-fde9-463c-b12e-27fc398d1e31', '0', null, '违章完善', '违章完善', null, 'wzws', null, 1, 0, 1, null, '2018-11-01 10:40:20', 'System', '超级管理员', null, null, null);
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('dd260731-2ffc-41bf-b1ef-16921e53a4fd', '71e8dfbc-fde9-463c-b12e-27fc398d1e31', '0', null, '验收确认', '验收确认', null, 'ysqr', null, 5, 0, 1, null, '2018-11-01 15:11:01', 'System', '超级管理员', null, null, null);
update base_dataitemdetail set sortcode='0' where ITEMDETAILID='f4cb3816-4f9d-4cf6-bee5-ba40c71cbf70';
update base_dataitemdetail set sortcode='6' where ITEMDETAILID='6bc63040-5da2-41a3-bc60-3fa078cfdaa8';
--隐患排查标准根节点
delete from BIS_HTSTANDARD where id='0';
insert into BIS_HTSTANDARD (ID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, REMARK, NAME, ENCODE, PARENTID, ISPUBLIC, LEV)
values ('0', 'a78070d5-5f70-409e-845f-2351dc94f3a7', '00', '00', '2018-09-30 16:32:02', '管理员', null, null, null, null, '标准分类', '00', '-1', 1, 0);
