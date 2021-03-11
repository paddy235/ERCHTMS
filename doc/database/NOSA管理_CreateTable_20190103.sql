--NOSAԪ�ر�
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_NOSAELE';
   if tabCount>0 then
      execute immediate 'drop table HRS_NOSAELE cascade constraints';
   end if;   
end;
/
create table HRS_NOSAELE
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
  parentname         nvarchar2(200),
  no                 NVARCHAR2(50),
  name               NVARCHAR2(200),
  dutyuserid         nvarchar2(36),
  dutyusername       nvarchar2(64),
  dutydepartid       nvarchar2(36),
  dutydepartname     nvarchar2(64)
);
alter table HRS_NOSAELE add primary key (id);
-- Add comments to the table 
comment on table HRS_NOSAELE  is 'Ԫ�ر�';
-- Add comments to the columns 
comment on column HRS_NOSAELE.parentid    is '�ϼ�id';
comment on column HRS_NOSAELE.parentname    is '�ϼ�Ԫ��';
comment on column HRS_NOSAELE.no    is '���';
comment on column HRS_NOSAELE.name  is '����';
comment on column HRS_NOSAELE.dutyuserid  is '������id';
comment on column HRS_NOSAELE.dutyusername  is '����������';
comment on column HRS_NOSAELE.dutydepartid  is '���β���id';
comment on column HRS_NOSAELE.dutydepartname  is '���β�������';
/
create or replace trigger trgHRS_NOSAELE before insert on HRS_NOSAELE for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--NOSA���������
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_NOSAWORKS';
   if tabCount>0 then
      execute immediate 'drop table HRS_NOSAWORKS cascade constraints';
   end if;   
end;
/
create table HRS_NOSAWORKS
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
  name               NVARCHAR2(200),
  according          nvarchar2(200),
  ratenum            nvarchar2(64),
  enddate            TIMESTAMP(4),
  dutyuserid         nvarchar2(2000),
  dutyusername       nvarchar2(2000),
  dutydepartid       nvarchar2(2000),
  dutydepartname     nvarchar2(2000),
  submituserid       nvarchar2(2000),
  submitusername     nvarchar2(2000),
  eleid              nvarchar2(36),
  eleno              nvarchar2(50),
  elename            nvarchar2(200),
  eledutyuserid         nvarchar2(36),
  eledutyusername       nvarchar2(64),
  eledutydepartid       nvarchar2(36),
  eledutydepartname     nvarchar2(64),
  advise                nvarchar2(500),
  remark                nvarchar2(500),
  issubmited            nvarchar2(2),
  pct                   number(8,2),
  dutyuserhtml       nvarchar2(2000),
  dutydeparthtml     nvarchar2(2000)
);
alter table HRS_NOSAWORKS add primary key (id);
-- Add comments to the table 
comment on table HRS_NOSAWORKS  is '��������';
-- Add comments to the columns 
comment on column HRS_NOSAWORKS.name  is '��Ŀ����';
comment on column HRS_NOSAWORKS.according  is '����';
comment on column HRS_NOSAWORKS.ratenum  is 'Ƶ��';
comment on column HRS_NOSAWORKS.enddate  is '�������ʱ��';
comment on column HRS_NOSAWORKS.dutyuserid  is '������id';
comment on column HRS_NOSAWORKS.dutyusername  is '����������';
comment on column HRS_NOSAWORKS.dutydepartid  is '���β���id';
comment on column HRS_NOSAWORKS.dutydepartname  is '���β�������';
comment on column HRS_NOSAWORKS.submituserid  is '���ύ������id';
comment on column HRS_NOSAWORKS.submitusername  is '���ύ����������';
comment on column HRS_NOSAWORKS.eleid  is 'Ԫ��id';
comment on column HRS_NOSAWORKS.eleno  is 'Ԫ��no';
comment on column HRS_NOSAWORKS.elename  is 'Ԫ������';
comment on column HRS_NOSAWORKS.eledutyuserid  is 'Ԫ�ظ�����id';
comment on column HRS_NOSAWORKS.eledutyusername  is 'Ԫ�ظ���������';
comment on column HRS_NOSAWORKS.eledutydepartid  is 'Ԫ�����β���id';
comment on column HRS_NOSAWORKS.eledutydepartname  is 'Ԫ����������';
comment on column HRS_NOSAWORKS.advise  is '����';
comment on column HRS_NOSAWORKS.remark  is '��ע';
comment on column HRS_NOSAWORKS.issubmited  is '�Ƿ��ύ��ֵ���ǣ���';
comment on column HRS_NOSAWORKS.pct  is '��ɽ���';
comment on column HRS_NOSAWORKS.dutyuserhtml  is '����������html';
comment on column HRS_NOSAWORKS.dutydeparthtml  is '���β�����html';
/
create or replace trigger trgHRS_NOSAWORKS before insert on HRS_NOSAWORKS for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--NOSA����������ϸ
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_NOSAWORKITEM';
   if tabCount>0 then
      execute immediate 'drop table HRS_NOSAWORKITEM cascade constraints';
   end if;   
end;
/
create table HRS_NOSAWORKITEM
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
  workid             nvarchar2(36),
  dutyuserid         nvarchar2(36),
  dutyusername       nvarchar2(64),
  dutydepartid       nvarchar2(36),
  dutydepartname     nvarchar2(64),
  uploaddate         TIMESTAMP(4),
  issubmitted        nvarchar2(2),  
  state              nvarchar2(64),
  checkuserid        nvarchar2(36),
  checkusername      nvarchar2(64),  
  checkidea          nvarchar2(200),
  checkdate          timestamp(4)
);
alter table HRS_NOSAWORKITEM add primary key (id);
-- Add comments to the table 
comment on table HRS_NOSAWORKITEM  is '����������ϸ';
-- Add comments to the columns 
comment on column HRS_NOSAWORKITEM.workid  is '��������id';
comment on column HRS_NOSAWORKITEM.dutyuserid  is '������id';
comment on column HRS_NOSAWORKITEM.dutyusername  is '����������';
comment on column HRS_NOSAWORKITEM.dutydepartid  is '���β���id';
comment on column HRS_NOSAWORKITEM.dutydepartname  is '���β�������';
comment on column HRS_NOSAWORKITEM.uploaddate  is '�ϴ�����';
comment on column HRS_NOSAWORKITEM.issubmitted  is '�Ƿ��ύ��ֵ���ǣ���';
comment on column HRS_NOSAWORKITEM.checkuserid  is '�����id';
comment on column HRS_NOSAWORKITEM.checkusername  is '���������';
comment on column HRS_NOSAWORKITEM.state  is '״̬�����ϴ�������С����ͨ�������δͨ����';
comment on column HRS_NOSAWORKITEM.checkidea  is '������';
comment on column HRS_NOSAWORKITEM.checkdate  is '�������';
/
create or replace trigger trgHRS_NOSAWORKITEM before insert on HRS_NOSAWORKITEM for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--NOSA�����ɹ�
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_NOSAWORKRESULT';
   if tabCount>0 then
      execute immediate 'drop table HRS_NOSAWORKRESULT cascade constraints';
   end if;   
end;
/
create table HRS_NOSAWORKRESULT
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
  workid             nvarchar2(36),
  name               NVARCHAR2(200),
  templatename       nvarchar2(200),
  templatepath       nvarchar2(500)
);
alter table HRS_NOSAWORKRESULT add primary key (id);
-- Add comments to the table 
comment on table HRS_NOSAWORKRESULT  is '�����ɹ�';
-- Add comments to the columns 
comment on column HRS_NOSAWORKRESULT.workid  is '��������id';
comment on column HRS_NOSAWORKRESULT.name  is '�ɹ�����';
comment on column HRS_NOSAWORKRESULT.templatename  is 'ģ���ļ�����';
comment on column HRS_NOSAWORKRESULT.templatepath  is 'ģ���ļ�·��';
/
create or replace trigger trgHRS_NOSAWORKRESULT before insert on HRS_NOSAWORKRESULT for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--NOSA��ѵ�ļ�����
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_NOSATRATYPE';
   if tabCount>0 then
      execute immediate 'drop table HRS_NOSATRATYPE cascade constraints';
   end if;   
end;
/
create table HRS_NOSATRATYPE
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
  name               NVARCHAR2(200),
  remark             nvarchar2(500)
);
alter table HRS_NOSATRATYPE add primary key (id);
-- Add comments to the table 
comment on table HRS_NOSATRATYPE  is '��ѵ�ļ������';
-- Add comments to the columns 
comment on column HRS_NOSATRATYPE.parentid    is '�ϼ�id';
comment on column HRS_NOSATRATYPE.code    is '����';
comment on column HRS_NOSATRATYPE.name  is '����';
comment on column HRS_NOSATRATYPE.remark  is '��ע';
/
create or replace trigger trgHRS_NOSATRATYPE before insert on HRS_NOSATRATYPE for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--NOSA��ѵ�ļ�
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_NOSATRAFILES';
   if tabCount>0 then
      execute immediate 'drop table HRS_NOSATRAFILES cascade constraints';
   end if;   
end;
/
create table HRS_NOSATRAFILES
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
  refid              nvarchar2(36),
  refname            nvarchar2(64),
  pubdate            timestamp(4),
  pubdepartid        nvarchar2(36),
  pubdepartname      nvarchar2(64),
  pubuserid          nvarchar2(36),
  pubusername        nvarchar2(64),
  msguserid         nvarchar2(2000),
  msgusername       nvarchar2(2000),
  viewtimes         number(10)
);
alter table HRS_NOSATRAFILES add primary key (id);
-- Add comments to the table 
comment on table HRS_NOSATRAFILES  is '��ѵ�ļ�';
-- Add comments to the columns 
comment on column HRS_NOSATRAFILES.filename  is '�ļ�����';
comment on column HRS_NOSATRAFILES.refid  is '����id';
comment on column HRS_NOSATRAFILES.refname  is '��������';
comment on column HRS_NOSATRAFILES.pubdate  is '��������';
comment on column HRS_NOSATRAFILES.pubdepartid  is '��������id';
comment on column HRS_NOSATRAFILES.pubdepartname  is '������������';
comment on column HRS_NOSATRAFILES.pubuserid  is '������id';
comment on column HRS_NOSATRAFILES.pubusername  is '����������';
comment on column HRS_NOSATRAFILES.msguserid  is '��Ϣ������id';
comment on column HRS_NOSATRAFILES.msgusername  is '��Ϣ����������';
comment on column HRS_NOSATRAFILES.viewtimes  is '���Ĵ���';
/
create or replace trigger trgHRS_NOSATRAFILES before insert on HRS_NOSATRAFILES for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/
--ɾ��NOSA����ģ��
delete from base_modulebutton where moduleid in(select moduleid from base_module where fullname in('NOSA����','NOSAԪ��','����Ԫ�ظ�����','NOSAԪ�ع����嵥','�����ɹ��ϴ�','NOSA��ѵ'));
delete from base_module where fullname in('NOSA����','NOSAԪ��','����Ԫ�ظ�����','NOSAԪ�ع����嵥','�����ɹ��ϴ�','NOSA��ѵ');
--����NOSA����ģ��
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('33aef115-e4ac-4a51-9f5b-3172746f7ef5', '0', 'NOSAManagement', 'NOSA����', 'fa fa-inbox', null, 'expand', 1, 0, 0, null, null, 55, 0, 1, null, '2018-12-11 09:30:59', 'System', '��������Ա', null, null, null);
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('3e31b357-8d35-4156-815d-ae6f39abdf34', '33aef115-e4ac-4a51-9f5b-3172746f7ef5', 'NOSAEle', '����Ԫ�ظ�����', 'fa fa-circle', '/NosaManage/Nosaele/Index', 'iframe', 1, 0, 0, null, null, 0, 0, 1, null, '2019-01-03 13:57:34', 'System', '��������Ա', '2019-01-10 13:28:17', 'System', '��������Ա');
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('cf340886-d998-4f41-974b-0b08e8459032', '33aef115-e4ac-4a51-9f5b-3172746f7ef5', 'WorkList', 'NOSAԪ�ع����嵥', 'fa fa-list-ul', '/NosaManage/Nosaworks/Index', 'iframe', 1, 0, 0, null, null, 1, 0, 1, null, '2018-12-11 09:31:51', 'System', '��������Ա', '2019-01-10 13:28:34', 'System', '��������Ա');
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('b8ae20f7-970f-424b-8e9b-3d8ca826dc6b', '33aef115-e4ac-4a51-9f5b-3172746f7ef5', 'UploadIndex', '�����ɹ��ϴ�', 'fa fa-cloud-upload', '/NosaManage/Nosaworks/UploadIndex', 'iframe', 1, 0, 0, null, null, 2, 0, 1, null, '2019-01-07 14:06:49', 'System', '��������Ա', null, null, null);
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('6471b5fe-fdb4-4004-9ad0-4050f5b89b9a', '33aef115-e4ac-4a51-9f5b-3172746f7ef5', 'NOSATraining', 'NOSA��ѵ', 'fa fa-users', '/NosaManage/Nosatrafiles/Index', 'iframe', 1, 0, 0, null, null, 3, 0, 1, null, '2018-12-11 09:32:48', 'System', '��������Ա', '2019-01-07 14:05:58', 'System', '��������Ա');
--NOSA����ģ���������
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e01', 'b8ae20f7-970f-424b-8e9b-3d8ca826dc6b', '0', null, 'add', '����', null, 1, 'add', 'fa fa-plus', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e02', 'b8ae20f7-970f-424b-8e9b-3d8ca826dc6b', '0', null, 'edit', '�޸�', null, 2, 'edit', 'fa fa-pencil-square-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e03', 'b8ae20f7-970f-424b-8e9b-3d8ca826dc6b', '0', null, 'delete', 'ɾ��', null, 3, 'del', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e04', 'b8ae20f7-970f-424b-8e9b-3d8ca826dc6b', '0', null, 'export', '����', null, 4, 'export', 'fa fa-download', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('310dd06c-6a9d-4594-92f3-a8d37771bad50', '6471b5fe-fdb4-4004-9ad0-4050f5b89b9a', '0', null, 'search', '��ѯ', null, 0, 'search', 'fa fa-search', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('310dd06c-6a9d-4594-92f3-a8d37771bad51', '6471b5fe-fdb4-4004-9ad0-4050f5b89b9a', '0', null, 'add', '����', null, 1, 'add', 'fa fa-plus', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('310dd06c-6a9d-4594-92f3-a8d37771bad52', '6471b5fe-fdb4-4004-9ad0-4050f5b89b9a', '0', null, 'edit', '�޸�', null, 2, 'edit', 'fa fa-pencil-square-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('310dd06c-6a9d-4594-92f3-a8d37771bad53', '6471b5fe-fdb4-4004-9ad0-4050f5b89b9a', '0', null, 'delete', 'ɾ��', null, 3, 'del', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('310dd06c-6a9d-4594-92f3-a8d37771bad54', '6471b5fe-fdb4-4004-9ad0-4050f5b89b9a', '0', null, 'export', '����', null, 4, 'export', 'fa fa-download', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e00', 'b8ae20f7-970f-424b-8e9b-3d8ca826dc6b', '0', null, 'search', '��ѯ', null, 0, 'search', 'fa fa-search', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('9551e3d5-d9ba-4d0b-b1f2-eb8d5d1388f60', 'cf340886-d998-4f41-974b-0b08e8459032', '0', null, 'search', '��ѯ', null, 0, 'search', 'fa fa-search', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('9551e3d5-d9ba-4d0b-b1f2-eb8d5d1388f61', 'cf340886-d998-4f41-974b-0b08e8459032', '0', null, 'add', '����', null, 1, 'add', 'fa fa-plus', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('9551e3d5-d9ba-4d0b-b1f2-eb8d5d1388f62', 'cf340886-d998-4f41-974b-0b08e8459032', '0', null, 'edit', '�޸�', null, 2, 'edit', 'fa fa-pencil-square-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('9551e3d5-d9ba-4d0b-b1f2-eb8d5d1388f63', 'cf340886-d998-4f41-974b-0b08e8459032', '0', null, 'delete', 'ɾ��', null, 3, 'del', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('9551e3d5-d9ba-4d0b-b1f2-eb8d5d1388f64', 'cf340886-d998-4f41-974b-0b08e8459032', '0', null, 'export', '����', null, 4, 'export', 'fa fa-download', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('b8f01f8b-4f7b-49dd-be9f-9479e22d86060', '3e31b357-8d35-4156-815d-ae6f39abdf34', '0', null, 'search', '��ѯ', null, 0, 'search', 'fa fa-search', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('b8f01f8b-4f7b-49dd-be9f-9479e22d86061', '3e31b357-8d35-4156-815d-ae6f39abdf34', '0', null, 'add', '����', null, 1, 'add', 'fa fa-plus', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('b8f01f8b-4f7b-49dd-be9f-9479e22d86062', '3e31b357-8d35-4156-815d-ae6f39abdf34', '0', null, 'edit', '�޸�', null, 2, 'edit', 'fa fa-pencil-square-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('b8f01f8b-4f7b-49dd-be9f-9479e22d86063', '3e31b357-8d35-4156-815d-ae6f39abdf34', '0', null, 'delete', 'ɾ��', null, 3, 'del', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('b8f01f8b-4f7b-49dd-be9f-9479e22d86064', '3e31b357-8d35-4156-815d-ae6f39abdf34', '0', null, 'export', '����', null, 4, 'export', 'fa fa-download', 0);
--ɾ��NOSA����ģ���Ȩ������
delete from base_authorize where itemid in(
'33aef115-e4ac-4a51-9f5b-3172746f7ef5',--NOSA����
'3e31b357-8d35-4156-815d-ae6f39abdf34',--NOSAԪ��
'cf340886-d998-4f41-974b-0b08e8459032',--NOSAԪ�ع����嵥
'b8ae20f7-970f-424b-8e9b-3d8ca826dc6b',--�����ɹ��ϴ�
'6471b5fe-fdb4-4004-9ad0-4050f5b89b9a'--NOSA��ѵ
);
delete from base_authorizedata where resourceid in(
'33aef115-e4ac-4a51-9f5b-3172746f7ef5',--NOSA����
'3e31b357-8d35-4156-815d-ae6f39abdf34',--NOSAԪ��
'cf340886-d998-4f41-974b-0b08e8459032',--NOSAԪ�ع����嵥
'b8ae20f7-970f-424b-8e9b-3d8ca826dc6b',--�����ɹ��ϴ�
'6471b5fe-fdb4-4004-9ad0-4050f5b89b9a'--NOSA��ѵ
);
--����NOSA����ģ��Ĳ�ѯȨ�����ã�����Ȩ�޴��ڴ�����ʵ�֡�
--1.��˾���û�����˾����Ա����ȫ����Ա��NOSAԪ�ء�NOSAԪ�ع����嵥�������ɹ��ϴ���NOSA��ѵ��
--2.��ͨ�û���NOSAԪ�ع����嵥�������ɹ��ϴ���NOSA��ѵ
--ģ��˵�
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('1eb463f7-b547-4f5b-af8e-71273e213bb6', 2, '2a878044-06e9-4fe4-89f0-ba7bd5a1bde6', 1, 'b8ae20f7-970f-424b-8e9b-3d8ca826dc6b', 130, '2019-01-10 15:20:23', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('5e9ea495-085d-46aa-b207-bc4777b5c32a', 2, '2a878044-06e9-4fe4-89f0-ba7bd5a1bde6', 1, '33aef115-e4ac-4a51-9f5b-3172746f7ef5', 128, '2019-01-10 15:20:23', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('75fdc12c-28da-4f7f-a367-471b0691b6fa', 2, '2a878044-06e9-4fe4-89f0-ba7bd5a1bde6', 1, 'cf340886-d998-4f41-974b-0b08e8459032', 129, '2019-01-10 15:20:23', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('efea63d2-9199-4361-87b1-59094c803986', 2, '2a878044-06e9-4fe4-89f0-ba7bd5a1bde6', 1, '6471b5fe-fdb4-4004-9ad0-4050f5b89b9a', 131, '2019-01-10 15:20:23', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('cf76f272-c981-437b-9d93-e9be5e4043c4', 2, '5af22786-e2f2-4a3d-8da3-ecfb16b96f36', 1, '33aef115-e4ac-4a51-9f5b-3172746f7ef5', 159, '2019-01-10 15:13:46', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('e28d72e3-0fab-44a5-968e-63d22b5b96a1', 2, '5af22786-e2f2-4a3d-8da3-ecfb16b96f36', 1, 'cf340886-d998-4f41-974b-0b08e8459032', 161, '2019-01-10 15:13:46', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('58864a4f-e2c6-4089-a850-578318540647', 2, '62787f62-1c21-42c4-b411-906fa03bc2de', 1, '3e31b357-8d35-4156-815d-ae6f39abdf34', 150, '2019-01-10 15:13:22', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('cf6a7808-6324-4ef3-bfd7-b982f3c455eb', 2, '62787f62-1c21-42c4-b411-906fa03bc2de', 1, 'cf340886-d998-4f41-974b-0b08e8459032', 151, '2019-01-10 15:13:22', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('d5603d0b-8cc7-4136-9b09-8092f5b4b033', 2, '62787f62-1c21-42c4-b411-906fa03bc2de', 1, '33aef115-e4ac-4a51-9f5b-3172746f7ef5', 149, '2019-01-10 15:13:22', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('46bc6f6c-6c1a-4847-a7bf-1aefff02e3a6', 2, '5af22786-e2f2-4a3d-8da3-ecfb16b96f36', 1, '3e31b357-8d35-4156-815d-ae6f39abdf34', 160, '2019-01-10 15:13:46', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('40b71ac8-31c9-42f1-9ccf-d7cbf509a64d', 2, 'aece6d68-ef8a-4eac-a746-e97f0067fab5', 1, '33aef115-e4ac-4a51-9f5b-3172746f7ef5', 159, '2019-01-10 15:14:10', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('7f6a0f61-dc2b-4263-8e10-d81ba8b08381', 2, 'aece6d68-ef8a-4eac-a746-e97f0067fab5', 1, 'cf340886-d998-4f41-974b-0b08e8459032', 161, '2019-01-10 15:14:10', 'System', '��������Ա');
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('8efb9e64-b976-4559-a2bd-c65fd51ec65d', 2, 'aece6d68-ef8a-4eac-a746-e97f0067fab5', 1, '3e31b357-8d35-4156-815d-ae6f39abdf34', 160, '2019-01-10 15:14:10', 'System', '��������Ա');
--ģ�����ݷ�Χ
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('a4c4c3d1-9e92-4e42-b494-b3ea3a73d8ce', 4, 2, '2a878044-06e9-4fe4-89f0-ba7bd5a1bde6', '310dd06c-6a9d-4594-92f3-a8d37771bad50', '��ѯ', '6471b5fe-fdb4-4004-9ad0-4050f5b89b9a', 0, null, 1, '2019-01-10 14:59:25', 'System', '��������Ա', 'search');
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('ec15433a-1a6e-4b21-89b2-2a4706f8477e', 4, 2, 'aece6d68-ef8a-4eac-a746-e97f0067fab5', '9551e3d5-d9ba-4d0b-b1f2-eb8d5d1388f60', '��ѯ', 'cf340886-d998-4f41-974b-0b08e8459032', 0, null, 1, '2019-01-10 15:03:44', 'System', '��������Ա', 'search');
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('a2b64afa-a3f6-4291-90d0-b8ef117dfe69', 4, 2, '62787f62-1c21-42c4-b411-906fa03bc2de', 'b8f01f8b-4f7b-49dd-be9f-9479e22d86060', '��ѯ', '3e31b357-8d35-4156-815d-ae6f39abdf34', 0, null, 1, '2019-01-10 15:04:07', 'System', '��������Ա', 'search');
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('e71285dc-b45d-4a30-8138-2ae4600411e3', 4, 2, '5af22786-e2f2-4a3d-8da3-ecfb16b96f36', 'b8f01f8b-4f7b-49dd-be9f-9479e22d86060', '��ѯ', '3e31b357-8d35-4156-815d-ae6f39abdf34', 0, null, 1, '2019-01-10 15:04:34', 'System', '��������Ա', 'search');
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('05932a52-eb44-4ea9-b620-0e56abc46f0e', 4, 2, 'aece6d68-ef8a-4eac-a746-e97f0067fab5', 'b8f01f8b-4f7b-49dd-be9f-9479e22d86060', '��ѯ', '3e31b357-8d35-4156-815d-ae6f39abdf34', 0, null, 1, '2019-01-10 15:09:21', 'System', '��������Ա', 'search');
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('2e3e36e3-ff12-4f6a-b06e-da26c9b2d41a', 4, 2, '5af22786-e2f2-4a3d-8da3-ecfb16b96f36', '9551e3d5-d9ba-4d0b-b1f2-eb8d5d1388f60', '��ѯ', 'cf340886-d998-4f41-974b-0b08e8459032', 0, null, 1, '2019-01-10 13:44:29', 'System', '��������Ա', 'search');
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('4d2e4187-535a-482d-9be5-2b5d2d67c702', 4, 2, '62787f62-1c21-42c4-b411-906fa03bc2de', '9551e3d5-d9ba-4d0b-b1f2-eb8d5d1388f60', '��ѯ', 'cf340886-d998-4f41-974b-0b08e8459032', 0, null, 1, '2019-01-10 13:42:57', 'System', '��������Ա', 'search');
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('7b960d64-b1a0-4699-8a6c-07b5a46aabb3', 4, 2, '2a878044-06e9-4fe4-89f0-ba7bd5a1bde6', '2f003ad8-a727-45d8-893c-77ee0059a4e00', '��ѯ', 'b8ae20f7-970f-424b-8e9b-3d8ca826dc6b', 0, null, 1, '2019-01-07 14:07:18', 'System', '��������Ա', 'search');
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('ad16502e-7225-4bb7-8910-1dcc01486a0e', 4, 2, '2a878044-06e9-4fe4-89f0-ba7bd5a1bde6', '9551e3d5-d9ba-4d0b-b1f2-eb8d5d1388f60', '��ѯ', 'cf340886-d998-4f41-974b-0b08e8459032', 0, null, 1, '2019-01-10 15:20:23', 'System', '��������Ա', 'search');
--NOSA����ģ���������
delete from base_dataitemdetail where itemvalue in('NOSACHECK','NOSAUPLOAD');
delete from base_dataset where itemcode in('NOSACHECK','NOSAUPLOAD');
--1.��������
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('bfa79b04-0e0c-4254-a30c-7a6ac5c7fbf4', '50c51435-bd1e-4560-9079-5aeae8de2634', '0', 'DBSX', '���ϴ���NOSA�����嵥', 'NOSAUPLOAD', null, 'dscdnosagzqd', null, 6, 0, 1, '��������', '2019-01-14 09:15:44', 'System', '��������Ա', '2019-01-14 09:19:31', 'System', '��������Ա');
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('e72ddd64-4455-4ce6-a536-c68e56786cb3', '50c51435-bd1e-4560-9079-5aeae8de2634', '0', 'DBSX', '����˵�NOSA�����嵥', 'NOSACHECK', null, 'dshdnosagzqd', null, 7, 0, 1, '��������', '2019-01-14 09:20:27', 'System', '��������Ա', null, null, null);
--2.���⡢��ַ����
insert into base_dataset (ID, CREATEDATE, MODIFYDATE, ITEMCODE, ITEMNAME, ITEMKIND, ITEMTYPE, DEPTCODE, DEPTNAME, ITEMROLE, ICON, ADDRESS, ISDEFAULT, ISOPEN, CALLBACK, REMARK, ITEMSTYLE, SORTCODE, DEPTID)
values ('dbfae9af-1019-49cd-9a0a-0d14e61df63a', '2019-01-14 09:21:40', null, 'NOSAUPLOAD', '���ϴ���NOSA�����嵥', 'DBSX', '�����Ų�', ',013001004001001,', '��������������޹�˾', '��˾�쵼,һ���û�', 'fa fa-cloud-upload', '../NosaManage/Nosaworks/UploadIndex?myupload=1', '��', '��', 'top.openTab(''b30dc575-14a9-4249-a2de-10ec214a21a7'', ''{Url}'',''{ItemName}'')', null, '<li><a onclick="{Callback}"><i class="{Icon}"></i>{ItemName}<span>{Num}</span></a></li>', 17, '668fa31b-7caf-472b-a481-14df870e183e');
insert into base_dataset (ID, CREATEDATE, MODIFYDATE, ITEMCODE, ITEMNAME, ITEMKIND, ITEMTYPE, DEPTCODE, DEPTNAME, ITEMROLE, ICON, ADDRESS, ISDEFAULT, ISOPEN, CALLBACK, REMARK, ITEMSTYLE, SORTCODE, DEPTID)
values ('e7b01c8a-f963-455c-92b2-975ed8f3c58e', '2019-01-14 09:22:17', null, 'NOSACHECK', '����˵�NOSA�����嵥', 'DBSX', '�����Ų�', ',013001004001001,', '��������������޹�˾', '��˾�쵼,һ���û�', 'fa fa-check', '../NOSAManage/Nosaworks/Index?mycheck=3', '��', '��', 'top.openTab(''b30dc575-14a9-4249-a2de-10ec214a21a8'', ''{Url}'',''{ItemName}'')', null, '<li><a onclick="{Callback}"><i class="{Icon}"></i>{ItemName}<span>{Num}</span></a></li>', 17, '668fa31b-7caf-472b-a481-14df870e183e');
commit;
