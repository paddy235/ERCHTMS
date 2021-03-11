--标准修订申请
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_STANDARDAPPLY';
   if tabCount>0 then
      execute immediate 'drop table HRS_STANDARDAPPLY cascade constraints';
   end if;   
end;
/
create table HRS_STANDARDAPPLY
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
  filename           VARCHAR2(200) not null,
  editdeptid         VARCHAR2(2000), 
  editdeptname       VARCHAR2(2000),
  editpersonid       VARCHAR2(2000),
  editperson         VARCHAR2(2000),
  editdate           TIMESTAMP(4),
  remark             VARCHAR2(2000),
  checkuserid        NVARCHAR2(1000),
  checkusername      NVARCHAR2(1000),
  checkdeptid        NVARCHAR2(1000),
  checkdeptname      NVARCHAR2(1000),
  flowstate          NVARCHAR2(60),
  checkbackflag      nvarchar2(2),
  checkbackuserid    NVARCHAR2(1000),
  checkbackusername  NVARCHAR2(1000),
  checkbackdeptid    NVARCHAR2(1000),
  checkbackdeptname  NVARCHAR2(1000)
);
alter table HRS_STANDARDAPPLY add primary key (id);
-- Add comments to the table 
comment on table HRS_STANDARDAPPLY  is '标准修订申请表';
-- Add comments to the columns 
comment on column HRS_STANDARDAPPLY.filename  is '文件名';
comment on column HRS_STANDARDAPPLY.editdeptid  is '修辑部门id';
comment on column HRS_STANDARDAPPLY.editdeptname  is '修订部门名称';
comment on column HRS_STANDARDAPPLY.editperson  is '修订人';
comment on column HRS_STANDARDAPPLY.editdate  is '修订日期';
comment on column HRS_STANDARDAPPLY.remark  is '备注';
comment on column HRS_STANDARDAPPLY.checkuserid  is '处理人id';
comment on column HRS_STANDARDAPPLY.checkusername  is '处理人姓名';
comment on column HRS_STANDARDAPPLY.checkdeptid  is '处理部门编号';
comment on column HRS_STANDARDAPPLY.checkdeptname  is '处理部门名称';
comment on column HRS_STANDARDAPPLY.flowstate  is '工作流状态';
comment on column HRS_STANDARDAPPLY.checkbackflag  is '驳回流程Flag值';
comment on column HRS_STANDARDAPPLY.checkbackuserid  is '驳回时审核人员id';
comment on column HRS_STANDARDAPPLY.checkbackusername  is '驳回时审核人员姓名';
comment on column HRS_STANDARDAPPLY.checkbackdeptid  is '驳回时审核部门id';
comment on column HRS_STANDARDAPPLY.checkbackdeptname  is '驳回时审核部门名称';
/
create or replace trigger trgHRS_STANDARDAPPLY before insert on HRS_STANDARDAPPLY for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--标准修订审批
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_STANDARDCHECK';
   if tabCount>0 then
      execute immediate 'drop table HRS_STANDARDCHECK cascade constraints';
   end if;   
end;
/
create table HRS_STANDARDCHECK
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
  recid              NVARCHAR2(36) not null,
  checkuserid        NVARCHAR2(2000), 
  checkusername      NVARCHAR2(2000),
  checkdeptid        NVARCHAR2(2000),
  checkdeptname      NVARCHAR2(2000),
  checkresult        NVARCHAR2(2),
  checkreason        nvarchar2(1000),
  checkdate          timestamp(4),
  checkbacktype      nvarchar2(64),
  checktype          nvarchar2(60)
);
alter table HRS_STANDARDCHECK add primary key (id);
-- Add comments to the table 
comment on table HRS_STANDARDCHECK  is '标准修订审批表';
-- Add comments to the columns 
comment on column HRS_STANDARDCHECK.recid  is '申请记录id';
comment on column HRS_STANDARDCHECK.checkuserid  is '审批人id';
comment on column HRS_STANDARDCHECK.checkusername  is '审批人名称';
comment on column HRS_STANDARDCHECK.checkdeptid  is '审批部门id';
comment on column HRS_STANDARDCHECK.checkdeptname  is '审批部门名称';
comment on column HRS_STANDARDCHECK.checkresult  is '审批结果（1：通过，0：未通过）';
comment on column HRS_STANDARDCHECK.checkreason  is '审批意见';
comment on column HRS_STANDARDCHECK.checkdate  is '审批日期';
comment on column HRS_STANDARDCHECK.checkbacktype  is '驳回类型';
comment on column HRS_STANDARDCHECK.checktype  is '审核类型';
/
create or replace trigger trgHRS_STANDARDCHECK before insert on HRS_STANDARDCHECK for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--标准修订申请审批流程
delete from sys_wftbprocess where id='76c4c857-a3e1-45eb-9c61-e8e5dd9bf880';
delete from sys_wftbactivity where processid='76c4c857-a3e1-45eb-9c61-e8e5dd9bf880';
delete from sys_wftbcondition where processid='76c4c857-a3e1-45eb-9c61-e8e5dd9bf880';
--1.基本信息
insert into sys_wftbprocess (ID, OPERDATE, AUTOID, NAME, MESSAGEMODE, REMARK, VERSION, OPERUSER, CREATEUSER, CREATEDATE, PARENTID, CATEGORY, ISSENDPHONEMESSAGE, ISBACKTOBEGINACTIVITY, CODE, ISSINGLEBACK, ISCUSTOMPROCESS, ISSTOP, ORGANID)
values ('76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', '2018-12-10 17:05:07', null, '标准修(订)审核发布', null, null, null, 'System', 'System', '2018-12-10 09:32:15', null, null, null, null, '05', null, null, null, null);
--2.节点信息
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('1b702b28-1746-4e61-ae72-ab411dfa9576', '开始节点', '2018-12-10 17:05:07', 8000, '申请人申请', 'startround', 150, 65, 61, 159, 'System', 'System', '2018-12-10 09:32:15', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('4cbf2d26-c628-4920-8408-cda6826de3db', '标准节点', '2018-12-10 17:05:07', 8001, '1级审核', 'stepnode', 150, 65, 62, 21, 'System', 'System', '2018-12-10 09:32:16', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('6015d28f-b38b-47ec-b96b-f911afa2f6ee', '标准节点', '2018-12-10 17:05:07', 8002, '2级审核', 'stepnode', 150, 65, 281, 21, 'System', 'System', '2018-12-10 09:32:16', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('1a920f22-f4f3-404a-b0f5-813dff369ac0', '标准节点', '2018-12-10 17:05:07', 8003, '审核分配会签', 'stepnode', 150, 65, 503, 22, 'System', 'System', '2018-12-10 09:32:16', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('16bdf6be-9c3a-48bf-b9ca-46c90d8c869f', '标准节点', '2018-12-10 17:05:07', 8004, '部门会签', 'stepnode', 150, 65, 502, 163, 'System', 'System', '2018-12-10 09:32:16', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('341d1592-0e86-4be9-9de8-8bbe10339095', '标准节点', '2018-12-10 17:05:07', 8005, '分配分委会', 'stepnode', 150, 65, 500, 294, 'System', 'System', '2018-12-10 09:32:16', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('3df46c65-aa95-4277-aba4-ac45070b491b', '标准节点', '2018-12-10 17:05:07', 8006, '分委会审核', 'stepnode', 150, 65, 282, 292, 'System', 'System', '2018-12-10 09:32:16', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('3b7accbc-7ddb-4ca6-9b7a-cb230f9dce8a', '标准节点', '2018-12-10 17:05:07', 8007, '审批', 'stepnode', 150, 65, 58, 290, 'System', 'System', '2018-12-10 09:32:16', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('77a65ce8-2a64-4e70-bb7b-aaffdd6ccbb3', '结束节点', '2018-12-10 17:05:07', 8008, '结束', 'endround', 150, 65, 279, 157, 'System', 'System', '2018-12-10 14:17:39', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880', null, null, null, null, null, null, null, null, null, null, null, null, null);
--3.条件信息
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('e822f1b7-2dd7-49ce-9cb7-5ddf49d89f6a', '2018-12-10 17:05:07', 9001, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-10 09:32:16', '1b702b28-1746-4e61-ae72-ab411dfa9576', '4cbf2d26-c628-4920-8408-cda6826de3db', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('17245d7c-60cd-4683-96aa-73ca4d8a19bc', '2018-12-10 17:05:07', 9002, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-10 09:32:16', '1b702b28-1746-4e61-ae72-ab411dfa9576', '6015d28f-b38b-47ec-b96b-f911afa2f6ee', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('469dfdae-2efc-4e8e-b2df-6bc0f9a056e8', '2018-12-10 17:05:07', 9003, '@{wfFlag}==3', '2', 'System', 'System', '2018-12-10 09:32:16', '1b702b28-1746-4e61-ae72-ab411dfa9576', '1a920f22-f4f3-404a-b0f5-813dff369ac0', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('2c52cd52-49c7-486e-bcdf-180b3d2a7903', '2018-12-10 17:05:08', 9004, '@{wfFlag}==4', '2', 'System', 'System', '2018-12-10 09:32:16', '1b702b28-1746-4e61-ae72-ab411dfa9576', '16bdf6be-9c3a-48bf-b9ca-46c90d8c869f', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('caae75c4-23d1-491d-89d1-b131d498615b', '2018-12-10 17:05:08', 9005, '@{wfFlag}==5', '2', 'System', 'System', '2018-12-10 09:32:16', '1b702b28-1746-4e61-ae72-ab411dfa9576', '3df46c65-aa95-4277-aba4-ac45070b491b', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('864d1403-6fda-4bf5-8040-960948f69deb', '2018-12-10 17:05:08', 9006, '@{wfFlag}==6', '2', 'System', 'System', '2018-12-10 09:32:16', '1b702b28-1746-4e61-ae72-ab411dfa9576', '3b7accbc-7ddb-4ca6-9b7a-cb230f9dce8a', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('09e6f6f1-af35-4d30-8e59-e691e923694f', '2018-12-10 17:05:07', 9007, '@{wfFlag}==7', '2', 'System', 'System', '2018-12-10 14:17:39', '1b702b28-1746-4e61-ae72-ab411dfa9576', '77a65ce8-2a64-4e70-bb7b-aaffdd6ccbb3', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('1d039e0a-0a6c-4a14-b681-2db2e56ee3b2', '2018-12-10 17:05:08', 9008, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-10 09:32:16', '4cbf2d26-c628-4920-8408-cda6826de3db', '6015d28f-b38b-47ec-b96b-f911afa2f6ee', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('438614bd-02e0-4bf6-9a8a-352fe9829c96', '2018-12-10 17:05:08', 9009, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-10 09:32:16', '4cbf2d26-c628-4920-8408-cda6826de3db', '1b702b28-1746-4e61-ae72-ab411dfa9576', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('6e5756f5-ae3e-40e4-abaa-9c91bc4ad468', '2018-12-10 17:05:08', 9010, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-10 09:32:16', '6015d28f-b38b-47ec-b96b-f911afa2f6ee', '1a920f22-f4f3-404a-b0f5-813dff369ac0', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('3ea90121-8600-4a38-9ac4-05ba2a14fbd7', '2018-12-10 17:05:08', 9011, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-10 09:32:16', '6015d28f-b38b-47ec-b96b-f911afa2f6ee', '1b702b28-1746-4e61-ae72-ab411dfa9576', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('8272e421-7f49-4e18-936b-fb06416cbc86', '2018-12-10 17:05:08', 9012, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-10 09:32:16', '1a920f22-f4f3-404a-b0f5-813dff369ac0', '16bdf6be-9c3a-48bf-b9ca-46c90d8c869f', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('8fd987d0-d256-4c1b-992b-984c6fd09c0e', '2018-12-10 17:05:08', 9013, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-10 09:32:16', '1a920f22-f4f3-404a-b0f5-813dff369ac0', '1b702b28-1746-4e61-ae72-ab411dfa9576', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('db467027-df2d-4c23-9540-2b342c57fe23', '2018-12-10 17:05:08', 9014, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-10 09:32:16', '16bdf6be-9c3a-48bf-b9ca-46c90d8c869f', '341d1592-0e86-4be9-9de8-8bbe10339095', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('3d9368e0-83ad-4629-b7d1-eb1e6870c202', '2018-12-10 17:05:08', 9015, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-10 09:32:16', '16bdf6be-9c3a-48bf-b9ca-46c90d8c869f', '1b702b28-1746-4e61-ae72-ab411dfa9576', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('5ea82e37-f263-4a1f-bc5c-59665aebc99b', '2018-12-10 17:05:08', 9016, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-10 09:32:16', '341d1592-0e86-4be9-9de8-8bbe10339095', '3df46c65-aa95-4277-aba4-ac45070b491b', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('7e98e72a-c450-467c-bafa-c28b16a52d04', '2018-12-10 17:05:08', 9017, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-10 09:32:16', '3df46c65-aa95-4277-aba4-ac45070b491b', '3b7accbc-7ddb-4ca6-9b7a-cb230f9dce8a', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('83cd627e-a2b4-4ec4-a7bd-66d778735e92', '2018-12-10 17:05:08', 9018, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-10 09:32:16', '3df46c65-aa95-4277-aba4-ac45070b491b', '1b702b28-1746-4e61-ae72-ab411dfa9576', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('c664b4f3-7f6f-403e-a820-6e5234bf4b58', '2018-12-10 17:05:08', 9019, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-10 09:32:16', '3b7accbc-7ddb-4ca6-9b7a-cb230f9dce8a', '1b702b28-1746-4e61-ae72-ab411dfa9576', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('11d47d82-9cd5-449d-9a3b-0024ac6878a0', '2018-12-10 17:05:07', 9020, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-10 14:17:39', '3b7accbc-7ddb-4ca6-9b7a-cb230f9dce8a', '77a65ce8-2a64-4e70-bb7b-aaffdd6ccbb3', '76c4c857-a3e1-45eb-9c61-e8e5dd9bf880');

--编码信息
delete from base_dataitemdetail where ITEMID in(select ITEMID from base_dataitem where itemcode in ('StandardSystem','PresidentApprove','CheckUserAccount','Check2Dept'));
delete from base_dataitem where itemcode in ('StandardSystem','PresidentApprove','CheckUserAccount','Check2Dept');
insert into base_dataitem (ITEMID, PARENTID, ITEMCODE, ITEMNAME, ISTREE, ISNAV, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('1122e766-6ebc-4035-80a0-446546354cb3', '7BCDCAA4-2C65-444A-9D04-57F990585C92', 'StandardSystem', '标准修(订)审核发布', 0, null, 0, 0, 1, null, '2018-12-10 11:18:31', 'System', '超级管理员', '2018-12-10 11:33:43', 'System', '超级管理员');
insert into base_dataitem (ITEMID, PARENTID, ITEMCODE, ITEMNAME, ISTREE, ISNAV, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('69b515cb-ec33-4b93-b330-1541a9053b33', '1122e766-6ebc-4035-80a0-446546354cb3', 'CheckUserAccount', '标准化办公室审核人员', 0, null, 2, 0, 1, null, '2018-12-10 11:37:51', 'System', '超级管理员', '2018-12-12 13:41:53', 'System', '超级管理员');
insert into base_dataitem (ITEMID, PARENTID, ITEMCODE, ITEMNAME, ISTREE, ISNAV, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('540a6d25-7114-4ed3-9523-21cf0afaa995', '1122e766-6ebc-4035-80a0-446546354cb3', 'PresidentApprove', '总经理审批', 0, null, 1, 0, 1, null, '2018-12-10 11:39:28', 'System', '超级管理员', null, null, null);
insert into base_dataitem (ITEMID, PARENTID, ITEMCODE, ITEMNAME, ISTREE, ISNAV, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('f82eeb52-130d-4829-a000-b4ceb3b7c6cb', '1122e766-6ebc-4035-80a0-446546354cb3', 'Check2Dept', '直接2级审核部门', 0, null, 3, 0, 1, null, '2018-12-12 11:58:51', 'System', '超级管理员', '2018-12-12 13:39:01', 'System', '超级管理员');

--编码明细
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('d1c870f1-77f3-4f7e-ba36-dfa6da314952', '69b515cb-ec33-4b93-b330-1541a9053b33', '0', null, '28803381-c588-4875-ad66-b2ba75e3d9cd', 'ahbgly,00318|185bbead-4c7c-44ab-8033-6cc42f8691ca,3343169a-9fc8-49f4-bd93-4c7fe8668278|安环部,生技部', null, '28803381-c588-4875-ad66-b2ba75e3d9cd', null, 1, 0, 1, '项目名称：机构id，项目值：审核人员帐号|部门id|部门名称；（格式：xxx,yyy,zzz|部门1id,部门2id|部门1名称,部门名称2）。', '2018-12-10 11:41:04', 'System', '超级管理员', '2018-12-11 19:58:04', 'System', '超级管理员');
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('e8a30a63-e2b0-4cb9-aac3-259ce750e093', 'f82eeb52-130d-4829-a000-b4ceb3b7c6cb', '0', null, '28803381-c588-4875-ad66-b2ba75e3d9cd', '7871a7b7-c0e5-4230-917f-1260d1aa4a5a', null, '28803381-c588-4875-ad66-b2ba75e3d9cd', null, 1, 0, 1, '项目名：机构编号；项目值：部门编号，多个用|分隔。', '2018-12-12 12:00:00', 'System', '超级管理员', '2018-12-12 13:16:33', 'System', '超级管理员');
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('b82bcaf0-e3a0-4249-8e83-d95a40dd71b7', '540a6d25-7114-4ed3-9523-21cf0afaa995', '0', null, '28803381-c588-4875-ad66-b2ba75e3d9cd', 'xssgly', null, '28803381-c588-4875-ad66-b2ba75e3d9cd', null, 1, 0, 1, '项目名：机构id,项目值：总经理帐号，格式：xssgly', '2018-12-10 11:41:51', 'System', '超级管理员', '2018-12-12 13:40:49', 'System', '超级管理员');
/
