--ɾ��EHS�����ƻ�����ģ��
delete from base_modulebutton where moduleid in(select moduleid from base_module where parentid='a72d424a-a44c-4306-ba8f-287bf55cba45');
delete from base_module where moduleid in(
'f067bc5a-ab7a-4050-9953-591f8463eec8',--�ۺ���Ϣ����
'a72d424a-a44c-4306-ba8f-287bf55cba45',--EHS�ƻ�����
'65afd84f-3de7-431f-abeb-9987f2a48cd0',--�����ƻ�
'904a8952-aeb3-4a8c-8e14-289aed0d48c8',--������¼̨��
'29191c49-add8-4167-b512-3f30f792158c'--ͳ�Ʒ���
);
--����EHS�����ƻ�����ģ��
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('f067bc5a-ab7a-4050-9953-591f8463eec8', '0', 'ComprehensiveInformationManagement', '�ۺ���Ϣ����', 'fa fa-newspaper-o', null, 'expand', 1, 0, 0, null, null, 50, 0, 1, null, '2018-12-11 09:13:47', 'System', '��������Ա', '2018-12-11 09:30:34', 'System', '��������Ա');
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('a72d424a-a44c-4306-ba8f-287bf55cba45', 'f067bc5a-ab7a-4050-9953-591f8463eec8', 'EHSPlanManagement', 'EHS�ƻ�����', 'fa fa-tags', null, 'expand', 1, 0, 0, null, null, 1, 0, 1, null, '2018-12-11 09:19:33', 'System', '��������Ա', '2018-12-11 09:19:51', 'System', '��������Ա');
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('65afd84f-3de7-431f-abeb-9987f2a48cd0', 'a72d424a-a44c-4306-ba8f-287bf55cba45', 'WorkPlan', '�����ƻ�', 'fa fa-television', '/WorkPlan/PlanApply/Index', 'iframe', 1, 0, 0, null, null, 1, 0, 1, null, '2018-12-11 09:21:42', 'System', '��������Ա', '2018-12-20 15:32:40', 'System', '��������Ա');
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('904a8952-aeb3-4a8c-8e14-289aed0d48c8', 'a72d424a-a44c-4306-ba8f-287bf55cba45', 'PlanDetails', '������¼̨��', 'fa fa-archive', '/WorkPlan/PlanDetails/Index', 'iframe', 1, 0, 0, null, null, 1, 0, 1, null, '2018-12-22 14:54:45', 'System', '��������Ա', null, null, null);
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('29191c49-add8-4167-b512-3f30f792158c', 'a72d424a-a44c-4306-ba8f-287bf55cba45', 'WorkPlanSta', 'ͳ�Ʒ���', 'fa fa-bar-chart-o', '/WorkPlan/PlanDetails/Statistics', 'iframe', 1, 0, 0, null, null, 2, 0, 1, null, '2018-12-11 09:22:09', 'System', '��������Ա', '2018-12-24 10:58:36', 'System', '��������Ա');
--EHS�����ƻ�����ģ���������
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('7c522274-2502-4720-914e-7bd81421cdd80', '65afd84f-3de7-431f-abeb-9987f2a48cd0', '0', null, 'search', '��ѯ', null, 0, 'search', 'fa fa-search', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('7c522274-2502-4720-914e-7bd81421cdd81', '65afd84f-3de7-431f-abeb-9987f2a48cd0', '0', null, 'add', '����', null, 1, 'add', 'fa fa-plus', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('7c522274-2502-4720-914e-7bd81421cdd82', '65afd84f-3de7-431f-abeb-9987f2a48cd0', '0', null, 'edit', '�޸�', null, 2, 'edit', 'fa fa-pencil-square-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('7c522274-2502-4720-914e-7bd81421cdd83', '65afd84f-3de7-431f-abeb-9987f2a48cd0', '0', null, 'delete', 'ɾ��', null, 3, 'del', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('7c522274-2502-4720-914e-7bd81421cdd84', '65afd84f-3de7-431f-abeb-9987f2a48cd0', '0', null, 'export', '����', null, 4, 'export', 'fa fa-download', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('1f913593-6953-422f-b758-4fc03aca67771', '29191c49-add8-4167-b512-3f30f792158c', '0', null, 'add', '����', null, 1, 'add', 'fa fa-plus', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('1f913593-6953-422f-b758-4fc03aca67772', '29191c49-add8-4167-b512-3f30f792158c', '0', null, 'edit', '�޸�', null, 2, 'edit', 'fa fa-pencil-square-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('1f913593-6953-422f-b758-4fc03aca67773', '29191c49-add8-4167-b512-3f30f792158c', '0', null, 'delete', 'ɾ��', null, 3, 'del', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('1f913593-6953-422f-b758-4fc03aca67774', '29191c49-add8-4167-b512-3f30f792158c', '0', null, 'export', '����', null, 4, 'export', 'fa fa-download', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('1f913593-6953-422f-b758-4fc03aca67770', '29191c49-add8-4167-b512-3f30f792158c', '0', null, 'search', '��ѯ', null, 0, 'search', 'fa fa-search', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('212de008-52b3-42bf-9fe6-23d38d14d7280', '904a8952-aeb3-4a8c-8e14-289aed0d48c8', '0', null, 'search', '��ѯ', null, 0, 'search', 'fa fa-search', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('212de008-52b3-42bf-9fe6-23d38d14d7281', '904a8952-aeb3-4a8c-8e14-289aed0d48c8', '0', null, 'add', '����', null, 1, 'add', 'fa fa-plus', 0);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('212de008-52b3-42bf-9fe6-23d38d14d7282', '904a8952-aeb3-4a8c-8e14-289aed0d48c8', '0', null, 'edit', '�޸�', null, 2, 'edit', 'fa fa-pencil-square-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('212de008-52b3-42bf-9fe6-23d38d14d7283', '904a8952-aeb3-4a8c-8e14-289aed0d48c8', '0', null, 'delete', 'ɾ��', null, 3, 'del', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('212de008-52b3-42bf-9fe6-23d38d14d7284', '904a8952-aeb3-4a8c-8e14-289aed0d48c8', '0', null, 'export', '����', null, 4, 'export', 'fa fa-download', 0);
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

--���żƻ���ˣ���������
delete from sys_wftbprocess where id='998ceb5a-2957-4d3f-a66d-585ceb330653';
delete from sys_wftbactivity where processid='998ceb5a-2957-4d3f-a66d-585ceb330653';
delete from sys_wftbcondition where processid='998ceb5a-2957-4d3f-a66d-585ceb330653';

insert into sys_wftbprocess (ID, OPERDATE, AUTOID, NAME, MESSAGEMODE, REMARK, VERSION, OPERUSER, CREATEUSER, CREATEDATE, PARENTID, CATEGORY, ISSENDPHONEMESSAGE, ISBACKTOBEGINACTIVITY, CODE, ISSINGLEBACK, ISCUSTOMPROCESS, ISSTOP, ORGANID)
values ('998ceb5a-2957-4d3f-a66d-585ceb330653', '2018-12-20 16:54:18', null, '���Ź����ƻ���ˣ���������', null, null, null, 'System', 'System', '2018-12-20 16:54:00', null, null, null, null, '06', null, null, null, null);

insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('6ce8c4c0-1b36-42af-8b2a-2df07635c7f8', '��ʼ�ڵ�', '2018-12-20 16:54:18', 880001, '�ϱ��ƻ�', 'startround', 150, 65, 42, 160, 'System', 'System', '2018-12-20 16:54:00', '998ceb5a-2957-4d3f-a66d-585ceb330653', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('c264dcae-822f-4abf-8ae8-6c5bafa08c59', '��׼�ڵ�', '2018-12-20 16:54:18', 880002, '1�����', 'stepnode', 150, 65, 257, 41, 'System', 'System', '2018-12-20 16:54:00', '998ceb5a-2957-4d3f-a66d-585ceb330653', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('11b09e02-7f63-493a-80ec-b4b15d4031b2', '��׼�ڵ�', '2018-12-20 16:54:18', 880003, '2�����', 'stepnode', 150, 65, 259, 159, 'System', 'System', '2018-12-20 16:54:00', '998ceb5a-2957-4d3f-a66d-585ceb330653', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('1712100a-d991-46cc-984c-11128530937f', '��׼�ڵ�', '2018-12-20 16:54:18', 880004, '����', 'stepnode', 150, 65, 262, 271, 'System', 'System', '2018-12-20 16:54:00', '998ceb5a-2957-4d3f-a66d-585ceb330653', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('f5f746b4-9d17-4d84-b878-ce3d1111bf71', '�����ڵ�', '2018-12-20 16:54:18', 880005, '����', 'endround', 150, 65, 481, 161, 'System', 'System', '2018-12-20 16:54:00', '998ceb5a-2957-4d3f-a66d-585ceb330653', null, null, null, null, null, null, null, null, null, null, null, null, null);

insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('a4e45814-ffd4-4fef-8fef-44b5ba8ab72f', '2018-12-20 16:54:18', 99001, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-20 16:54:00', '6ce8c4c0-1b36-42af-8b2a-2df07635c7f8', 'c264dcae-822f-4abf-8ae8-6c5bafa08c59', '998ceb5a-2957-4d3f-a66d-585ceb330653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('a4e45814-ffd4-4fef-8fef-44b5ba8ab33e', '2018-12-20 16:54:18', 99002, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-20 16:54:00', '6ce8c4c0-1b36-42af-8b2a-2df07635c7f8', '11b09e02-7f63-493a-80ec-b4b15d4031b2', '998ceb5a-2957-4d3f-a66d-585ceb330653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('37ad3394-b85e-4887-ba93-8ba3c9e61f1a', '2018-12-20 16:54:18', 99003, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-20 16:54:00', 'c264dcae-822f-4abf-8ae8-6c5bafa08c59', '11b09e02-7f63-493a-80ec-b4b15d4031b2', '998ceb5a-2957-4d3f-a66d-585ceb330653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('bb9ef8b1-d531-43a6-845e-29bbed25c934', '2018-12-20 16:54:18', 99004, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-20 16:54:00', 'c264dcae-822f-4abf-8ae8-6c5bafa08c59', '6ce8c4c0-1b36-42af-8b2a-2df07635c7f8', '998ceb5a-2957-4d3f-a66d-585ceb330653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('33f63d14-0af0-4322-b7b3-e2c3ab397fa1', '2018-12-20 16:54:18', 99005, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-20 16:54:00', '11b09e02-7f63-493a-80ec-b4b15d4031b2', '1712100a-d991-46cc-984c-11128530937f', '998ceb5a-2957-4d3f-a66d-585ceb330653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('9ba58fe1-2042-4744-8dc3-9116d37e9107', '2018-12-20 16:54:18', 99006, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-20 16:54:00', '11b09e02-7f63-493a-80ec-b4b15d4031b2', 'c264dcae-822f-4abf-8ae8-6c5bafa08c59', '998ceb5a-2957-4d3f-a66d-585ceb330653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('9ba58fe1-2042-4744-8dc3-9116d37e9108', '2018-12-20 16:54:18', 99007, '@{wfFlag}==3', '2', 'System', 'System', '2018-12-20 16:54:00', '11b09e02-7f63-493a-80ec-b4b15d4031b2', '6ce8c4c0-1b36-42af-8b2a-2df07635c7f8', '998ceb5a-2957-4d3f-a66d-585ceb330653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('4f0d4efc-cd66-410a-a818-13f2683e1d14', '2018-12-20 16:54:18', 99008, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-20 16:54:00', '1712100a-d991-46cc-984c-11128530937f', 'f5f746b4-9d17-4d84-b878-ce3d1111bf71', '998ceb5a-2957-4d3f-a66d-585ceb330653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('58336629-d89f-493b-a488-db7978990c8f', '2018-12-20 16:54:18', 99009, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-20 16:54:00', '1712100a-d991-46cc-984c-11128530937f', '11b09e02-7f63-493a-80ec-b4b15d4031b2', '998ceb5a-2957-4d3f-a66d-585ceb330653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('9ba58fe1-2042-4744-8dc3-9116d37e9789', '2018-12-20 16:54:18', 99010, '@{wfFlag}==3', '2', 'System', 'System', '2018-12-20 16:54:00', '1712100a-d991-46cc-984c-11128530937f', '6ce8c4c0-1b36-42af-8b2a-2df07635c7f8', '998ceb5a-2957-4d3f-a66d-585ceb330653');


--���˼ƻ���ˣ���������
delete from sys_wftbprocess where id='2fdb8273-a648-45c3-8c12-4b1ac331f653';
delete from sys_wftbactivity where processid='2fdb8273-a648-45c3-8c12-4b1ac331f653';
delete from sys_wftbcondition where processid='2fdb8273-a648-45c3-8c12-4b1ac331f653';

insert into sys_wftbprocess (ID, OPERDATE, AUTOID, NAME, MESSAGEMODE, REMARK, VERSION, OPERUSER, CREATEUSER, CREATEDATE, PARENTID, CATEGORY, ISSENDPHONEMESSAGE, ISBACKTOBEGINACTIVITY, CODE, ISSINGLEBACK, ISCUSTOMPROCESS, ISSTOP, ORGANID)
values ('2fdb8273-a648-45c3-8c12-4b1ac331f653', '2018-12-20 16:57:27', null, '���˹����ƻ���ˣ���������', null, null, null, 'System', 'System', '2018-12-20 16:57:27', null, null, null, null, '07', null, null, null, null);

insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('759179eb-d625-44b4-ad8e-91808baeaf09', '��ʼ�ڵ�', '2018-12-20 16:57:27', 888001, '�ϱ��ƻ�', 'startround', 150, 65, 82, 21, 'System', 'System', '2018-12-20 16:57:27', '2fdb8273-a648-45c3-8c12-4b1ac331f653', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('bd593dab-3575-447e-8e4e-b9538b172b70', '��׼�ڵ�', '2018-12-20 16:57:27', 888002, '�ϼ��쵼����', 'stepnode', 150, 65, 80, 147, 'System', 'System', '2018-12-20 16:57:27', '2fdb8273-a648-45c3-8c12-4b1ac331f653', null, null, null, null, null, null, null, null, null, null, null, null, null);
insert into sys_wftbactivity (ID, KIND, OPERDATE, AUTOID, NAME, FORMNAME, FORMWIDTH, FORMHEIGHT, GRAPHLEFT, GRAPHTOP, OPERUSER, CREATEUSER, CREATEDATE, PROCESSID, STAYTIMESPAN, INNERHANDLER, ALLOWBACK, ALLOWCANCEL, ISORDERSIGN, ISDYNAMICSIGNER, MOVEPROCESSNAME, UNSHOWNNEXTDIALOG, UNSHOWNPREVDIALOG, TAG, ACTIVITYORDER, SIGNTYPE, AUTONEXTACTIVITYID)
values ('f123248e-0550-4f83-a183-bb13934f7a9b', '�����ڵ�', '2018-12-20 16:57:27', 888003, '����', 'endround', 150, 65, 80, 273, 'System', 'System', '2018-12-20 16:57:27', '2fdb8273-a648-45c3-8c12-4b1ac331f653', null, null, null, null, null, null, null, null, null, null, null, null, null);

insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('dd01595a-d006-4206-9d2e-3bf2c37eb0e2', '2018-12-20 16:57:27', 99901, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-20 16:57:27', '759179eb-d625-44b4-ad8e-91808baeaf09', 'bd593dab-3575-447e-8e4e-b9538b172b70', '2fdb8273-a648-45c3-8c12-4b1ac331f653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('be03b789-d851-4eae-b703-1ae0a1e3e4dd', '2018-12-20 16:57:27', 99902, '@{wfFlag}==1', '2', 'System', 'System', '2018-12-20 16:57:27', 'bd593dab-3575-447e-8e4e-b9538b172b70', 'f123248e-0550-4f83-a183-bb13934f7a9b', '2fdb8273-a648-45c3-8c12-4b1ac331f653');
insert into sys_wftbcondition (ID, OPERDATE, AUTOID, EXPRESSION, REMARK, OPERUSER, CREATEUSER, CREATEDATE, ACTIVITYID, TOACTIVITYID, PROCESSID)
values ('c425f662-f582-4dd1-a868-ffc165d76146', '2018-12-20 16:57:27', 99903, '@{wfFlag}==2', '2', 'System', 'System', '2018-12-20 16:57:27', 'bd593dab-3575-447e-8e4e-b9538b172b70', '759179eb-d625-44b4-ad8e-91808baeaf09', '2fdb8273-a648-45c3-8c12-4b1ac331f653');
/

--������Ϣ
delete from base_dataitemdetail where ITEMID in(select ITEMID from base_dataitem where itemcode in ('WorkPlan','EHSDepartment'));
delete from base_dataitem where itemcode in ('WorkPlan','EHSDepartment');
insert into base_dataitem (ITEMID, PARENTID, ITEMCODE, ITEMNAME, ISTREE, ISNAV, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('1122e766-6ebc-4035-80a0-446546354eee', '7BCDCAA4-2C65-444A-9D04-57F990585C92', 'WorkPlan', 'EHS�����ƻ�', 0, null, 0, 0, 1, null, '2018-12-10 11:18:31', 'System', '��������Ա', '2018-12-10 11:33:43', 'System', '��������Ա');
insert into base_dataitem (ITEMID, PARENTID, ITEMCODE, ITEMNAME, ISTREE, ISNAV, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('69b515cb-ec33-4b93-b330-1541a9053fff', '1122e766-6ebc-4035-80a0-446546354eee', 'EHSDepartment', 'EHS����', 0, null, 2, 0, 1, null, '2018-12-10 11:37:51', 'System', '��������Ա', '2018-12-12 13:41:53', 'System', '��������Ա');

--������ϸ
insert into base_dataitemdetail (ITEMDETAILID, ITEMID, PARENTID, ITEMCODE, ITEMNAME, ITEMVALUE, QUICKQUERY, SIMPLESPELLING, ISDEFAULT, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('d1c870f1-77f3-4f7e-ba36-dfa6da314789', '69b515cb-ec33-4b93-b330-1541a9053fff', '0', null, '668fa31b-7caf-472b-a481-14df870e183e', '013001004001001002', null, '668fa31b-7caf-472b-a481-14df870e183e', null, 1, 0, 1, '��Ŀ���ƣ�����id����Ŀֵ������encode������ʽ��013001004001001002����', '2018-12-10 11:41:04', 'System', '��������Ա', '2018-12-11 19:58:04', 'System', '��������Ա');
/
commit;
