declare v_cnt int :=0;
begin
    select count(1) into v_cnt from user_tables where table_name = upper('hse_warningcard');
    if v_cnt = 0 then
        execute immediate '
create table hse_warningcard(
cardid varchar2(36) primary key,
cardname varchar2(200),
category varchar2(50),
createuserid varchar2(36),
createtime date,
modifyuserid varchar2(36),
modifytime date,
submituser varchar2(50),
submittime  date,
deptid varchar2(36)
)
        ';
        end if;
end;
/
comment on table hse_warningcard is '指标卡';
comment on column hse_warningcard.cardid is '主键';
comment on column hse_warningcard.cardname is '名称';
comment on column hse_warningcard.category is '类别';
comment on column hse_warningcard.submituser is '提交人';
comment on column hse_warningcard.submittime is '提交时间';

declare v_cnt int :=0;
begin
    select count(1) into v_cnt from user_tables where table_name = upper('hse_checkcontent');
    if v_cnt = 0 then
        execute immediate '
create table hse_checkcontent(
checkcontentid varchar2(36) primary key,
content varchar2(500),
createuserid varchar2(36),
createtime date,
modifyuserid varchar2(36),
modifytime date,
cardid varchar2(36)
)
        ';
        end if;
end;
/
comment on table hse_checkcontent is '指标卡检查内容';
comment on column hse_checkcontent.checkcontentid is '主键';
comment on column hse_checkcontent.content is '内容';
comment on column hse_checkcontent.cardid is '指标卡';

declare v_cnt int :=0;
begin
    select count(1) into v_cnt from user_tables where table_name = upper('hse_checkrecord');
    if v_cnt = 0 then
        execute immediate '
create table hse_checkrecord(
checkrecordid varchar2(36) primary key,
cardid varchar2(36),
cardname varchar2(200),
checkplace varchar2(500),
checkuser varchar2(50),
category varchar2(50),
checktime date,
deptid varchar2(36),
createuserid varchar2(36),
createtime date,
modifyuserid varchar2(36),
modifytime date
)
        ';
        end if;
end;
/
comment on table hse_checkrecord is '检查记录';
comment on column hse_checkrecord.checkrecordid is '主键';
comment on column hse_checkrecord.cardid is '指标卡';
comment on column hse_checkrecord.cardname is '名称';
comment on column hse_checkrecord.checkplace is '地点';
comment on column hse_checkrecord.checkuser is '检查人';
comment on column hse_checkrecord.checktime is '检查时间';
comment on column hse_checkrecord.deptid is '部门';
comment on column hse_checkrecord.category is '类型';

declare v_cnt int :=0;
begin
    select count(1) into v_cnt from user_tables where table_name = upper('hse_checkitem');
    if v_cnt = 0 then
        execute immediate '
create table hse_checkitem(
checkitemid varchar2(36) primary key,
checkrecordid varchar2(36),
checkcontentid varchar2(36),
checkcontent varchar2(500),
num1 int,
num2 int,
num3 int,
num4 int,
dangerous varchar2(500),
measures varchar2(500),
createuserid varchar2(36),
createtime date,
modifyuserid varchar2(36),
modifytime date
)
        ';
        end if;
end;
/
comment on table hse_checkitem is '检查内容';
comment on column hse_checkitem.checkitemid is '主键';
comment on column hse_checkitem.checkrecordid is '检查记录';
comment on column hse_checkitem.checkcontentid is '检查内容';
comment on column hse_checkitem.checkcontent is '检查内容';
comment on column hse_checkitem.num1 is '安全数量';
comment on column hse_checkitem.num2 is '风险数量';
comment on column hse_checkitem.num3 is '紧急的风险数量';
comment on column hse_checkitem.num4 is '不适宜';
comment on column hse_checkitem.dangerous is '存在问题';
comment on column hse_checkitem.Measures is '采取措施';