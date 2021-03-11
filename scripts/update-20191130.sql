declare v_cnt int :=0;
begin
    select count(1) into v_cnt from user_tables where table_name = upper('hse_observersetting');
    if v_cnt = 0 then
        execute immediate '
create table hse_observersetting(
settingid varchar2(36) primary key,
settingname nvarchar2(50),
cycle varchar2(50),
times int
)
        ';
        end if;
end;
/
comment on table hse_observersetting is '�����ͳ��';
comment on column hse_observersetting.settingid is '����';
comment on column hse_observersetting.settingname is '����';
comment on column hse_observersetting.cycle is '����';
comment on column hse_observersetting.times is '����';


declare v_cnt int :=0;
begin
    select count(1) into v_cnt from user_tables where table_name = upper('hse_observersettingitem');
    if v_cnt = 0 then
        execute immediate '
create table hse_observersettingitem(
itemid varchar2(36) primary key,
settingid varchar(36),
deptid varchar(36)
)
        ';
        end if;
end;
/
comment on table hse_observersettingitem is '�����ͳ�Ʋ���';
comment on column hse_observersettingitem.itemid is '����';
comment on column hse_observersettingitem.settingid is '���';
comment on column hse_observersettingitem.deptid is '����';

select sys_guid() from dual;



insert into hse_observersetting (settingid, settingname, cycle, times) values ('96d9ce5f-6e1b-45f9-96c4-de0b3dc24160', 'Ԥ��ָ�꿨', null, 0);
insert into hse_observersetting (settingid, settingname, cycle, times) values ('e0da1280-ad07-4867-bba0-90103effa0be', '��ȫ�۲쿨', null, 0);
insert into hse_observersetting (settingid, settingname, cycle, times) values ('b9ee9a7b-0ae9-4b29-bbc2-b7fe02eade26', 'HSE��������', null, 0);