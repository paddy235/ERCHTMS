--NOSAԪ������״̬�ֶ�
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='HRS_NOSAELE' and column_name='STATE';
   if colCount=0 then
      execute immediate 'alter table HRS_NOSAELE add STATE NUMBER(2) default 0';   
   end if;
end;
/
comment on column HRS_NOSAELE.STATE  is 'ɾ��״̬(0:������1����ɾ��)';
/
--NOSA��������״̬�ֶ�
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='HRS_NOSAAREA' and column_name='STATE';
   if colCount=0 then
      execute immediate 'alter table HRS_NOSAAREA add STATE NUMBER(2) default 0';   
   end if;
end;
/
comment on column HRS_NOSAAREA.STATE  is 'ɾ��״̬(0:������1����ɾ��)';
/
