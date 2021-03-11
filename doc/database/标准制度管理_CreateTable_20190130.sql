--��׼�ƶȷ���
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_STDSYSTYPE';
   if tabCount>0 then
      execute immediate 'drop table HRS_STDSYSTYPE cascade constraints';
   end if;   
end;
/
create table HRS_STDSYSTYPE
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
  parentid           nvarchar2(36),
  code               nvarchar2(64),
  name               nvarchar2(200),
  scope              nvarchar2(64),
  remark             nvarchar2(500)
);
alter table HRS_STDSYSTYPE add primary key (id);
-- Add comments to the table 
comment on table HRS_STDSYSTYPE  is '��׼�ƶȷ����';
-- Add comments to the columns 
comment on column HRS_STDSYSTYPE.parentid    is '�ϼ�id';
comment on column HRS_STDSYSTYPE.code    is '����';
comment on column HRS_STDSYSTYPE.name  is '����';
comment on column HRS_STDSYSTYPE.scope  is '���ݲ㼶';
comment on column HRS_STDSYSTYPE.remark  is '��ע';
/
create or replace trigger trgHRS_STDSYSTYPE before insert on HRS_STDSYSTYPE for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--��׼�ƶ��ļ�
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_STDSYSFILES';
   if tabCount>0 then
      execute immediate 'drop table HRS_STDSYSFILES cascade constraints';
   end if;   
end;
/
create table HRS_STDSYSFILES
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
  filename           NVARCHAR2(200),
  fileno             nvarchar2(64),
  refid              nvarchar2(36),
  refname            nvarchar2(64),
  pubdate            timestamp(4),
  revisedate         timestamp(4),
  usedate            timestamp(4),
  pubdepartid        nvarchar2(36),
  pubdepartname      nvarchar2(64),
  pubuserid          nvarchar2(36),
  pubusername        nvarchar2(64),
  remark             nvarchar2(500)
);
alter table HRS_STDSYSFILES add primary key (id);
-- Add comments to the table 
comment on table HRS_STDSYSFILES  is '��׼�ƶ��ļ�';
-- Add comments to the columns 
comment on column HRS_STDSYSFILES.filename  is '�ļ�����';
comment on column HRS_STDSYSFILES.fileno  is '�ļ����';
comment on column HRS_STDSYSFILES.pubdate  is '��������';
comment on column HRS_STDSYSFILES.revisedate  is '�޶�����';
comment on column HRS_STDSYSFILES.usedate  is 'ʵʩ����';
comment on column HRS_STDSYSFILES.pubdepartname  is '������λ';
comment on column HRS_STDSYSFILES.pubusername  is '������';
comment on column HRS_STDSYSFILES.remark  is '��ע';
/
create or replace trigger trgHRS_STDSYSFILES before insert on HRS_STDSYSFILES for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/
--��׼�ƶ��ļ��ղ�
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_STDSYSSTOREFILES';
   if tabCount>0 then
      execute immediate 'drop table HRS_STDSYSSTOREFILES cascade constraints';
   end if;   
end;
/
create table HRS_STDSYSSTOREFILES
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
  userid             NVARCHAR2(36),
  stdsysid           nvarchar2(36)
);
alter table HRS_STDSYSSTOREFILES add primary key (id);
-- Add comments to the table 
comment on table HRS_STDSYSSTOREFILES  is '��׼�ƶ��ղ��ļ�';
-- Add comments to the columns 
comment on column HRS_STDSYSSTOREFILES.userid  is '�û����';
comment on column HRS_STDSYSSTOREFILES.stdsysid  is '��׼�ƶȱ��';
/
create or replace trigger trgHRS_STDSYSSTOREFILES before insert on HRS_STDSYSSTOREFILES for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/
--ɾ����׼�ƶȹ���ģ��
delete from base_modulebutton where moduleid in(select moduleid from base_module where fullname in('��׼�ƶȹ���'));
delete from base_module where fullname in('��׼�ƶȹ���');
--�����׼�ƶȹ���ģ��
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('1b7bf360-ece0-4f2b-b819-11c012ed7985', 'ed530449-0077-4520-be68-cfac0e461524', 'StdsysFiles', '��׼�ƶȹ���', 'fa fa-balance-scale', '/SafetyLawManage/StdsysFiles/Index', 'iframe', 1, 0, 0, null, null, 6, 0, 1, null, '2019-01-24 16:05:13', 'System', '��������Ա', null, null, null);
--��׼�ƶȹ���ģ���������
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('695655d1-6335-425b-a59b-0d1070da7bfa0', '1b7bf360-ece0-4f2b-b819-11c012ed7985', '0', null, 'search', '��ѯ', null, 0, 'search', 'fa fa-search', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('695655d1-6335-425b-a59b-0d1070da7bfa1', '1b7bf360-ece0-4f2b-b819-11c012ed7985', '0', null, 'add', '����', null, 1, 'add', 'fa fa-plus', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('695655d1-6335-425b-a59b-0d1070da7bfa2', '1b7bf360-ece0-4f2b-b819-11c012ed7985', '0', null, 'edit', '�޸�', null, 2, 'edit', 'fa fa-pencil-square-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('695655d1-6335-425b-a59b-0d1070da7bfa3', '1b7bf360-ece0-4f2b-b819-11c012ed7985', '0', null, 'delete', 'ɾ��', null, 3, 'del', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('695655d1-6335-425b-a59b-0d1070da7bfa4', '1b7bf360-ece0-4f2b-b819-11c012ed7985', '0', null, 'export', '����', null, 4, 'export', 'fa fa-download', 0);
--ɾ����׼�ƶȹ���ģ���Ȩ������
delete from base_authorize where itemid in(
'1b7bf360-ece0-4f2b-b819-11c012ed7985'--��׼�ƶȹ���
);
delete from base_authorizedata where resourceid in(
'1b7bf360-ece0-4f2b-b819-11c012ed7985'--��׼�ƶȹ���
);
--����NOSA����ģ��Ĳ�ѯȨ�����ã�����Ȩ�޴��ڴ�����ʵ�֡�
--1.��ͨ�û�����׼�ƶ�
--ģ��˵�
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('bb90058f-6660-45fc-82da-ca8b620af290', 2, '2a878044-06e9-4fe4-89f0-ba7bd5a1bde6', 1, '1b7bf360-ece0-4f2b-b819-11c012ed7985', 19, '2019-01-25 11:09:34', 'System', '��������Ա');
--ģ�����ݷ�Χ
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('a0059f0c-577f-44ee-872e-f341d20b1720', 4, 2, '2a878044-06e9-4fe4-89f0-ba7bd5a1bde6', '695655d1-6335-425b-a59b-0d1070da7bfa0', '��ѯ', '1b7bf360-ece0-4f2b-b819-11c012ed7985', 0, null, 1, '2019-01-24 16:05:51', 'System', '��������Ա', 'search');
/
--��׼�ƶ�����
--ע�⣺���糧һ�����ͣ���Ҫ�޸���Ӧ�Ĳ���ֵ���ظ�ִ�����½ű���
declare 
  orgCode nvarchar2(36);
  userId nvarchar2(36);
  userName nvarchar2(64);
  baseId nvarchar2(36);
begin
orgCode := '001';
userId :='38fab486-9b35-4d1a-bda0-f6a0e93e0ee4';
userName :='F��˾����Ա';
baseId :='13d1234d-73fc-8beb-7f58-8047aee279';
--��ɾ��
delete from HRS_STDSYSTYPE where CREATEUSERORGCODE=orgCode;
--�󴴽�
insert into HRS_STDSYSTYPE (ID, AUTOID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, PARENTID, CODE, NAME, SCOPE, REMARK)
values (baseId||'-1', 127156, userId, orgCode, orgCode, '2019-01-24 16:07:35', userName, null, null, null, '-1', '000001', '������׼', orgCode, null);
insert into HRS_STDSYSTYPE (ID, AUTOID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, PARENTID, CODE, NAME, SCOPE, REMARK)
values (baseId||'-2', 127157, userId, orgCode, orgCode, '2019-01-24 16:07:57', userName, null, null, null, '-1', '000002', '�����׼', orgCode, null);
insert into HRS_STDSYSTYPE (ID, AUTOID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, PARENTID, CODE, NAME, SCOPE, REMARK)
values (baseId||'-3', 127158, userId, orgCode, orgCode, '2019-01-24 16:08:51', userName, null, null, null, '-1', '000003', '������׼', orgCode, null);
insert into HRS_STDSYSTYPE (ID, AUTOID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, PARENTID, CODE, NAME, SCOPE, REMARK)
values (baseId||'-4', 127159, userId, orgCode, orgCode, '2019-01-24 16:09:08', userName, null, null, null, '-1', '000004', '���ñ�׼', orgCode, null);
insert into HRS_STDSYSTYPE (ID, AUTOID, CREATEUSERID, CREATEUSERDEPTCODE, CREATEUSERORGCODE, CREATEDATE, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME, PARENTID, CODE, NAME, SCOPE, REMARK)
values (baseId||'-5', 127160, userId, orgCode, orgCode, '2019-01-24 16:09:24', userName, null, null, null, '-1', '000005', '�����׼��', '05', null);
end;
/
commit;
