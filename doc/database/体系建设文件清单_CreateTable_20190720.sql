--��ϵ�����ļ��嵥
declare
   tabCount number;
begin
   select count(*) into tabCount from user_tables where table_name='HRS_FILELIST';
   if tabCount>0 then
      execute immediate 'drop table HRS_FILELIST cascade constraints';
   end if;   
end;
/
create table HRS_FILELIST
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
  comid              nvarchar2(36),
  tmpltnum           NUMBER(2), 
  filetype           NUMBER(2), 
  filename           NVARCHAR2(200),
  filepath           nvarchar2(200)
);
alter table HRS_FILELIST add primary key (id);
-- Add comments to the table 
comment on table HRS_FILELIST  is '��ϵ�����ļ��嵥';
-- Add comments to the columns 
comment on column HRS_FILELIST.comid    is '��ҵid';
comment on column HRS_FILELIST.tmpltnum    is 'ģ����';
comment on column HRS_FILELIST.filetype    is '�ļ�����(0:�����ļ�,1:���ձ�ʶ������ܿ��ļ�)';
comment on column HRS_FILELIST.filename    is '�ļ�����';
comment on column HRS_FILELIST.filepath  is '�ļ�·��';
/
create or replace trigger trgHRS_FILELIST before insert on HRS_FILELIST for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--ɾ���ļ��嵥����ģ��
delete from base_modulebutton where moduleid in(select moduleid from base_module where fullname in('�ļ��嵥����'));
delete from base_module where fullname in('�ļ��嵥����');
--�����ļ��嵥����ģ��
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('e92d9383-a5e8-4e1d-8679-815584baf95b', '2', 'FileList', '�ļ��嵥����', 'fa fa-file-word-o', '/FileListManage/FileList/Index?filetype=0', 'iframe', 1, 0, 0, null, null, 14, 0, 1, null, '2019-01-03 13:57:34', 'System', '��������Ա', '2019-01-10 13:28:17', 'System', '��������Ա');
--�ļ��嵥����ģ���������
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e11', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'onlineEdit', '���߱༭', null, 1, 'onlineEdit', 'fa fa-file-word-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e12', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'upload', '�ϴ�', null, 2, 'upload', 'fa fa-upload', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e13', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'delete', 'ɾ��', null, 3, 'delete', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e14', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'export', '����', null, 4, 'export', 'fa fa-download', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e15', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'search', '��ѯ', null, 0, 'search', 'fa fa-search', 0);
--ɾ���ļ��嵥����ģ���Ȩ������
delete from base_authorize where itemid in(
'e92d9383-a5e8-4e1d-8679-815584baf95b'--�ļ��嵥����
);
delete from base_authorizedata where resourceid in(
'e92d9383-a5e8-4e1d-8679-815584baf95b'--�ļ��嵥����
);
--�����ļ��嵥����ģ��Ĳ�ѯȨ�����ã�����Ȩ�޴��ڴ�����ʵ�֡�
--1.ר�ҡ���˾����Ա���ļ��嵥���裻
--ģ��˵�
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('ea7d2b41-6279-4c71-9f83-82de4b9379a9', 2, '5af22786-e2f2-4a3d-8da3-ecfb16b96f36', 1, 'e92d9383-a5e8-4e1d-8679-815584baf95b', 14, '2019-07-15 10:20:33', 'System', '��������Ա');
--ģ�����ݷ�Χ
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('555a2f00-dd9b-430a-bfe5-e50d4dca0b28', 4, 2, '5af22786-e2f2-4a3d-8da3-ecfb16b96f36', '2f003ad8-a727-45d8-893c-77ee0059a4e15', '��ѯ', 'e92d9383-a5e8-4e1d-8679-815584baf95b', 0, null, 1, '2019-07-15 10:20:34', 'System', '��������Ա', 'search');
--1.��������
update base_dataitemdetail set sortcode=0 where itemdetailid ='6064b7b6-c30c-4b1f-91c4-e86ffb0228f6';
update base_dataitemdetail set sortcode=1 where itemdetailid ='a2b4535f-6757-4046-85af-5ccc68ba82c8';
update base_dataitemdetail set sortcode=2 where itemdetailid ='739ab7ff-baee-4ca2-8cba-fedc244ee4d2';
update base_dataitemdetail set sortcode=3 where itemdetailid ='21ca85d5-8620-4748-8f99-036445ca65cb';
commit;
