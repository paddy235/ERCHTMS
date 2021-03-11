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
comment on table hse_observersetting is '参与度统计';
comment on column hse_observersetting.settingid is '主键';
comment on column hse_observersetting.settingname is '名称';
comment on column hse_observersetting.cycle is '周期';
comment on column hse_observersetting.times is '次数';


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
comment on table hse_observersettingitem is '参与度统计部门';
comment on column hse_observersettingitem.itemid is '主键';
comment on column hse_observersettingitem.settingid is '外键';
comment on column hse_observersettingitem.deptid is '部门';

select sys_guid() from dual;



insert into hse_observersetting (settingid, settingname, cycle, times) values ('96d9ce5f-6e1b-45f9-96c4-de0b3dc24160', '预警指标卡', null, 0);
insert into hse_observersetting (settingid, settingname, cycle, times) values ('e0da1280-ad07-4867-bba0-90103effa0be', '安全观察卡', null, 0);
insert into hse_observersetting (settingid, settingname, cycle, times) values ('b9ee9a7b-0ae9-4b29-bbc2-b7fe02eade26', 'HSE自我评估', null, 0);