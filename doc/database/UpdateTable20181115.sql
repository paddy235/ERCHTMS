--��ȫ���ƻ�
declare
   colCount number;
begin
   select count(*) into colCount from user_tab_columns where table_name='BIS_SAFTYCHECKDATARECORD' and column_name='CHECKEDDEPART';
   if colCount=0 then
      execute immediate 'alter table BIS_SAFTYCHECKDATARECORD add CHECKEDDEPARTID varchar(36)';   
      execute immediate 'alter table BIS_SAFTYCHECKDATARECORD add CHECKEDDEPART varchar(500)';   
      execute immediate 'alter table BIS_SAFTYCHECKDATARECORD add ISSYNVIEW varchar(2)';   
   end if;
end;
/
comment on column BIS_SAFTYCHECKDATARECORD.CHECKEDDEPARTID  is '����鵥λ���';
comment on column BIS_SAFTYCHECKDATARECORD.CHECKEDDEPART  is '����鵥λ';
comment on column BIS_SAFTYCHECKDATARECORD.ISSYNVIEW  is '�Ƿ�ͬ���ɼ�';
/
