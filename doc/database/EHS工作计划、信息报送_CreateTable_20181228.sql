--信息报送
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_INFOSUBMIT';
   if tabCount>0 then
      execute immediate 'drop table HRS_INFOSUBMIT cascade constraints';
   end if;   
end;
/
create table HRS_INFOSUBMIT
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
  infoname           NVARCHAR2(500),
  require            NVARCHAR2(2000),
  starttime          TIMESTAMP(4),
  endtime            TIMESTAMP(4),
  submituserid       NVARCHAR2(2000),
  submituseraccount  NVARCHAR2(2000),
  submitusername     NVARCHAR2(2000),
  submitdepartid     NVARCHAR2(2000),
  submitdepartname   NVARCHAR2(2000),
  submiteduserid     NVARCHAR2(2000),
  pct                number(10,2),
  remnum             number(4),
  remusername        nvarchar2(2000),                   
  remdepartname      nvarchar2(2000),
  issubmit           nvarchar2(2),
  infotype           nvarchar2(20)
);
alter table HRS_INFOSUBMIT add primary key (id);
-- Add comments to the table 
comment on table HRS_INFOSUBMIT  is '信息报送表';
-- Add comments to the columns 
comment on column HRS_INFOSUBMIT.infoname  is '报送名称';
comment on column HRS_INFOSUBMIT.require  is '报送要求';
comment on column HRS_INFOSUBMIT.starttime  is '报送开始时间';
comment on column HRS_INFOSUBMIT.endtime  is '报送结束时间';
comment on column HRS_INFOSUBMIT.submituseraccount  is '报送人帐号';
comment on column HRS_INFOSUBMIT.submitusername  is '报送人姓名';
comment on column HRS_INFOSUBMIT.submituserid  is '报送人id';
comment on column HRS_INFOSUBMIT.submiteduserid  is '已报送人id';
comment on column HRS_INFOSUBMIT.submitdepartid  is '报送部门id';
comment on column HRS_INFOSUBMIT.submitdepartname  is '报送部门名称';
comment on column HRS_INFOSUBMIT.pct  is '报送完成情况百分比';
comment on column HRS_INFOSUBMIT.remnum  is '剩余未报送人数';
comment on column HRS_INFOSUBMIT.remusername  is '剩余未报送人姓名';
comment on column HRS_INFOSUBMIT.remdepartname  is '剩余未报部门名称';
comment on column HRS_INFOSUBMIT.issubmit  is '是否发送';
comment on column HRS_INFOSUBMIT.infotype  is '类型（月报、季报、半年报、年报）';
/
create or replace trigger trgHRS_INFOSUBMIT before insert on HRS_INFOSUBMIT for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--信息报送明细表
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_INFOSUBMITDETAILS';
   if tabCount>0 then
      execute immediate 'drop table HRS_INFOSUBMITDETAILS cascade constraints';
   end if;   
end;
/
create table HRS_INFOSUBMITDETAILS
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
  contents           nvarchar2(2000),
  departname         nvarchar2(200),
  submitdate         TIMESTAMP(4),
  infoid             nvarchar2(36),
  issubmit           nvarchar2(2)
);
alter table HRS_INFOSUBMITDETAILS add primary key (id);
-- Add comments to the table 
comment on table HRS_INFOSUBMITDETAILS  is '信息报送明细表';
-- Add comments to the columns 
comment on column HRS_INFOSUBMITDETAILS.createuserid  is '报送人id';
comment on column HRS_INFOSUBMITDETAILS.createusername  is '报送人姓名';
comment on column HRS_INFOSUBMITDETAILS.createuserdeptcode  is '报送部门code';
comment on column HRS_INFOSUBMITDETAILS.departname  is '报送部门名称';
comment on column HRS_INFOSUBMITDETAILS.contents  is '报送内容';
comment on column HRS_INFOSUBMITDETAILS.submitdate  is '报送时间';
comment on column HRS_INFOSUBMITDETAILS.infoid  is '报送信息id';
comment on column HRS_INFOSUBMITDETAILS.issubmit  is '是否提交';
/
create or replace trigger trgHRS_INFOSUBMITDETAILS before insert on HRS_INFOSUBMITDETAILS for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--EHS计划申请表
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_PLANAPPLY';
   if tabCount>0 then
      execute immediate 'drop table HRS_PLANAPPLY cascade constraints';
   end if;   
end;
/
create table HRS_PLANAPPLY
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
  userId             NVARCHAR2(36),
  userName           NVARCHAR2(200),
  departId           NVARCHAR2(36),
  departName         NVARCHAR2(200),
  applyDate          TIMESTAMP(4),
  applyType          nvarchar2(20), 
  checkuseraccount   NVARCHAR2(2000),
  flowState          nvarchar2(200),
  baseId             nvarchar2(36)
);
alter table HRS_PLANAPPLY add primary key (id);
-- Add comments to the table 
comment on table HRS_PLANAPPLY  is 'EHS计划申请表';
-- Add comments to the columns 
comment on column HRS_PLANAPPLY.userId  is '申请人id';
comment on column HRS_PLANAPPLY.userName  is '申请人姓名';
comment on column HRS_PLANAPPLY.departId  is '申请部门id';
comment on column HRS_PLANAPPLY.departName  is '申请部门名称';
comment on column HRS_PLANAPPLY.applyDate  is '申请日期';
comment on column HRS_PLANAPPLY.applyType  is '申请类型（部门计划，个人计划）';
comment on column HRS_PLANAPPLY.checkuseraccount  is '审核（批）人帐号';
comment on column HRS_PLANAPPLY.flowState  is '流程状态';
comment on column HRS_PLANAPPLY.baseId  is '引用记录id';
/
create or replace trigger trgHRS_PLANAPPLY before insert on HRS_PLANAPPLY for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--EHS计划申请明细表
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_PLANDETAILS';
   if tabCount>0 then
      execute immediate 'drop table HRS_PLANDETAILS cascade constraints';
   end if;   
end;
/
create table HRS_PLANDETAILS
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
  contents           nvarchar2(2000),
  dutyUserId         nvarchar2(2000),
  dutyUserName       nvarchar2(2000),
  dutyDepartId       nvarchar2(2000),
  dutyDepartName     nvarchar2(2000),
  planFinDate        TIMESTAMP(4),
  realFinDate        TIMESTAMP(4),
  stdId              nvarchar2(2000),
  stdName            nvarchar2(2000),       
  isCancel           nvarchar2(2),
  changeReason       nvarchar2(2000),
  applyId            nvarchar2(36) not null,
  baseId             nvarchar2(36),
  ischanged          number(2),
  isMark             number(2)
);
alter table HRS_PLANDETAILS add primary key (id);
-- Add comments to the table 
comment on table HRS_PLANDETAILS  is 'EHS计划申请明细表';
-- Add comments to the columns 
comment on column HRS_PLANDETAILS.contents  is '工作内容';
comment on column HRS_PLANDETAILS.dutyUserId  is '责任人id';
comment on column HRS_PLANDETAILS.dutyUserName  is '责任人姓名';
comment on column HRS_PLANDETAILS.dutyDepartId  is '责任部门id';
comment on column HRS_PLANDETAILS.dutyDepartName  is '责任部门';
comment on column HRS_PLANDETAILS.planFinDate  is '计划完成日期';
comment on column HRS_PLANDETAILS.realFinDate  is '实际完成日期';
comment on column HRS_PLANDETAILS.stdId  is '管理标准id';
comment on column HRS_PLANDETAILS.stdName  is '管理标准名称';
comment on column HRS_PLANDETAILS.isCancel  is '是否取消计划';
comment on column HRS_PLANDETAILS.changeReason  is '变动原因';
comment on column HRS_PLANDETAILS.applyId  is '计划申请id';
comment on column HRS_PLANDETAILS.baseId  is '引用记录id';
comment on column HRS_PLANDETAILS.ischanged  is '是否变化';
comment on column HRS_PLANDETAILS.isMark  is '是否标记';
/
create or replace trigger trgHRS_PLANDETAILS before insert on HRS_PLANDETAILS for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--EHS计划申请审批（核）表
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_PLANCHECK';
   if tabCount>0 then
      execute immediate 'drop table HRS_PLANCHECK cascade constraints';
   end if;   
end;
/
create table HRS_PLANCHECK
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
  applyid            NVARCHAR2(36) not null,
  checkuserid        NVARCHAR2(2000), 
  checkusername      NVARCHAR2(2000),
  checkdeptid        NVARCHAR2(2000),
  checkdeptname      NVARCHAR2(2000),
  checkresult        NVARCHAR2(2),
  checkreason        nvarchar2(1000),
  checkdate          timestamp(4),
  checkbacktype      nvarchar2(20),
  checktype          nvarchar2(20)
);
alter table HRS_PLANCHECK add primary key (id);
-- Add comments to the table 
comment on table HRS_PLANCHECK  is 'EHS计划申请审批（核）表';
-- Add comments to the columns 
comment on column HRS_PLANCHECK.applyid  is '申请记录id';
comment on column HRS_PLANCHECK.checkuserid  is '审批人id';
comment on column HRS_PLANCHECK.checkusername  is '审批人名称';
comment on column HRS_PLANCHECK.checkdeptid  is '审批部门id';
comment on column HRS_PLANCHECK.checkdeptname  is '审批部门名称';
comment on column HRS_PLANCHECK.checkresult  is '审批结果（1：通过，0：未通过）';
comment on column HRS_PLANCHECK.checkreason  is '审批意见';
comment on column HRS_PLANCHECK.checkdate  is '审批日期';
comment on column HRS_PLANCHECK.checkbacktype  is '回退类型';
comment on column HRS_PLANCHECK.checktype  is '流程类型';
/
create or replace trigger trgHRS_PLANCHECK before insert on HRS_PLANCHECK for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/
