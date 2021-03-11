--��Ϣ����
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
comment on table HRS_INFOSUBMIT  is '��Ϣ���ͱ�';
-- Add comments to the columns 
comment on column HRS_INFOSUBMIT.infoname  is '��������';
comment on column HRS_INFOSUBMIT.require  is '����Ҫ��';
comment on column HRS_INFOSUBMIT.starttime  is '���Ϳ�ʼʱ��';
comment on column HRS_INFOSUBMIT.endtime  is '���ͽ���ʱ��';
comment on column HRS_INFOSUBMIT.submituseraccount  is '�������ʺ�';
comment on column HRS_INFOSUBMIT.submitusername  is '����������';
comment on column HRS_INFOSUBMIT.submituserid  is '������id';
comment on column HRS_INFOSUBMIT.submiteduserid  is '�ѱ�����id';
comment on column HRS_INFOSUBMIT.submitdepartid  is '���Ͳ���id';
comment on column HRS_INFOSUBMIT.submitdepartname  is '���Ͳ�������';
comment on column HRS_INFOSUBMIT.pct  is '�����������ٷֱ�';
comment on column HRS_INFOSUBMIT.remnum  is 'ʣ��δ��������';
comment on column HRS_INFOSUBMIT.remusername  is 'ʣ��δ����������';
comment on column HRS_INFOSUBMIT.remdepartname  is 'ʣ��δ����������';
comment on column HRS_INFOSUBMIT.issubmit  is '�Ƿ���';
comment on column HRS_INFOSUBMIT.infotype  is '���ͣ��±������������걨���걨��';
/
create or replace trigger trgHRS_INFOSUBMIT before insert on HRS_INFOSUBMIT for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--��Ϣ������ϸ��
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
comment on table HRS_INFOSUBMITDETAILS  is '��Ϣ������ϸ��';
-- Add comments to the columns 
comment on column HRS_INFOSUBMITDETAILS.createuserid  is '������id';
comment on column HRS_INFOSUBMITDETAILS.createusername  is '����������';
comment on column HRS_INFOSUBMITDETAILS.createuserdeptcode  is '���Ͳ���code';
comment on column HRS_INFOSUBMITDETAILS.departname  is '���Ͳ�������';
comment on column HRS_INFOSUBMITDETAILS.contents  is '��������';
comment on column HRS_INFOSUBMITDETAILS.submitdate  is '����ʱ��';
comment on column HRS_INFOSUBMITDETAILS.infoid  is '������Ϣid';
comment on column HRS_INFOSUBMITDETAILS.issubmit  is '�Ƿ��ύ';
/
create or replace trigger trgHRS_INFOSUBMITDETAILS before insert on HRS_INFOSUBMITDETAILS for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--EHS�ƻ������
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
comment on table HRS_PLANAPPLY  is 'EHS�ƻ������';
-- Add comments to the columns 
comment on column HRS_PLANAPPLY.userId  is '������id';
comment on column HRS_PLANAPPLY.userName  is '����������';
comment on column HRS_PLANAPPLY.departId  is '���벿��id';
comment on column HRS_PLANAPPLY.departName  is '���벿������';
comment on column HRS_PLANAPPLY.applyDate  is '��������';
comment on column HRS_PLANAPPLY.applyType  is '�������ͣ����żƻ������˼ƻ���';
comment on column HRS_PLANAPPLY.checkuseraccount  is '��ˣ��������ʺ�';
comment on column HRS_PLANAPPLY.flowState  is '����״̬';
comment on column HRS_PLANAPPLY.baseId  is '���ü�¼id';
/
create or replace trigger trgHRS_PLANAPPLY before insert on HRS_PLANAPPLY for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--EHS�ƻ�������ϸ��
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
comment on table HRS_PLANDETAILS  is 'EHS�ƻ�������ϸ��';
-- Add comments to the columns 
comment on column HRS_PLANDETAILS.contents  is '��������';
comment on column HRS_PLANDETAILS.dutyUserId  is '������id';
comment on column HRS_PLANDETAILS.dutyUserName  is '����������';
comment on column HRS_PLANDETAILS.dutyDepartId  is '���β���id';
comment on column HRS_PLANDETAILS.dutyDepartName  is '���β���';
comment on column HRS_PLANDETAILS.planFinDate  is '�ƻ��������';
comment on column HRS_PLANDETAILS.realFinDate  is 'ʵ���������';
comment on column HRS_PLANDETAILS.stdId  is '�����׼id';
comment on column HRS_PLANDETAILS.stdName  is '�����׼����';
comment on column HRS_PLANDETAILS.isCancel  is '�Ƿ�ȡ���ƻ�';
comment on column HRS_PLANDETAILS.changeReason  is '�䶯ԭ��';
comment on column HRS_PLANDETAILS.applyId  is '�ƻ�����id';
comment on column HRS_PLANDETAILS.baseId  is '���ü�¼id';
comment on column HRS_PLANDETAILS.ischanged  is '�Ƿ�仯';
comment on column HRS_PLANDETAILS.isMark  is '�Ƿ���';
/
create or replace trigger trgHRS_PLANDETAILS before insert on HRS_PLANDETAILS for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--EHS�ƻ������������ˣ���
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
comment on table HRS_PLANCHECK  is 'EHS�ƻ������������ˣ���';
-- Add comments to the columns 
comment on column HRS_PLANCHECK.applyid  is '�����¼id';
comment on column HRS_PLANCHECK.checkuserid  is '������id';
comment on column HRS_PLANCHECK.checkusername  is '����������';
comment on column HRS_PLANCHECK.checkdeptid  is '��������id';
comment on column HRS_PLANCHECK.checkdeptname  is '������������';
comment on column HRS_PLANCHECK.checkresult  is '���������1��ͨ����0��δͨ����';
comment on column HRS_PLANCHECK.checkreason  is '�������';
comment on column HRS_PLANCHECK.checkdate  is '��������';
comment on column HRS_PLANCHECK.checkbacktype  is '��������';
comment on column HRS_PLANCHECK.checktype  is '��������';
/
create or replace trigger trgHRS_PLANCHECK before insert on HRS_PLANCHECK for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/
