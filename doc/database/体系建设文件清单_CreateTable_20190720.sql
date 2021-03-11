--体系建设文件清单
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
comment on table HRS_FILELIST  is '体系建设文件清单';
-- Add comments to the columns 
comment on column HRS_FILELIST.comid    is '企业id';
comment on column HRS_FILELIST.tmpltnum    is '模板编号';
comment on column HRS_FILELIST.filetype    is '文件类型(0:基础文件,1:风险辨识评估与管控文件)';
comment on column HRS_FILELIST.filename    is '文件名称';
comment on column HRS_FILELIST.filepath  is '文件路径';
/
create or replace trigger trgHRS_FILELIST before insert on HRS_FILELIST for each row
begin  select AUTOID.nextval into :new.autoid from dual;  end;
/

--删除文件清单建设模块
delete from base_modulebutton where moduleid in(select moduleid from base_module where fullname in('文件清单建设'));
delete from base_module where fullname in('文件清单建设');
--插入文件清单建设模块
insert into base_module (MODULEID, PARENTID, ENCODE, FULLNAME, ICON, URLADDRESS, TARGET, ISMENU, ALLOWEXPAND, ISPUBLIC, ALLOWEDIT, ALLOWDELETE, SORTCODE, DELETEMARK, ENABLEDMARK, DESCRIPTION, CREATEDATE, CREATEUSERID, CREATEUSERNAME, MODIFYDATE, MODIFYUSERID, MODIFYUSERNAME)
values ('e92d9383-a5e8-4e1d-8679-815584baf95b', '2', 'FileList', '文件清单建设', 'fa fa-file-word-o', '/FileListManage/FileList/Index?filetype=0', 'iframe', 1, 0, 0, null, null, 14, 0, 1, null, '2019-01-03 13:57:34', 'System', '超级管理员', '2019-01-10 13:28:17', 'System', '超级管理员');
--文件清单建设模块操作功能
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e11', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'onlineEdit', '在线编辑', null, 1, 'onlineEdit', 'fa fa-file-word-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e12', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'upload', '上传', null, 2, 'upload', 'fa fa-upload', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e13', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'delete', '删除', null, 3, 'delete', 'fa fa-trash-o', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e14', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'export', '导出', null, 4, 'export', 'fa fa-download', 1);
insert into base_modulebutton (MODULEBUTTONID, MODULEID, PARENTID, ICON, ENCODE, FULLNAME, ACTIONADDRESS, SORTCODE, ACTIONNAME, FAIMAGE, BUTTONTYPE)
values ('2f003ad8-a727-45d8-893c-77ee0059a4e15', 'e92d9383-a5e8-4e1d-8679-815584baf95b', '0', null, 'search', '查询', null, 0, 'search', 'fa fa-search', 0);
--删除文件清单建设模块的权限配置
delete from base_authorize where itemid in(
'e92d9383-a5e8-4e1d-8679-815584baf95b'--文件清单建设
);
delete from base_authorizedata where resourceid in(
'e92d9383-a5e8-4e1d-8679-815584baf95b'--文件清单建设
);
--插入文件清单建设模块的查询权限配置，操作权限存在代码中实现。
--1.专家、公司管理员：文件清单建设；
--模块菜单
insert into base_authorize (AUTHORIZEID, CATEGORY, OBJECTID, ITEMTYPE, ITEMID, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME)
values ('ea7d2b41-6279-4c71-9f83-82de4b9379a9', 2, '5af22786-e2f2-4a3d-8da3-ecfb16b96f36', 1, 'e92d9383-a5e8-4e1d-8679-815584baf95b', 14, '2019-07-15 10:20:33', 'System', '超级管理员');
--模块数据范围
insert into base_authorizedata (AUTHORIZEDATAID, AUTHORIZETYPE, CATEGORY, OBJECTID, ITEMID, ITEMNAME, RESOURCEID, ISREAD, AUTHORIZECONSTRAINT, SORTCODE, CREATEDATE, CREATEUSERID, CREATEUSERNAME, ITEMCODE)
values ('555a2f00-dd9b-430a-bfe5-e50d4dca0b28', 4, 2, '5af22786-e2f2-4a3d-8da3-ecfb16b96f36', '2f003ad8-a727-45d8-893c-77ee0059a4e15', '查询', 'e92d9383-a5e8-4e1d-8679-815584baf95b', 0, null, 1, '2019-07-15 10:20:34', 'System', '超级管理员', 'search');
--1.编码设置
update base_dataitemdetail set sortcode=0 where itemdetailid ='6064b7b6-c30c-4b1f-91c4-e86ffb0228f6';
update base_dataitemdetail set sortcode=1 where itemdetailid ='a2b4535f-6757-4046-85af-5ccc68ba82c8';
update base_dataitemdetail set sortcode=2 where itemdetailid ='739ab7ff-baee-4ca2-8cba-fedc244ee4d2';
update base_dataitemdetail set sortcode=3 where itemdetailid ='21ca85d5-8620-4748-8f99-036445ca65cb';
commit;
