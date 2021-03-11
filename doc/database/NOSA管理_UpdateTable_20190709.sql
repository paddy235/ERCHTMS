--NOSA元素增加状态字段
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='HRS_NOSAELE' and column_name='STATE';
   if colCount=0 then
      execute immediate 'alter table HRS_NOSAELE add STATE NUMBER(2) default 0';   
   end if;
end;
/
comment on column HRS_NOSAELE.STATE  is '删除状态(0:正常，1：已删除)';
/
--NOSA区域增加状态字段
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='HRS_NOSAAREA' and column_name='STATE';
   if colCount=0 then
      execute immediate 'alter table HRS_NOSAAREA add STATE NUMBER(2) default 0';   
   end if;
end;
/
comment on column HRS_NOSAAREA.STATE  is '删除状态(0:正常，1：已删除)';
/
